using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ClickUpApp.Nuget
{
    /// <summary>
    /// Repository common interface
    /// </summary>
    public interface IRepository<T, TContext> where T : class where TContext : DbContext
    {
        /// <summary>
        /// context
        /// </summary>
        /// <value></value>
        public TContext Context { get; }
        /// <summary>
        /// Insert item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> TryInsertItemAsync(T item); 
        /// <summary>
        /// Insert list of item async
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        Task<bool> TryInsertListAsync(ICollection<T> itemList);
        /// <summary>
        /// Updates item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> TryUpdateItemAsync(T item);
        /// <summary>
        /// Updates multiple item async
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        Task<bool> TryUpdateListAsync(ICollection<T> itemList);
        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> TryDeleteItemAsync(T item);
        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        Task<bool> TryDeleteListAsync(ICollection<T> itemList);
        /// <summary>
        /// Gets T type item by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProps"></param>
        /// <returns></returns>
        Task<T> GetItemAsync(Expression<Func<T, bool>> predicate, params string[] includeProps);
        /// <summary>
        /// gets all items
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProps"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetFilteredItemsAsync(Expression<Func<T, bool>> predicate, params string[] includeProps);
        /// <summary>
        /// gets all items
        /// </summary>
        /// <param name="includeProps"></param>
        Task<IQueryable<T>> GetAllItemsAsync(params string[] includeProps);
        /// <summary>
        /// Truncates partition
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <param name="partitionStatusOrder"></param>
        /// <returns></returns>
        Task<int> TruncatePartitionAsync(string schema, string tableName, int partitionStatusOrder);
        /// <summary>
        /// operates by raw query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IQueryable<T>> FromRawQueryAsync(string query);
        /// <summary>
        /// Exec query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<int> ExecuteQueryAsync(string query);
    }
}