
namespace ClickUpApp.Nuget.Service
{ 
    public interface ICacheable 
    {
        Task TryReloadCacheAsync();
        int GetItemCount();
        Type GetCacheEntity();
        object GetPolicy();
    }
}