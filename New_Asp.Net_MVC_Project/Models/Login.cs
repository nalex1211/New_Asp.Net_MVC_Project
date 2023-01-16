using System.ComponentModel.DataAnnotations;

namespace New_Asp.Net_MVC_Project.Models;

public class Login
{
    [Required(ErrorMessage ="Вы должны указать почту!"), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage ="Вы должны ввести пароль!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
