namespace CheckList.Web.Repositories.Implementations;

using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TemplateSetRepository(AppDbContext db) : ITemplateSetRepository
{
    public async Task<IEnumerable<TemplateSet>> GetAllAsync()
        => await db.TemplateSets.ToListAsync();

    public async Task<TemplateSet?> GetByIdWithChildrenAsync(int id)
        => await db.TemplateSets
            .Include(s => s.TemplateLists)
                .ThenInclude(l => l.TemplateCategories)
                    .ThenInclude(c => c.TemplateActions)
            .FirstOrDefaultAsync(s => s.Id == id);
}
