//file này giống với file IAuditableEntity nhưng thêm thông tin về người tạo và người cập nhật

using FirstProject.Domain.Common.Interfaces;
namespace FirstProject.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}