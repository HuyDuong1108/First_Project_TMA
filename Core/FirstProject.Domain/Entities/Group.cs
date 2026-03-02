using FirstProject.Domain.Common;
namespace FirstProject.Domain.Entities
{
    public class Group : BaseEntity
    {
        public Guid GroupId { get; set; }=Guid.NewGuid();
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}