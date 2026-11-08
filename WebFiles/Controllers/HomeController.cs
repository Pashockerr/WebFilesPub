using Microsoft.AspNetCore.Mvc;
using WebFiles.ViewModels;

namespace WebFiles.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        HomeIndexViewModel model;

        if (User.Identity!.IsAuthenticated)
        {
            if (User.Claims.Any(item => item.Type.Equals("Administrator")))
                model = new HomeIndexViewModel(true);
            else
                model = new HomeIndexViewModel(false);
        }
        else
        {
            model = new HomeIndexViewModel(false);
        }

        return View(model);
    }
}