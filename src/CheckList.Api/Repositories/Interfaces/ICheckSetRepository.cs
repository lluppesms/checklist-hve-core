namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ICheckSetRepository
{
    Task<IEnumerable<CheckSet>> GetByOwnerAsync(string ownerName);
    Task<CheckSet?> GetByIdAsync(int id);
    Task<CheckSet> CreateAsync(CheckSet entity);
    Task<CheckSet?> UpdateAsync(CheckSet entity);
    Task<bool> DeleteAsync(int id);
}
