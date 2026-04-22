namespace CheckList.Api.Repositories.Implementations;

using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TemplateActionRepository(AppDbContext db) : ITemplateActionRepository
{
    public async Task<IEnumerable<TemplateAction>> GetByCategoryAsync(int categoryId)
        => await db.TemplateActions.Where(a => a.CategoryId == categoryId).ToListAsync();

    public async Task<TemplateAction?> GetByIdAsync(int id)
        => await db.TemplateActions.FindAsync(id);
}
