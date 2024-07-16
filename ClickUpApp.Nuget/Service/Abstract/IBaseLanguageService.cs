using ClickUpApp.Nuget.Dto;

namespace ClickUpApp.Nuget.Service
{
    public interface IBaseLanguageService<TEntity, TLanguageEntity, TContext>
    {
        Task<List<DefinationWithLanguageDto>> GetDefinationWithLanguageAsync(); 
    }
}