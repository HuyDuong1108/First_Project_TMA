namespace FirstProject.Application.Common.Permissions;

/// <summary>
/// Mã permission (trùng với Permission_Code trong DB)_ Dùng cho [RequirePermission(PermissionCodes_X)]_
/// Khi tạo permission trong DB, Code nên trùng với constant ở đây_
/// </summary>
public static class PermissionCodes
{
    public const string Users_List = "Users_List";
    public const string Users_Create = "Users_Create";
    public const string Users_Update = "Users_Update";
    public const string Users_Delete = "Users_Delete";
    public const string Users_View = "Users_View";

    public const string Roles_List = "Roles_List";
    public const string Roles_Create = "Roles_Create";
    public const string Roles_Update = "Roles_Update";
    public const string Roles_Delete = "Roles_Delete";
    public const string Roles_View = "Roles_View";
    public const string Roles_AssignPermission = "Roles_AssignPermission";

    public const string Permissions_List = "Permissions_List";
    public const string Permissions_Create = "Permissions_Create";
    public const string Permissions_Update = "Permissions_Update";
    public const string Permissions_Delete = "Permissions_Delete";

    public const string Groups_List = "Groups_List";
    public const string Groups_Create = "Groups_Create";
    public const string Groups_Update = "Groups_Update";
    public const string Groups_Delete = "Groups_Delete";
    public const string Groups_View = "Groups_View";
    public const string Groups_AssignPermission = "Groups_AssignPermission";

}
