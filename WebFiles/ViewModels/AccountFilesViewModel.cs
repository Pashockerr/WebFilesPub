using WebFiles.Domain.Entities;
using WebFiles.ViewModelsClasses;

namespace WebFiles.ViewModels;

public class AccountFilesViewModel : PagedViewModel
{
    public AccountFilesViewModel(List<StoredFile> files, int pagesTotal, int currentPage, bool notEmpty) : base(pagesTotal, currentPage, notEmpty)
    {
        Files = files;
    }

    public List<StoredFile> Files { get; set; }
}