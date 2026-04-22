namespace CheckList.Web.Data.Models;

public class TemplateList
{
    public int Id { get; set; }
    public int SetId { get; set; }
    public string ListName { get; set; } = string.Empty;
    public string? ListDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateSet? Set { get; set; }
    public ICollection<TemplateCategory> TemplateCategories { get; set; } = [];
}
