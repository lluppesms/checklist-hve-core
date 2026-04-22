namespace CheckList.Web.Repositories.Interfaces;

using CheckList.Web.Data.Models;

public interface ITemplateListRepository
{
    Task<IEnumerable<TemplateList>> GetBySetAsync(int setId);
    Task<TemplateList?> GetByIdAsync(int id);
}
