namespace CheckList.Api.Data.Models;

public class TemplateSet
{
    public int Id { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string? SetDscr { get; set; }
    public string? OwnerName { get; set; }
    public string ActiveInd { get; set; } = "Y";
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public ICollection<TemplateList> TemplateLists { get; set; } = [];
}
