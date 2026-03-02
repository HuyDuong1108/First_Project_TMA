//Flie này dùng để store các event của entity thành 1 collection và có thêm các helper methods để thêm, xóa, làm sạch các event 
//Đại ý là dùng để lưu event của entity vào collection để xử lý sau khi entity được lưu vào database 

using System.ComponentModel.DataAnnotations.Schema;
using FirstProject.Domain.Common.Interfaces;
namespace FirstProject.Domain.Common;

public abstract class BaseEntity : IEntity
{
    private readonly List<BaseEvent> _domainEvents = new();
 
    public int Id { get; set; }
 
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();
 
    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear(); 
}