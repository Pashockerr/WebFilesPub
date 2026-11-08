using WebFiles.Domain.Entities;
using WebFiles.ViewModelsClasses;

namespace WebFiles.ViewModels;

public class AccountNotesViewModel : PagedViewModel
{
    public AccountNotesViewModel(List<Note> notes, int pagesTotal, int currentPage, bool notEmpty) : base(pagesTotal, currentPage, notEmpty)
    {
        Notes = notes;
    }

    public List<Note> Notes { get; set; }
}