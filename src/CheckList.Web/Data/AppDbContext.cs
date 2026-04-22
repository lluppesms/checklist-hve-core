namespace CheckList.Web.Data;

using CheckList.Web.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<TemplateSet> TemplateSets => Set<TemplateSet>();
    public DbSet<TemplateList> TemplateLists => Set<TemplateList>();
    public DbSet<TemplateCategory> TemplateCategories => Set<TemplateCategory>();
    public DbSet<TemplateAction> TemplateActions => Set<TemplateAction>();
    public DbSet<CheckSet> CheckSets => Set<CheckSet>();
    public DbSet<CheckListModel> CheckLists => Set<CheckListModel>();
    public DbSet<CheckCategory> CheckCategories => Set<CheckCategory>();
    public DbSet<CheckAction> CheckActions => Set<CheckAction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CheckAction>()
            .Property(a => a.CompleteInd)
            .HasColumnType("nchar(1)")
            .HasDefaultValue(" ");
    }
}
