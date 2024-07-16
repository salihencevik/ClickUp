using Microsoft.EntityFrameworkCore;

namespace ClickUpApp.Nuget
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork<T> : IDisposable where T : DbContext
    {
        Task CommitAsync();
        Task RollbackAsync();
        Task BeginTransactionAsync();
        IRepository<TRepo, T> GetRepository<TRepo>() where TRepo : BaseIdEntity;
    }
}