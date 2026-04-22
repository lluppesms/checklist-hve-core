namespace CheckList.Web.Repositories.Interfaces;

using CheckList.Web.Data.Models;

public interface ITemplateActionRepository
{
    Task<IEnumerable<TemplateAction>> GetByCategoryAsync(int categoryId);
    Task<TemplateAction?> GetByIdAsync(int id);
}
