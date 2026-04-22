namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ITemplateListRepository
{
    Task<IEnumerable<TemplateList>> GetBySetAsync(int setId);
    Task<TemplateList?> GetByIdAsync(int id);
}
