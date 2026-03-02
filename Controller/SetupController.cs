using FirstProject.Application.Common;
using FirstProject.Application.Features.Setup.Commands.SeedAdminPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint setup một lần: tạo tất cả permissions + role Admin + gán hết quyền cho Admin.
/// [AllowAnonymous] để có thể gọi từ Postman khi chưa có user/phân quyền.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SetupController : ControllerBase
{
    private readonly IMediator _mediator;

    public SetupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tạo tất cả quyền trong project, tạo role Admin nếu chưa có, gán tất cả quyền cho Admin.
    /// Gọi 1 lần từ Postman (không cần token). Sau đó gán role Admin cho user của bạn.
    /// </summary>
    [HttpPost("seed-admin-permissions")]
    public async Task<ActionResult<Result<SeedAdminPermissionsResult>>> SeedAdminPermissions()
    {
        var result = await _mediator.Send(new SeedAdminPermissionsCommand());
        return Ok(result);
    }
}
