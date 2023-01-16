using Microsoft.EntityFrameworkCore;
using New_Asp.Net_MVC_Project.Data;
using New_Asp.Net_MVC_Project.Interfaces;
using New_Asp.Net_MVC_Project.Models;

namespace New_Asp.Net_MVC_Project.Repository;

public class NoteDataRepository : INoteDataRepository
{
    private readonly ApplicationDbContext _db;

    public NoteDataRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public bool Add(NoteData note)
    {
        _db.Add(note);
        return Save();
    }

    public bool Delete(NoteData note)
    {
        _db.Remove(note);
        return Save();
    }

    public async Task<IEnumerable<NoteData>> GetAllNotes()
    {
        return await _db.Notes.ToListAsync();
    }

    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<NoteData> GetNoteByIdAsync(int id)
    {
        return await _db.Notes.FirstOrDefaultAsync(x => x.Id == id);
    }

    public bool Save()
    {
        var saved = _db.SaveChanges();
        return saved > 0 ? true : false;
    }

    public bool Update(NoteData note)
    {
        _db.Update(note);
        return Save();
    }
}
