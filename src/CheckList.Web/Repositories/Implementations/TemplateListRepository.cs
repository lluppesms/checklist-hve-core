namespace CheckList.Web.Repositories.Implementations;

using CheckList.Web.Data;
using CheckList.Web.Data.Models;
using CheckList.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TemplateListRepository(AppDbContext db) : ITemplateListRepository
{
    public async Task<IEnumerable<TemplateList>> GetBySetAsync(int setId)
        => await db.TemplateLists.Where(l => l.SetId == setId).ToListAsync();

    public async Task<TemplateList?> GetByIdAsync(int id)
        => await db.TemplateLists.FindAsync(id);
}
