using WebFiles.Domain.Entities;
using WebFiles.ViewModelsClasses;

namespace WebFiles.Areas.Admin.ViewModels;

public class UsersIndexViewModel : PagedViewModel
{
    public UsersIndexViewModel(List<User> users, int totalPages, int currentPage, bool notEmpty) : base(totalPages, currentPage, notEmpty)
    {
        Users = users;
    }

    public List<User> Users { get; set; } = new();
}