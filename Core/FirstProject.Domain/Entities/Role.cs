using FirstProject.Domain.Common;
namespace FirstProject.Domain.Entities
{
    public class Role : BaseEntity
    {
        public record PermissionId(Guid Value);

        public Guid RoleId { get; set; } = Guid.NewGuid();
        public required string RoleName { get; set; }

        // PermissionIds is a list of PermissionId objects
        private readonly List<PermissionId> _permissionIds = [];
        public IReadOnlyCollection<PermissionId> PermissionIds => _permissionIds.AsReadOnly();
        public void AddPermissionId(PermissionId permissionId)
        {
            _permissionIds.Add(permissionId);
        }
        public void RemovePermissionId(PermissionId permissionId)
        {
            _ = _permissionIds.Remove(permissionId);
        }
        public void ClearPermissionIds()
        {
            _permissionIds.Clear();
        }
    }
}