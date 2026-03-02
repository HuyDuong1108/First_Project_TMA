// Interfaces này dùng để 1 work pattern cho phép lưu thay đổi từ nhiều repository cùng lúc
using FirstProject.Domain.Common;

namespace FirstProject.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Save(CancellationToken cancellationToken);
        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task Rollback();
    }
}