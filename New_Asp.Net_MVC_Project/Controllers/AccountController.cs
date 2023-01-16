using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using New_Asp.Net_MVC_Project.Data;
using New_Asp.Net_MVC_Project.Models;
using New_Asp.Net_MVC_Project.ViewModels;
using static New_Asp.Net_MVC_Project.AdditionalClasses.Constants;

namespace New_Asp.Net_MVC_Project.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly EmailService _emailService;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext db, RoleManager<IdentityRole> roleManager, EmailService emailSerice)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
        _roleManager = roleManager;
        _emailService = emailSerice;
    }

    public IActionResult Login()
    {
        var response = new Login();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Error"] = "Почты не существует!";
            return View(model);
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordCheck)
        {
            TempData["Error"] = "Неверный пароль!";
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("Password", "Вы должны подтвердить ваш аккаунт!");
        return View(model);
    }

    

    public IActionResult Register()
    {
        var response = new Register();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            TempData["Error"] = "Эта почта уже существует!";
            return View(model);
        }

        var userName = await _userManager.FindByNameAsync(model.Username);
        if (userName != null)
        {
            TempData["Error"] = "Этот никнейм уже существует!";
            return View(model);
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        if (model.Email == "nalex1211@gmail.com")
        {
            var adminUser = new ApplicationUser()
            {
                FristName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username
            };
            var newAdminResponse = await _userManager.CreateAsync(adminUser, model.Password);
            if (newAdminResponse.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                await _userManager.ConfirmEmailAsync(adminUser, token);
            }
            return RedirectToAction("Index", "Home");
        }

        var newUser = new ApplicationUser()
        {
            FristName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Username
        };

        var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);
        if (newUserResponse.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var callbakcUrl = Url.Action("ConfirmEmail", "Account",
                new { userId = newUser.Id, token = token },
                protocol: Request.Scheme);

            await _emailService.SendEmailAsync(newUser.Email, callbakcUrl, HostInfo.emailConfirmationSubject);
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return View("ConfirmationMessage");
        }

        if (!newUserResponse.Succeeded)
        {
            var descriptions = string.Empty;
            foreach (var item in newUserResponse.Errors)
            {
                descriptions += string.Concat(item.Description, "\n");
            }
            ModelState.AddModelError("Password", descriptions);
            return View(model);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPassword model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Error"] = "Такой почты не существует!";
            return View(model);
        }
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            TempData["Error"] = "Эта почта не подтверждена! Пожалуйста подтвердите ее.";
            return View(model);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Account",
            new { userId = user.Id, token = token, email = user.Email },
                protocol: Request.Scheme);

        await _emailService.SendEmailAsync(model.Email, callbackUrl, HostInfo.passowrdResetSubject);
        return View("PasswordConfirmationMessage");
    }
    [HttpGet]
    public IActionResult ResetPassword(string userId, string token, string email)
    {
        if (userId == null || token == null)
        {
            return View("Error");
        }
        var model = new ResetPasswordViewModel
        {
            Email = email,
            Token = token
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return View(model);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (!result.Succeeded)
        {
            var descriptions = string.Empty;
            foreach (var item in result.Errors)
            {
                descriptions += string.Concat(item.Description, "\n");
            }
            ModelState.AddModelError("Password", descriptions);
            return View(model);
        }
        return View("ResettedPAssword");
    }

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return View("Error");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return View("Error");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return View("ConfirmedEmail");
        }
        else
        {
            return View("Error");
        }
    }
}
