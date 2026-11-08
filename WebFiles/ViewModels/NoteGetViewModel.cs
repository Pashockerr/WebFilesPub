using WebFiles.Domain.Entities;

namespace WebFiles.ViewModels;

public class NoteGetViewModel
{
    public NoteGetViewModel(Note note)
    {
        Note = note;
    }

    public Note Note { get; set; }
}