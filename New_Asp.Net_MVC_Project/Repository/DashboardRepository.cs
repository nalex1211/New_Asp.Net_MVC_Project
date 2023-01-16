using Microsoft.EntityFrameworkCore;
using New_Asp.Net_MVC_Project.AdditionalClasses;
using New_Asp.Net_MVC_Project.Data;
using New_Asp.Net_MVC_Project.Interfaces;
using New_Asp.Net_MVC_Project.Models;

namespace New_Asp.Net_MVC_Project.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<List<NoteData>> GetAllUserNotes()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userNotes = _db.Notes.Where(x => x.ApplicationUser.Id == currentUser);
        return userNotes.ToList();
    }
    public async Task<ApplicationUser> GetUserById(string id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<ApplicationUser> GetByIdNoTracking(string id)
    {
        return await _db.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }

    public bool Update(ApplicationUser user)
    {
       _db.Users.Update(user);
        return Save();
    }

    public bool Save()
    {
        var saved = _db.SaveChanges();
        return saved > 0 ? true : false;
    }
}
