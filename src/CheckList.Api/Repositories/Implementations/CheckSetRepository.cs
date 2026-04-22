namespace CheckList.Api.Repositories.Implementations;

using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CheckSetRepository(AppDbContext db) : ICheckSetRepository
{
    public async Task<IEnumerable<CheckSet>> GetByOwnerAsync(string ownerName)
        => await db.CheckSets.Where(s => s.OwnerName == ownerName).ToListAsync();

    public async Task<CheckSet?> GetByIdAsync(int id)
        => await db.CheckSets.Include(s => s.CheckLists).FirstOrDefaultAsync(s => s.Id == id);

    public async Task<CheckSet> CreateAsync(CheckSet entity)
    {
        db.CheckSets.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<CheckSet?> UpdateAsync(CheckSet entity)
    {
        var existing = await db.CheckSets.FindAsync(entity.Id);
        if (existing is null) return null;
        db.Entry(existing).CurrentValues.SetValues(entity);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await db.CheckSets.FindAsync(id);
        if (entity is null) return false;
        db.CheckSets.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
