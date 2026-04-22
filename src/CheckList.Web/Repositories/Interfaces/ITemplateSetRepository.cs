namespace CheckList.Web.Repositories.Interfaces;

using CheckList.Web.Data.Models;

public interface ITemplateSetRepository
{
    Task<IEnumerable<TemplateSet>> GetAllAsync();
    Task<TemplateSet?> GetByIdWithChildrenAsync(int id);
}
