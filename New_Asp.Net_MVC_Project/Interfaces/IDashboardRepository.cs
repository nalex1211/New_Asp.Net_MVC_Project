using New_Asp.Net_MVC_Project.Models;

namespace New_Asp.Net_MVC_Project.Interfaces;

public interface IDashboardRepository
{
    public Task<List<NoteData>> GetAllUserNotes();
    public Task<ApplicationUser> GetUserById(string id);
    public Task<ApplicationUser> GetByIdNoTracking(string id);
    public bool Update(ApplicationUser user);
    public bool Save();
}
