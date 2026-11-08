using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFiles.Areas.Admin.ViewModels;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize("AdminAccess")]
public class FilesController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IFileService _fileService;
    private readonly IPageService _pageService;
    private readonly IConfiguration _configuration;
    private int _itemsPerPage;
    private readonly UserManager<User> _userManager;

    public FilesController(IFileService fileService, ApplicationDbContext dbContext, IPageService pageService,
        IConfiguration config, UserManager<User> userManager)
    {
        _fileService = fileService;
        _dbContext = dbContext;
        _pageService = pageService;
        _configuration = config;
        _userManager = userManager;

        _itemsPerPage = config.GetValue<int>("PageItems");
    }

    [HttpGet]
    public IActionResult Index(int id, [FromQuery(Name = "query")] string query)
    {
        List<StoredFile> files = _dbContext.Files.ToList();
        if (!query.IsNullOrEmpty())
        {
            query = query.ToLower();
            
            files = (from f in files
                where f.Name.ToLower().Contains(query)
                select f).ToList();
            
            _itemsPerPage = files.Count;
        }

        var slice = _pageService.GetPageSlice(files, _itemsPerPage, id);
        return View(new FilesIndexViewModel(slice, _pageService.GetPagesCount(files, _itemsPerPage), id,
            !slice.IsNullOrEmpty()));
    }

    public async Task<IActionResult> DeleteFile(int id)
    {
        var fileEntity = from f in _dbContext.Files
            where f.Id == id
            select f;
        await _fileService.DeleteFileAsync(id, fileEntity.First().UserId, true);
        return Redirect("/admin/files");
    }
}