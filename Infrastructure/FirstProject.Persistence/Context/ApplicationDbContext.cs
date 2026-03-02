using Microsoft.EntityFrameworkCore;
using FirstProject.Domain.Common.Interfaces;
using FirstProject.Domain.Entities;
using System.Reflection;
using FirstProject.Domain.Common;
using FirstProject.Application.Interfaces;

namespace FirstProject.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
      IDomainEventDispatcher dispatcher) : DbContext(options), IApplicationDbContext
    {
        private readonly IDomainEventDispatcher _dispatcher = dispatcher;

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // User: PK is UserId (Guid); RoleIds map to existing table User_roles (UserId, RoleId)
            _ = modelBuilder.Entity<User>(static e =>
            {
                _ = e.HasKey(static x => x.UserId);
                _ = e.Property(static x => x.UserId).ValueGeneratedNever();
                _ = e.Property(static x => x.UserId).ValueGeneratedOnAdd();
                _ = e.Ignore(static x => x.Id);
                _ = e.Navigation(static u => u.RoleIds).HasField("_roleIds");
                _ = e.OwnsMany(static u => u.RoleIds, static r =>
                {
                    _ = r.ToTable("User_roles");
                    _ = r.Property("UserId").HasColumnName("User_Id");
                    _ = r.Property(static x => x.Value).HasColumnName("RoleId");
                    _ = r.WithOwner();
                    _ = r.HasKey("UserId", "Value");
                });
                // GroupIds is a list of GroupId objects
                _ = e.Navigation(static u => u.GroupIds).HasField("_groupIds");
                _ = e.OwnsMany(static u => u.GroupIds, static g =>
                {
                    _ = g.ToTable("User_groups");
                    _ = g.Property("UserId").HasColumnName("User_Id");
                    _ = g.Property(static x => x.Value).HasColumnName("GroupId");
                    _ = g.WithOwner();
                    _ = g.HasKey("UserId", "Value");
                });
            });
            // Role: PK is RoleId (Guid); PermissionIds map to existing table Role_permissions (RoleId, PermissionId)

            _ = modelBuilder.Entity<Role>(static e =>
            {
                _ = e.HasKey(static x => x.RoleId);
                _ = e.Property(static x => x.RoleId).ValueGeneratedNever();
                _ = e.Property(static x => x.RoleId).ValueGeneratedOnAdd();
                _ = e.Ignore(static x => x.Id);
                _ = e.Navigation(static r => r.PermissionIds).HasField("_permissionIds");
                _ = e.OwnsMany(static r => r.PermissionIds, static p =>
                {
                    _ = p.ToTable("Role_permissions");
                    _ = p.Property("RoleId").HasColumnName("RoleId");
                    _ = p.Property(static x => x.Value).HasColumnName("PermissionId");
                    _ = p.WithOwner();
                    _ = p.HasKey("RoleId", "Value");
                });
            });
            _ = modelBuilder.Entity<Permission>(static e =>
            {
                _ = e.HasKey(static x => x.PermissionId);
                _ = e.Property(static x => x.PermissionId).ValueGeneratedNever();
                _ = e.Property(static x => x.PermissionId).ValueGeneratedOnAdd();
                _ = e.Ignore(static x => x.Id);
            });
            _ = modelBuilder.Entity<Group>(static e =>
            {
                _ = e.HasKey(static x => x.GroupId);
                _ = e.Property(static x => x.GroupId).ValueGeneratedNever();
                _ = e.Property(static x => x.GroupId).ValueGeneratedOnAdd();
                _ = e.Ignore(static x => x.Id);
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_dispatcher == null)
            {
                return result;
            }

            BaseEntity[] entitiesWithEvents = [.. ChangeTracker.Entries<BaseEntity>()
                .Select(static e => e.Entity)
                .Where(static e => e.DomainEvents.Count != 0)];

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}