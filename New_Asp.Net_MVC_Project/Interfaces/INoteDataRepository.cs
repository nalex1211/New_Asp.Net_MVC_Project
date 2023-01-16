using New_Asp.Net_MVC_Project.Models;

namespace New_Asp.Net_MVC_Project.Interfaces;

public interface INoteDataRepository
{
    Task<IEnumerable<NoteData>> GetAllNotes();
    Task<List<ApplicationUser>> GetAllUsers();
    Task<NoteData> GetNoteByIdAsync(int id);
    bool Add(NoteData note);
    bool Update(NoteData note);
    bool Delete(NoteData note);
    bool Save();
}
