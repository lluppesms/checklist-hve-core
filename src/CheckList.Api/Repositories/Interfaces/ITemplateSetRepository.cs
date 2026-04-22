namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ITemplateSetRepository
{
    Task<IEnumerable<TemplateSet>> GetAllAsync();
    Task<TemplateSet?> GetByIdWithChildrenAsync(int id);
}
