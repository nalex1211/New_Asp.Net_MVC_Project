using Microsoft.AspNetCore.Identity;

namespace New_Asp.Net_MVC_Project.Models;

public class ApplicationUser : IdentityUser
{
    public string? FristName { get; set; }
    public string? LastName { get; set; }
}
