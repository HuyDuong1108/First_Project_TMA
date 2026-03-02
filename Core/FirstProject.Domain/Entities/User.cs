using FirstProject.Domain.Common;
namespace FirstProject.Domain.Entities
{
    public record RoleId(Guid Value);
    public record GroupId(Guid Value);
    public class User : BaseEntity
    {
        public Guid UserId { get; set; }=Guid.NewGuid();
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
        public required bool Status { get; set; }
        public required DateTime CreatedAt { get; set; }

        // RoleIds is a list of RoleId objects
        private readonly List<RoleId> _roleIds = [];
        public IReadOnlyCollection<RoleId> RoleIds => _roleIds.AsReadOnly();
        public void AddRoleId(RoleId roleId)
        {
            _roleIds.Add(roleId);
        }

        public void RemoveRoleId(RoleId roleId)
        {
            _roleIds.Remove(roleId);
        }

        public void ClearRoleIds()
        {
            _roleIds.Clear();
        }
        // GroupIds is a list of GroupId objects
        private readonly List<GroupId> _groupIds = [];
        public IReadOnlyCollection<GroupId> GroupIds => _groupIds.AsReadOnly();
        public void AddGroupId(GroupId groupId)
        {
            _groupIds.Add(groupId);
        }
        public void RemoveGroupId(GroupId groupId)
        {
            _groupIds.Remove(groupId);
        }
    }
}