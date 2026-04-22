namespace CheckList.Web.Data.Models;

public class TemplateCategory
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string CategoryText { get; set; } = string.Empty;
    public string? CategoryDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateList? List { get; set; }
    public ICollection<TemplateAction> TemplateActions { get; set; } = [];
}
