using FirstProject.Domain.Common;
namespace FirstProject.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public Guid PermissionId { get; set; }=Guid.NewGuid();
        public required string Code { get; set; }
       public required string Description { get; set; }
    }
}