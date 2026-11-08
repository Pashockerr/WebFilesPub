using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using WebFiles.Attributes;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;
using WebFiles.Utilities;
using WebFiles.ViewModels;

namespace WebFiles.Controllers;

[RequestSizeLimit(5_368_709_120)] //5GB
[RequestFormLimits(MultipartBodyLengthLimit = 5_368_709_120)]
public class FileController : Controller
{
    private readonly IFileService _fileService;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public FileController(IFileService fileService, UserManager<User> userManager, ApplicationDbContext context)
    {
        _fileService = fileService;
        _userManager = userManager;
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);

        FileIndexViewModel model = new FileIndexViewModel("5GB", (int)(user.AvailableSpace / 1_048_576));

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> Upload()
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        {
            return BadRequest("Request isn't send files.");
        }

        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 234252345);
        var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        /*
         * First section is fileName
         * Second section is isPublic section
         * Third section is fileSection
         */

        var fileNameSection = await reader.ReadNextSectionAsync();
        string fileName = await fileNameSection.ReadAsStringAsync();
        var extension = Path.GetExtension(fileName);
        var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
        var user = await _userManager.FindByIdAsync(userId);
        bool isPublic;
        var isPublicSection = await reader.ReadNextSectionAsync();
        if ((await isPublicSection.ReadAsStringAsync()).Equals("false"))
            isPublic = false;
        else
            isPublic = true;

        var fileSection = await reader.ReadNextSectionAsync();
        
        var result = await _fileService.UploadFileAsync(fileName, extension, userId, isPublic, fileSection);
        if (result == IFileService.Result.Success)
            return Redirect("/account/files");
        return Redirect("/error?code=500");
    }

    public async Task<IActionResult> Get(string id)
    {
        int fileExtPos = id.LastIndexOf(".");
        if (fileExtPos >= 0)
            id = id.Substring(0, fileExtPos); //Removing extension

        int fileId = int.Parse(Regex.Replace(id, "[^0-9]", ""));

        var provider = new FileExtensionContentTypeProvider();
        string type;
        StoredFile res;

        try
        {
            if (User.Identity.IsAuthenticated)
                res = _fileService.GetFile(fileId, (await _userManager.FindByNameAsync(User.Identity.Name)).Id);
            else
                res = _fileService.GetFile(fileId);
        }
        catch (Exception)
        {
            return Redirect("/error?code=404");
        }

        var extension = Path.GetExtension(res.Path);
        if (!provider.TryGetContentType(extension, out type)) type = "application/octet-stream";
        if (Request.Query["download"].IsNullOrEmpty())
            return PhysicalFile(res.Path, type, enableRangeProcessing: true);
        else
            return PhysicalFile(res.Path, type, fileDownloadName: $"{res.Name}", enableRangeProcessing: true);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _fileService.DeleteFileAsync(id, (await _userManager.FindByNameAsync(User.Identity.Name)).Id);
        if (res == IFileService.Result.Success)
            return Redirect("/account/files");
        return Redirect("/error?code=404");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePublic(int id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);

        var fileRes = from f in _context.Files
            where f.Id == id && f.UserId.Equals(user.Id)
            select f;

        if (fileRes.Any())
        {
            fileRes.First().SetAvailability(!fileRes.First().IsPublic);
            await _context.SaveChangesAsync();
        }

        return Redirect("/account/files");
    }

    public IActionResult WatchVideo(string id)
    {
        int fileExtPos = id.LastIndexOf(".");

        return View(new FileWatchVideoViewModel($"/file/get/{id}"));
    }
}