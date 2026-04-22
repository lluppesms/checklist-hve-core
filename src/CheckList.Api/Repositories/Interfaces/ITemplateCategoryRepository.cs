namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ITemplateCategoryRepository
{
    Task<IEnumerable<TemplateCategory>> GetByListAsync(int listId);
    Task<TemplateCategory?> GetByIdAsync(int id);
}
