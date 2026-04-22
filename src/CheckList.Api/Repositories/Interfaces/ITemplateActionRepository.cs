namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ITemplateActionRepository
{
    Task<IEnumerable<TemplateAction>> GetByCategoryAsync(int categoryId);
    Task<TemplateAction?> GetByIdAsync(int id);
}
