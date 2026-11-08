using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Domain.Entities;

namespace WebFiles.Controllers;

public class TestController : Controller
{
    private UserManager<User> _userManager;

    public TestController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string Index()
    {
        return "Hello World!";
    }
}