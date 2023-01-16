using System.ComponentModel.DataAnnotations;

namespace New_Asp.Net_MVC_Project.Models;

public class Register
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Вы должны ввести почту!"), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Вы долнжны ввести свой никнейм!")]
    public string Username { get; set; }

    [Required(ErrorMessage ="Вы должны ввести пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name ="Confirm password")]
    [Required(ErrorMessage = "Необходимо подтверджение пароля")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage ="Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}
