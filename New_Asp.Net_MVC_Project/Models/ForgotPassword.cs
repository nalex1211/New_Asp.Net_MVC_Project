using System.ComponentModel.DataAnnotations;

namespace New_Asp.Net_MVC_Project.Models;

public class ForgotPassword
{
    [Required(ErrorMessage = "Вы должны указать почту!"), EmailAddress]
    public string Email { get; set; }
}
