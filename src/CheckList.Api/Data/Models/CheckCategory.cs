namespace CheckList.Api.Data.Models;

public class CheckCategory
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public int? TemplateCategoryId { get; set; }
    public string CategoryText { get; set; } = string.Empty;
    public string? CategoryDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public CheckList? List { get; set; }
    public ICollection<CheckAction> CheckActions { get; set; } = [];
}
