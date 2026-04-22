namespace CheckList.Web.Data.Models;

public class Customer
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public AppUser? User { get; set; }
}
