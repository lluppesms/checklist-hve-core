namespace CheckList.Web.Repositories.Implementations;

using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TemplateActionRepository(AppDbContext db) : ITemplateActionRepository
{
    public async Task<IEnumerable<TemplateAction>> GetByCategoryAsync(int categoryId)
        => await db.TemplateActions.Where(a => a.CategoryId == categoryId).ToListAsync();

    public async Task<TemplateAction?> GetByIdAsync(int id)
        => await db.TemplateActions.FindAsync(id);
}
