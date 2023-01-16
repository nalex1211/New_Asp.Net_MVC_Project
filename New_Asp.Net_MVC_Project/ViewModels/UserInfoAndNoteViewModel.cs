using New_Asp.Net_MVC_Project.Models;

namespace New_Asp.Net_MVC_Project.ViewModels;

public class UserInfoAndNoteViewModel
{
    public IEnumerable<NoteData> Notes { get; set; }
    public List<ApplicationUser> Registers { get; set; }
}
