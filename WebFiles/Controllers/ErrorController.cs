using Microsoft.AspNetCore.Mvc;
using WebFiles.ViewModels;

namespace WebFiles.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(Request.Query["code"]))
            return View(new ErrorIndexViewModel("Error", "Yep, just an error"));

        var model = new ErrorIndexViewModel("", "");
        switch (Request.Query["code"])
        {
            case "LOGIN_REJECT":
                model.Title = "Login rejected";
                model.Subtitle = "Incorrect login or/and password";
                break;
            case "REGISTER_REJECT":
                model.Title = "Registration rejected";
                model.Subtitle = "User with this username already exists";
                break;
            case "FILE_TOO_BIG":
                model.Title = "File too big";
                model.Subtitle = "File too big";
                break;
            default:
                model.Title = $"Error {Request.Query["code"]}";
                model.Subtitle = "There's no description";
                break;
        }

        return View(model);
    }
}