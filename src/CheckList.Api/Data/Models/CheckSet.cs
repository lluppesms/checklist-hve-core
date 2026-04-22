namespace CheckList.Api.Data.Models;

public class CheckSet
{
    public int Id { get; set; }
    public int? TemplateSetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string? SetDscr { get; set; }
    public string? OwnerName { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public TemplateSet? TemplateSet { get; set; }
    public ICollection<CheckListModel> CheckLists { get; set; } = [];
}
