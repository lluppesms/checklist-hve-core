namespace CheckList.Web.Data.Models;

public class TemplateAction
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string ActionText { get; set; } = string.Empty;
    public string? ActionDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateCategory? Category { get; set; }
}
