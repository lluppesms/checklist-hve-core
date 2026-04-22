namespace CheckList.Web.Repositories.Interfaces;

using CheckList.Web.Data.Models;

public interface ITemplateCategoryRepository
{
    Task<IEnumerable<TemplateCategory>> GetByListAsync(int listId);
    Task<TemplateCategory?> GetByIdAsync(int id);
}
