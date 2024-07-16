using ClickUpApp.Nuget;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ClickUpApp.Nuget
{
    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class Repository<T, TContext> : IRepository<T, TContext> where TContext : DbContext
    where T : BaseIdEntity
    {
        /// <summary>
        /// context
        /// </summary>
        /// <value></value>
        public TContext Context { get; }
        /// <summary>
        /// DbContext Injector ctor
        /// </summary>
        /// <param name="_context"></param>
        public Repository(TContext _context)
        {
            Guard.NotNull<DbContext>(_context, nameof(DbContext));
            Context = _context;
        }

        /// <summary>
        /// Implemenets insert item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> TryInsertItemAsync(T item)
        {
            if (item is BaseIdCreateUpdateEntity entity)
            {
                entity.CreateDate = DateTime.Now;
            }

            await Context.Set<T>().AddAsync(item);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Inserts list
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public async Task<bool> TryInsertListAsync(ICollection<T> itemList)
        {
            if (itemList is BaseIdCreateUpdateEntity entity)
            {
                entity.CreateDate = DateTime.Now;
            }

            await Context.Set<T>().AddRangeAsync(itemList);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Update item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> TryUpdateItemAsync(T item)
        {
            if (item is BaseIdCreateUpdateEntity entity)
            {
                entity.UpdateDate = DateTime.Now;
            }

            Context.Set<T>().Update(item);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates multiple item async
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public async Task<bool> TryUpdateListAsync(ICollection<T> itemList)
        {
            if (itemList is BaseIdCreateUpdateEntity entity)
            {
                entity.UpdateDate = DateTime.Now;
            }

            Context.Set<T>().UpdateRange(itemList);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Delete item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> TryDeleteItemAsync(T item)
        {
            Context.Set<T>().Remove(item);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public async Task<bool> TryDeleteListAsync(ICollection<T> itemList)
        {
            Context.RemoveRange(itemList);
            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// get item with navigation props
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProps"></param>
        /// <returns></returns>
        public async Task<T> GetItemAsync(Expression<Func<T, bool>> predicate, params string[] includeProps)
        {
            IQueryable<T> query = Context.Set<T>();
            if (includeProps != null && includeProps.Length > 0)
            {
                foreach (var item in includeProps)
                {
                    query = query.Include(item);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// get filtered items with navigation props
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProps"></param>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetFilteredItemsAsync(Expression<Func<T, bool>> predicate, params string[] includeProps)
        {
            IQueryable<T> query = Context.Set<T>();
            if (includeProps != null && includeProps.Length > 0)
            {
                foreach (var item in includeProps)
                {
                    query = query.Include(item);
                }
            }

            return await Task.FromResult(query.Where(predicate));
        }

        /// <summary>
        /// get filtered items with navigation props
        /// </summary>
        /// <param name="includeProps"></param>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetAllItemsAsync(params string[] includeProps)
        {
            IQueryable<T> query = Context.Set<T>();
            if (includeProps != null && includeProps.Length > 0)
            {
                foreach (var item in includeProps)
                {
                    query = query.Include(item);
                }
            }

            return await Task.FromResult(query);
        }

        public async Task<int> TruncatePartitionAsync(string schema, string tableName, int partitionStatusOrder)
        {
            return await ExecuteQueryAsync($"TRUNCATE TABLE {schema}.{tableName} WITH (PARTITIONS ({partitionStatusOrder}))");
        }

        public async Task<IQueryable<T>> FromRawQueryAsync(string query)
        {
            return await Task.FromResult(Context.Set<T>().FromSqlRaw(query));
        }

        public async Task<int> ExecuteQueryAsync(string query)
        {
            return await Task.FromResult(Context.Database.ExecuteSqlRaw(query));
        }
    }
}