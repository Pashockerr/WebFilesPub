using WebFiles.Domain.Entities;
using WebFiles.ViewModelsClasses;

namespace WebFiles.Areas.Admin.ViewModels;

public class NotesIndexViewModel : PagedViewModel
{
    public NotesIndexViewModel(List<Note> notes, int totalPages, int currentPage, bool notEmpty) : base(totalPages, currentPage, notEmpty)
    {
        Notes = notes;
    }

    public List<Note> Notes { get; set; }
}