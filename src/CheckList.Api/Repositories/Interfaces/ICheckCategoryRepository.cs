namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ICheckCategoryRepository
{
    Task<IEnumerable<CheckCategory>> GetByListAsync(int listId);
    Task<CheckCategory?> GetByIdAsync(int id);
    Task<CheckCategory> CreateAsync(CheckCategory entity);
    Task<CheckCategory?> UpdateAsync(CheckCategory entity);
    Task<bool> DeleteAsync(int id);
}
