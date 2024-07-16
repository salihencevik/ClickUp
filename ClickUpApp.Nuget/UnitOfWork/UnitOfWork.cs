using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClickUpApp.Nuget
{ 
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        private IDbContextTransaction transaction;
        private readonly T context;
 
        public UnitOfWork(T _context)
        {
            context = _context;
        }
 
        public IRepository<TRepo, T> GetRepository<TRepo>() where TRepo : BaseIdEntity 
        {
            return new Repository<TRepo, T>(context);
        }

        /// <summary>
        /// Commits
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            if (IsInTransaction())
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        /// <summary>
        /// rollback all changes
        /// </summary>
        public async Task RollbackAsync()
        {
            if (IsInTransaction())
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        /// <summary>
        /// begins transaction
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            if (IsInTransaction())
            {
                await RollbackAsync();
            }

            transaction = await context.Database.BeginTransactionAsync();
        }

        private bool IsInTransaction()
        {
            if (transaction == null)
            {
                return false;
            }

            return transaction.TransactionId != Guid.Empty;
        }

        /// <summary>
        /// Dispose operation
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }

        /// <summary>
        /// Dispose operation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}