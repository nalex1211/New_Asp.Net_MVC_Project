using System.ComponentModel.DataAnnotations;

namespace New_Asp.Net_MVC_Project.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage ="Вы должны ввести пароль!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "Необходимо подтверджение пароля")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
