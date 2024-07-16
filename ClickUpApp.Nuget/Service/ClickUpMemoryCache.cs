using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;

namespace ClickUpApp.Nuget.Service
{ 
    public abstract class ClickUpAppMemoryCache<TEntity> : IWorhhubMemoryCache<TEntity>, IDisposable
    where TEntity : class
    {
        private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; set; }
        private readonly object cacheLock = new();
        private readonly string key = typeof(TEntity).Name + "Cache";
        private readonly IMemoryCache cache;

        protected readonly IServiceScopeFactory Factory;

        public ClickUpAppMemoryCache(IServiceScopeFactory _factory, IMemoryCache _cache)
        {
            Factory = _factory;
            cache = _cache;
        }

        public async Task<TEntity> TryGetItemAsync(Func<TEntity, bool> predicate)
        {
            var valueList = await TryGetListAsync();
            return await Task.FromResult(valueList?.FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<TEntity>> TryGetItemListAsync(Func<TEntity, bool> predicate)
        {
            var valueList = await TryGetListAsync();
            return await Task.FromResult(valueList?.Where(predicate));
        }

        public async Task<TEntity> TryGetSingleItemAsync()
        {
            var valueList = await TryGetListAsync();

            return await Task.FromResult(valueList?.FirstOrDefault());
        }

        public async Task<List<TEntity>> TryGetListAsync()
        {
            var valueList = await GetList();

            if (valueList == null || (MemoryCacheEntryOptions != null && MemoryCacheEntryOptions.AbsoluteExpiration < DateTime.Now))
            {
                await TryReloadCacheAsync();
                valueList = await GetList(true); // last try, if null we can't do anything more.
            }

            return valueList;
        }

        public Task<List<TEntity>> GetList(bool isRecursed = false)
        {
            var result = (List<TEntity>)cache.Get(key);
            if (result == null && isRecursed)
            {
                result = new List<TEntity>();
            }

            return Task.FromResult(result);
        }

        protected void SetCacheList(List<TEntity> valueList)
        {
            lock (cacheLock)
            {
                cache.Set(key, valueList, MemoryCacheEntryOptions);
            }
        }

        public async Task TryReloadCacheAsync()
        {
            IEnumerable<TEntity> tempList = await GetEntityListAsync();
            MemoryCacheEntryOptions = SetPolicy();

            if (!tempList.Any())
            {
                return; // exception handler
            }

            SetCacheList(tempList.ToList());
        }

        public async Task TryPushItems(List<TEntity> itemList)
        {
            var valueList = await TryGetListAsync();

            valueList ??= new List<TEntity>();

            valueList.AddRange(itemList);

            SetCacheList(valueList);
        }

        public async Task TryPushItem(TEntity item)
        {
            var listItem = new List<TEntity>() { item };

            await TryPushItems(listItem);
        }

        protected virtual MemoryCacheEntryOptions SetPolicy()
        {
            return null;
        }

        public object GetPolicy()
        {
            return MemoryCacheEntryOptions;
        }

        public int GetItemCount()
        {
            throw new NotImplementedException();
        }

        public Type GetCacheEntity()
        {
            return typeof(TEntity);
        }

        public virtual Task<List<TEntity>> GetEntityListAsync()
        {
            return Task.FromResult(new List<TEntity>());
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}