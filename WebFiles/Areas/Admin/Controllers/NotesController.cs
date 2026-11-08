using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Areas.Admin.ViewModels;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminAccess")]
public class NotesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INoteService _noteService;
    private readonly IPageService _pageService;
    private readonly IConfiguration _configuration;
    private int _itemsPerPage;

    public NotesController(ApplicationDbContext context, INoteService noteService, IPageService pageService, IConfiguration config)
    {
        _context = context;
        _noteService = noteService;
        _pageService = pageService;
        _configuration = config;

        _itemsPerPage = config.GetValue<int>("PageItems");
    }

    public IActionResult Index(int id, [FromQuery(Name = "query")] string query)
    {
        List<Note> notes = _context.Notes.ToList();
        if (!query.IsNullOrEmpty())
        {
            query = query.ToLower();
            
            notes = (from n in notes
                where n.Name.ToLower().Contains(query)
                select n).ToList();

            _itemsPerPage = notes.Count;
        }
        var slice = _pageService.GetPageSlice(notes, _itemsPerPage, id);
        return View(new NotesIndexViewModel(slice, _pageService.GetPagesCount(notes, _itemsPerPage), id, !slice.IsNullOrEmpty()));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _noteService.DeleteNoteAsync(id, "Admin", true);
        return Redirect("/admin/notes");
    }
}