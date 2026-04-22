namespace CheckList.Api.Repositories.Implementations;

using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
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
