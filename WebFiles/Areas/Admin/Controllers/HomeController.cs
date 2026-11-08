using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Areas.Admin.ViewModels;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminAccess")]
public class HomeController : Controller
{
    private UserManager<User> _userManager;
    private IStorageService _storageService;

    public HomeController(UserManager<User> userManager, IStorageService storageService)
    {
        _userManager = userManager;
        _storageService = storageService;
    }

    public async Task<IActionResult> Index()
    {
        HomeIndexViewModel model = new HomeIndexViewModel(_storageService.GetCurrentFreeSpace());
        return View(model);
    }
}