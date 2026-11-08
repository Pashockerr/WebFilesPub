using System.Security.Claims;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;
using WebFiles.ViewModels;

namespace WebFiles.Controllers;

public class AccountController : Controller
{
    private readonly IFileService _fileService;
    private readonly INoteService _noteService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private ApplicationDbContext _dbContext;
    private readonly IPageService _pageService;
    private readonly IConfiguration _configuration;
    private int _itemsPerPage;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        ApplicationDbContext dbContext, IUserService userService, IFileService fileService, INoteService noteService,
        IPageService pageService,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _userService = userService;
        _fileService = fileService;
        _noteService = noteService;
        _pageService = pageService;
        _configuration = config;

        _itemsPerPage = _configuration.GetValue<int>("PageItems");
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login()
    {
        string username = Request.Form["username"];
        string password = Request.Form["password"];

        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return Redirect("/error?code=LOGIN_REJECT");
        var res = _userManager.PasswordHasher.VerifyHashedPassword(new User { UserName = username }, user.PasswordHash,
            password);
        if (res == PasswordVerificationResult.Success)
        {
            IEnumerable<Claim> claims;
            claims = await _userManager.GetClaimsAsync(user);
            //await _signInManager.SignInAsync(new Domain.Entities.User { UserName = username }, true);
            await _signInManager.SignInWithClaimsAsync(new User { UserName = username }, true, claims);

            return Redirect("/");
        }

        return Redirect("/error?code=LOGIN_REJECT");
    }

    [HttpPost]
    public async Task<IActionResult> Register()
    {
        string username = Request.Form["username"];
        string password = Request.Form["password"];

        var res = await _userService.AddUserAsync(new User { UserName = username }, password);
        if (res == IdentityResult.Success)
        {
            await _signInManager.SignInAsync(new User { UserName = username }, true);
            return Redirect("/");
        }

        return Redirect("/error?code=REGISTER_REJECT");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }

    [Authorize]
    public async Task<IActionResult> Files(int id, [FromQuery(Name = "query")] string query)
    {
        var userId = (await _userManager.FindByNameAsync(User.Identity?.Name)).Id;
        List<StoredFile> files = _fileService.GetUserFiles(userId);
        if (!query.IsNullOrEmpty())
        {
            query = query.ToLower();
            
            files = (from f in files
                where f.Name.ToLower().Contains(query)
                select f).ToList();

            _itemsPerPage = files.Count;
        }

        var slice = _pageService.GetPageSlice(files, _itemsPerPage, id);
        return View(new AccountFilesViewModel(slice, _pageService.GetPagesCount(files, _itemsPerPage), id, !slice.IsNullOrEmpty()));
    }

    [Authorize]
    public async Task<IActionResult> Notes(int id, [FromQuery(Name = "query")] string query)
    {
        var userId = (await _userManager.FindByNameAsync(User.Identity?.Name)).Id;
        List<Note> notes = _noteService.GetUserNotes(userId);
        if (!query.IsNullOrEmpty())
        {
            query = query.ToLower();
            
            notes = (from n in notes
                where n.Name.ToLower().Contains(query)
                select n).ToList();

            _itemsPerPage = notes.Count;
        }

        var slice = _pageService.GetPageSlice(notes.ToList(), _itemsPerPage, id);
        return View(new AccountNotesViewModel(slice, _pageService.GetPagesCount(notes, _itemsPerPage), id, !slice.IsNullOrEmpty()));
    }

    [Authorize]
    public IActionResult Manage()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Delete()
    {
        await _userService.DeleteUserAsync(await _userManager.FindByNameAsync(User.Identity!.Name));
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}