using WebFiles.Domain.Entities;
using WebFiles.ViewModelsClasses;

namespace WebFiles.Areas.Admin.ViewModels;

public class FilesIndexViewModel : PagedViewModel
{
    public FilesIndexViewModel(List<StoredFile> files, int pagesTotal, int currentPage, bool notEmpty) : base(pagesTotal, currentPage, notEmpty)
    {
        Files = files;
    }

    public List<StoredFile> Files { get; set; } = new();
}