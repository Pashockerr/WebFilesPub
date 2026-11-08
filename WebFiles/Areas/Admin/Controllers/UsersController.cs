using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Areas.Admin.ViewModels;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize("AdminAccess")]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IPageService _pageService;
    private readonly IConfiguration _configuration;
    private int _itemsPerPage;

    public UsersController(IUserService userService, ApplicationDbContext dbContext, IPageService pageService, IConfiguration config)
    {
        _userService = userService;
        _dbContext = dbContext;
        _pageService = pageService;
        _configuration = config;

        _itemsPerPage = _configuration.GetValue<int>("PageItems");
    }

    public IActionResult Index(int id, [FromQuery(Name = "query")] string query)
    {
        List<Domain.Entities.User> users = _dbContext.Users.ToList();
        if (!query.IsNullOrEmpty())
        {
            query = query.ToLower();
            
            users = (from u in users
                where u.UserName.ToLower().Contains(query)
                select u).ToList();

            _itemsPerPage = users.Count;
        }
        var slice = _pageService.GetPageSlice(users, _itemsPerPage, id);
        return View(new UsersIndexViewModel(slice, _pageService.GetPagesCount(users, _itemsPerPage), id, !slice.IsNullOrEmpty()));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUserAsync(new User { Id = id });
        return Redirect("/admin/users");
    }

    [HttpPost]
    public async Task<IActionResult> ChangeExtended(string id)
    {
        await _userService.ChangeExtendedAsync(id);

        return Redirect("/admin/users");
    }
}