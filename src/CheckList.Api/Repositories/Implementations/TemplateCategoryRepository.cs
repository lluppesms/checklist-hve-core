namespace CheckList.Api.Repositories.Implementations;

using CheckList.Api.Data;
using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TemplateCategoryRepository(AppDbContext db) : ITemplateCategoryRepository
{
    public async Task<IEnumerable<TemplateCategory>> GetByListAsync(int listId)
        => await db.TemplateCategories.Where(c => c.ListId == listId).ToListAsync();

    public async Task<TemplateCategory?> GetByIdAsync(int id)
        => await db.TemplateCategories.FindAsync(id);
}
