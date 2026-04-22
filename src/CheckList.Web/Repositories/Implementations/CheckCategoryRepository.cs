namespace CheckList.Web.Repositories.Implementations;

using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CheckCategoryRepository(AppDbContext db) : ICheckCategoryRepository
{
    public async Task<IEnumerable<CheckCategory>> GetByListAsync(int listId)
        => await db.CheckCategories.Where(c => c.ListId == listId).ToListAsync();

    public async Task<CheckCategory?> GetByIdAsync(int id)
        => await db.CheckCategories.Include(c => c.CheckActions).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<CheckCategory> CreateAsync(CheckCategory entity)
    {
        db.CheckCategories.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<CheckCategory?> UpdateAsync(CheckCategory entity)
    {
        var existing = await db.CheckCategories.FindAsync(entity.Id);
        if (existing is null) return null;
        db.Entry(existing).CurrentValues.SetValues(entity);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await db.CheckCategories.FindAsync(id);
        if (entity is null) return false;
        db.CheckCategories.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
