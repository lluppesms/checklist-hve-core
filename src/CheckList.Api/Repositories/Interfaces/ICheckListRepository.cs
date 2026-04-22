namespace CheckList.Api.Repositories.Interfaces;

using CheckList.Api.Data.Models;

public interface ICheckListRepository
{
    Task<IEnumerable<CheckListModel>> GetBySetAsync(int setId);
    Task<CheckListModel?> GetByIdAsync(int id);
    Task<CheckListModel> CreateAsync(CheckListModel entity);
    Task<CheckListModel?> UpdateAsync(CheckListModel entity);
    Task<bool> DeleteAsync(int id);
}
