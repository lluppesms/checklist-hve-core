namespace CheckList.Api.Data.Models;

public class CheckList
{
    public int Id { get; set; }
    public int SetId { get; set; }
    public int? TemplateListId { get; set; }
    public string ListName { get; set; } = string.Empty;
    public string? ListDscr { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public CheckSet? Set { get; set; }
    public ICollection<CheckCategory> CheckCategories { get; set; } = [];
}
