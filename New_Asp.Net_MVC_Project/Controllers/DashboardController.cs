using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using New_Asp.Net_MVC_Project.AdditionalClasses;
using New_Asp.Net_MVC_Project.Interfaces;
using New_Asp.Net_MVC_Project.Models;
using New_Asp.Net_MVC_Project.ViewModels;

namespace New_Asp.Net_MVC_Project.Controllers;
public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor)
    {
        _dashboardRepository = dashboardRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> ViewProfile()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _dashboardRepository.GetUserById(userId);
        return View(user);
    }

    public async Task<IActionResult> EditProfile()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _dashboardRepository.GetUserById(userId);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(ApplicationUser user)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка при изменении профиля!");
            return View("EditProfile", user);
        }

        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var getUser = await _dashboardRepository.GetByIdNoTracking(userId);

        if (user.UserName == null)
        {
            TempData["Error"] = "Вы должны ввести никнейм!";
            return View(user);
        }

        getUser.UserName = user.UserName;
        getUser.FristName = user.FristName;
        getUser.LastName = user.LastName;
        _dashboardRepository.Update(getUser);
        return RedirectToAction("ViewProfile");
    }

    public async Task<IActionResult> AllUserNotes()
    {
        var data = await _dashboardRepository.GetAllUserNotes();
        var dashboardViewModel = new DashboardViewModel()
        {
            Notes = data
        };
        return View(dashboardViewModel);
    }

}
