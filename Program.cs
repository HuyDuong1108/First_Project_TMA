using System.Text;
using First_project.Components;
using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Infrastructure;
using FirstProject.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FirstProject.Persistence;
using FirstProject.Domain.Common;
using FirstProject.Domain.Common.Interfaces;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

// Application layer: MediatR + AutoMapper (logic nằm trong CreateUserCommand handler)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Result<>).Assembly));
builder.Services.AddAutoMapper(typeof(Result<>).Assembly);

// Domain events: dispatch after SaveChanges via MediatR
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Auth: JWT + Password hasher + Current user (Features/Auth use these)
builder.Services.AddInfrastructureAuth(builder.Configuration);
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings is missing in configuration.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Data: UnitOfWork + DbContext (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");
// Debug: log connection (mask password) to verify which config is loaded
var masked = System.Text.RegularExpressions.Regex.Replace(connectionString, @"Password=[^;]*", "Password=***");
System.Console.WriteLine("[Config] Using: " + masked);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "First Project API",
        Version = "v1",
        Description = "API theo kiến trúc DDD & CQRS: Auth, Users, Roles, Permissions, Groups, Setup. Dùng **Authorize** (nút bên trên) để nhập JWT Bearer token cho các endpoint yêu cầu đăng nhập."
    });

    // JWT Bearer: cho phép nhập token trong Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token (chỉ cần token, không cần gõ 'Bearer ')"
    });
    // Bắt buộc: AddSecurityRequirement để Swagger UI thực sự gửi header Authorization khi gọi API (nếu không có thì dù đã nhập token vẫn không gửi kèm → 401, trông như không có phản hồi).
    options.AddSecurityRequirement(document =>
    {
        var schemeRef = new OpenApiSecuritySchemeReference("Bearer", document, null);
        var requirement = new OpenApiSecurityRequirement { [schemeRef] = new List<string>() };
        return requirement;
    });

    // Nhóm endpoint theo tên Controller (Auth, Users, Roles, ...)
    options.TagActionsBy(api =>
    {
        if (api.GroupName != null) return new[] { api.GroupName };
        var controller = api.ActionDescriptor.RouteValues.TryGetValue("controller", out var c) ? (c ?? "Api") : "Api";
        return new[] { controller };
    });
    options.DocInclusionPredicate((_, api) => true);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "First Project API v1");
        c.DocumentTitle = "First Project API (DDD & CQRS)";
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();

// Chạy CheckNameSpaceExist khi ứng dụng start (ví dụ)
CheckNameSpaceExist.CheckNameSpaceUserExist("FirstProject.Domain.Entities.Group");

app.Run();
