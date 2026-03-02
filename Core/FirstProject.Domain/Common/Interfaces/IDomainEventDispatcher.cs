// file này dùng để tạo thông báo xuyên suốt các thành phần của application vd: thanh toán thành công gửi thông báo đến các thành phần khác để gửi mail hay thông báo chẳng hạn
namespace FirstProject.Domain.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}