// file này là interface con của IEnity . File này thêm một số trường để có đc thông tin của Entity như các trường thời gian tạo ,tạo bởi ai,update bởi ai, thời gian?

namespace FirstProject.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    int CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    int UpdatedBy { get; set; }
    DateTime UpdatedAt { get; set; }
}