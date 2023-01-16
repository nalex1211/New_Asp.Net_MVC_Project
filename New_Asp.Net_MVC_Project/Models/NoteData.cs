using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_Asp.Net_MVC_Project.Models;

public class NoteData
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Вы должны указать название заметки!")]
    public string Title { get; set; }
    public string? NoteContent { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    [ForeignKey("ApplicationUser")]
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
