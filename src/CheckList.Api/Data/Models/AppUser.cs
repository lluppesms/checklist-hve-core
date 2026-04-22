namespace CheckList.Api.Data.Models;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Customer? Customer { get; set; }
}
