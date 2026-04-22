namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ICheckActionRepository
{
    Task<IEnumerable<CheckAction>> GetByCategoryAsync(int categoryId);
    Task<CheckAction?> GetByIdAsync(int id);
    Task<CheckAction> CreateAsync(CheckAction entity);
    Task<CheckAction?> UpdateAsync(CheckAction entity);
    Task<bool> DeleteAsync(int id);
    Task<CheckAction?> ToggleCompleteAsync(int id, string userId);
}
