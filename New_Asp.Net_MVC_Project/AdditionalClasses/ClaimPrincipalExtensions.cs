using System.Security.Claims;

namespace New_Asp.Net_MVC_Project.AdditionalClasses;

public static class ClaimPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
