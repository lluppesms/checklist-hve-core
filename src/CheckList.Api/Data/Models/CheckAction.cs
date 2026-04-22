namespace CheckList.Api.Data.Models;

public class CheckAction
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int ListId { get; set; }
    public int SetId { get; set; }   // denormalized for SignalR group broadcast
    public string ActionText { get; set; } = string.Empty;
    public string? ActionDscr { get; set; }
    public string CompleteInd { get; set; } = " ";   // "Y" or " "
    public string? CompletedBy { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public CheckCategory? Category { get; set; }
}
