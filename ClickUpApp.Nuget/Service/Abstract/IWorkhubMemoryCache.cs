 
namespace ClickUpApp.Nuget.Service
{ 
    public interface IWorhhubMemoryCache<TEntity> : ICacheable where TEntity : class
    {
        Task<TEntity> TryGetSingleItemAsync();
        Task<TEntity> TryGetItemAsync(Func<TEntity, bool> predicate);
        Task<IEnumerable<TEntity>> TryGetItemListAsync(Func<TEntity, bool> predicate);
        Task<List<TEntity>> TryGetListAsync();
        Task<List<TEntity>> GetList(bool isRecursed = false);
        Task TryPushItems(List<TEntity> itemList);
        Task TryPushItem(TEntity item);
        Task<List<TEntity>> GetEntityListAsync();
    }
}