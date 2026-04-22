namespace CheckList.Web.Repositories.Implementations;

using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CheckListRepository(AppDbContext db) : ICheckListRepository
{
    public async Task<IEnumerable<CheckListModel>> GetBySetAsync(int setId)
        => await db.CheckLists.Where(l => l.SetId == setId).ToListAsync();

    public async Task<CheckListModel?> GetByIdAsync(int id)
        => await db.CheckLists.Include(l => l.CheckCategories).FirstOrDefaultAsync(l => l.Id == id);

    public async Task<CheckListModel> CreateAsync(CheckListModel entity)
    {
        db.CheckLists.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<CheckListModel?> UpdateAsync(CheckListModel entity)
    {
        var existing = await db.CheckLists.FindAsync(entity.Id);
        if (existing is null) return null;
        db.Entry(existing).CurrentValues.SetValues(entity);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await db.CheckLists.FindAsync(id);
        if (entity is null) return false;
        db.CheckLists.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
