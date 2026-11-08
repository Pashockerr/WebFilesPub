using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;
using WebFiles.ViewModels;

namespace WebFiles.Controllers;

public class NoteController : Controller
{
    private readonly INoteService _noteService;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public NoteController(INoteService noteService, UserManager<User> userManager, ApplicationDbContext context)
    {
        _noteService = noteService;
        _userManager = userManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add()
    {
        var name = Request.Form["name"];
        var title = Request.Form["title"];
        var text = Request.Form["text"];
        var isPublic = Request.Form["isPublic"].Equals("on");
        var userId = (await _userManager.FindByNameAsync(User.Identity!.Name)).Id;
        await _noteService.AddNoteAsync(name, title, text, isPublic, userId);

        return Redirect("/account/notes");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = (await _userManager.FindByNameAsync(User.Identity!.Name)).Id;
        await _noteService.DeleteNoteAsync(id, userId);
        return Redirect("/account/notes");
    }

    public async Task<IActionResult> Get(int id)
    {
        if (User.Identity!.IsAuthenticated)
        {
            var userId = (await _userManager.FindByNameAsync(User.Identity!.Name)).Id;
            return View(new NoteGetViewModel(_noteService.GetNote(id, userId)));
        }

        return View(new NoteGetViewModel(_noteService.GetNote(id)));
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangePublic(int id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);

        var noteRes = from n in _context.Notes
            where n.Id == id && n.UserId.Equals(user.Id)
            select n;

        if(noteRes.Count() > 0)
        {
            noteRes.First().SetAvailability(!noteRes.First().IsPublic);
            await _context.SaveChangesAsync();
        }

        return Redirect("/account/notes");
    }

    [Authorize]
    public async Task<IActionResult> EditIndex([FromQuery(Name = "noteid")] int noteId)
    {
        var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
        var notesResult = from n in _noteService.GetUserNotes(userId)
            where n.Id == noteId
            select n;

        if (!notesResult.Any())
            return Redirect("/error?code=403");

        var note = notesResult.FirstOrDefault();
        
        return View(new NoteEditIndexViewModel(note.Id, note.Name, note.Title, note.Text, note.IsPublic));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit()
    {
        var name = Request.Form["name"];
        var title = Request.Form["title"];
        var text = Request.Form["text"];
        var isPublic = Request.Form["isPublic"].Equals("on");
        var userId = (await _userManager.FindByNameAsync(User.Identity!.Name)).Id;
        int noteId = int.Parse(Request.Form["id"]);
        await _noteService.UpdateNoteAsync(noteId, name, title, text, isPublic, userId);

        return Redirect("/account/notes");
    }
}