namespace CheckList.Api.Repositories.Implementations;

using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CheckActionRepository(AppDbContext db) : ICheckActionRepository
{
    public async Task<IEnumerable<CheckAction>> GetByCategoryAsync(int categoryId)
        => await db.CheckActions.Where(a => a.CategoryId == categoryId).ToListAsync();

    public async Task<CheckAction?> GetByIdAsync(int id)
        => await db.CheckActions.FindAsync(id);

    public async Task<CheckAction> CreateAsync(CheckAction entity)
    {
        db.CheckActions.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<CheckAction?> UpdateAsync(CheckAction entity)
    {
        var existing = await db.CheckActions.FindAsync(entity.Id);
        if (existing is null) return null;
        db.Entry(existing).CurrentValues.SetValues(entity);
        await db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await db.CheckActions.FindAsync(id);
        if (entity is null) return false;
        db.CheckActions.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<CheckAction?> ToggleCompleteAsync(int id, string userId)
    {
        var action = await db.CheckActions.FindAsync(id);
        if (action is null) return null;

        action.CompleteInd = action.CompleteInd.Trim() == "Y" ? " " : "Y";
        action.CompletedBy = action.CompleteInd.Trim() == "Y" ? userId : null;
        action.CompletedAt = action.CompleteInd.Trim() == "Y" ? DateTime.UtcNow : null;
        await db.SaveChangesAsync();
        return action;
    }
}
