using FirstProject.Application.Common;
using FirstProject.Application.Common.Permissions;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Setup.Commands.SeedAdminPermissions;

/// <summary>
/// Tạo tất cả permissions trong project (từ PermissionCodes), tạo role "Admin" nếu chưa có,
/// gán tất cả permissions cho Admin. Chỉ cần gọi 1 lần (vd từ Postman) khi setup.
/// </summary>
public record SeedAdminPermissionsCommand : IRequest<Result<SeedAdminPermissionsResult>>;

public record SeedAdminPermissionsResult(
    int PermissionsCreated,
    bool AdminRoleCreated,
    Guid AdminRoleId,
    int PermissionsAssignedToAdmin);

internal sealed class SeedAdminPermissionsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SeedAdminPermissionsCommand, Result<SeedAdminPermissionsResult>>
{
    private static readonly (string Code, string Description)[] AllPermissionCodes =
    {
        (PermissionCodes.Users_List, "Xem danh sách user"),
        (PermissionCodes.Users_Create, "Tạo user"),
        (PermissionCodes.Users_Update, "Cập nhật user"),
        (PermissionCodes.Users_Delete, "Xóa user"),
        (PermissionCodes.Users_View, "Xem chi tiết user"),
        (PermissionCodes.Roles_List, "Xem danh sách role"),
        (PermissionCodes.Roles_Create, "Tạo role"),
        (PermissionCodes.Roles_Update, "Cập nhật role"),
        (PermissionCodes.Roles_Delete, "Xóa role"),
        (PermissionCodes.Roles_View, "Xem chi tiết role"),
        (PermissionCodes.Roles_AssignPermission, "Gán/bỏ permission cho role"),
        (PermissionCodes.Permissions_List, "Xem danh sách permission"),
        (PermissionCodes.Permissions_Create, "Tạo permission"),
        (PermissionCodes.Permissions_Update, "Cập nhật permission"),
        (PermissionCodes.Permissions_Delete, "Xóa permission"),
        (PermissionCodes.Groups_List, "Xem danh sách group"),
        (PermissionCodes.Groups_Create, "Tạo group"),
        (PermissionCodes.Groups_Update, "Cập nhật group"),
        (PermissionCodes.Groups_Delete, "Xóa group"),
        (PermissionCodes.Groups_View, "Xem chi tiết group"),
        (PermissionCodes.Groups_AssignPermission, "Gán/bỏ permission cho group"),
    };

    public async Task<Result<SeedAdminPermissionsResult>> Handle(SeedAdminPermissionsCommand request, CancellationToken cancellationToken)
    {
        int permissionsCreated = 0;
        var existingCodes = await unitOfWork.Repository<Permission>().Entities
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);

        foreach (var (code, description) in AllPermissionCodes)
        {
            if (existingCodes.Contains(code))
                continue;

            var permission = new Permission { Code = code, Description = description };
            await unitOfWork.Repository<Permission>().AddAsync(permission);
            existingCodes.Add(code);
            permissionsCreated++;
        }

        if (permissionsCreated > 0)
            await unitOfWork.Save(cancellationToken);

        const string adminRoleName = "Admin";
        var adminRole = await unitOfWork.Repository<Role>().Entities
            .FirstOrDefaultAsync(r => r.RoleName == adminRoleName, cancellationToken);

        bool adminRoleCreated = false;
        if (adminRole is null)
        {
            adminRole = new Role { RoleName = adminRoleName };
            await unitOfWork.Repository<Role>().AddAsync(adminRole);
            await unitOfWork.Save(cancellationToken);
            adminRoleCreated = true;
        }

        var allPermissionIds = await unitOfWork.Repository<Permission>().Entities
            .Select(p => p.PermissionId)
            .ToListAsync(cancellationToken);

        adminRole = await unitOfWork.Repository<Role>().Entities
            .FirstOrDefaultAsync(r => r.RoleId == adminRole!.RoleId, cancellationToken);
        if (adminRole is null)
            return Result<SeedAdminPermissionsResult>.Failure("Không tìm thấy role Admin.");

        adminRole.ClearPermissionIds();
        foreach (var pid in allPermissionIds.Distinct())
            adminRole.AddPermissionId(new Role.PermissionId(pid));

        await unitOfWork.Save(cancellationToken);

        var result = new SeedAdminPermissionsResult(
            permissionsCreated,
            adminRoleCreated,
            adminRole.RoleId,
            allPermissionIds.Count);

        return Result<SeedAdminPermissionsResult>.Success(result);
    }
}
