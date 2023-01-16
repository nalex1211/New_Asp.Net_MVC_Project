using Microsoft.AspNetCore.Mvc;
using New_Asp.Net_MVC_Project.AdditionalClasses;
using New_Asp.Net_MVC_Project.Interfaces;
using New_Asp.Net_MVC_Project.Models;
using New_Asp.Net_MVC_Project.ViewModels;



namespace New_Asp.Net_MVC_Project.Controllers;
public class NoteController : Controller
{
    private readonly INoteDataRepository _noteDataRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NoteController(INoteDataRepository noteDataRepository, IHttpContextAccessor httpContextAccessor)
    {
        _noteDataRepository = noteDataRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> AllNotes()
    {
        var notes = await _noteDataRepository.GetAllNotes();
        var registers = await _noteDataRepository.GetAllUsers();
        var userAndNoteViewModel = new UserInfoAndNoteViewModel()
        {
            Notes = notes,
            Registers = registers
        };
        return View(userAndNoteViewModel);
    }
    public async Task<IActionResult> ViewNote(int id)
    {
        var note = await _noteDataRepository.GetNoteByIdAsync(id);
        return View(note);
    }
    public IActionResult AddNote()
    {
        if (!User.Identity.IsAuthenticated)
        {
            TempData["Error"] = "Вы должны войти в свой аккаунт!";
            return RedirectToAction("Index", "Home");
        }
        var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var noteData = new NoteData
        {
            ApplicationUserId = currentUserId
        };
        return View(noteData);
    }

    [HttpPost]
    public async Task<IActionResult> AddNote(NoteData note)
    {
        if (!ModelState.IsValid)
        {
            return View(note);
        }

        _noteDataRepository.Add(note);
        return RedirectToAction("AllNotes");
    }

    public async Task<IActionResult> EditNote(int id)
    {
        var note = await _noteDataRepository.GetNoteByIdAsync(id);
        var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var noteData = new NoteData
        {
            ApplicationUserId = currentUserId
        };
        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> EditNote(NoteData note)
    {
        if (!ModelState.IsValid)
        {
            return View(note);
        }
        _noteDataRepository.Update(note);
        return RedirectToAction("AllNotes");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var choseNote = await _noteDataRepository.GetNoteByIdAsync(id);
        _noteDataRepository.Delete(choseNote);
        return RedirectToAction("AllNotes");
    }
}
