using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using WebFiles.Data;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Services;

public class FileService : IFileService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IStorageService _storageService;

    public FileService(IConfiguration configuration, ApplicationDbContext dbContext, UserManager<User> userManager, IStorageService storageService)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _userManager = userManager;
        _storageService = storageService;
    }

    private IConfiguration _configuration { get; }

    public async Task<IFileService.Result> DeleteFileAsync(int id, string userId, bool forced = false)
    {
        IQueryable<StoredFile> fileResult;
        if (forced)
            fileResult = from f in _dbContext.Files
                where f.Id == id
                select f;
        else
            fileResult = from f in _dbContext.Files
                where f.Id == id && f.UserId == userId
                select f;

        if (fileResult.Any())
        {
            File.Delete(fileResult.First().Path);
            var userEntity = await _userManager.FindByIdAsync(userId);
            userEntity.AvailableSpace += fileResult.First().Size;

            _dbContext.Remove(fileResult.First());

            await _dbContext.SaveChangesAsync();
            return IFileService.Result.Success;
        }

        return IFileService.Result.NotFound;
    }

    public StoredFile GetFile(int id)
    {
        var fileResult = from f in _dbContext.Files
            where f.Id == id && f.IsPublic
            select f;

        if (fileResult.Any())
            return fileResult.First();
        throw new FileNotFoundException();
    }

    public StoredFile GetFile(int id, string userId)
    {
        var fileResult = from f in _dbContext.Files
            where f.Id == id && f.UserId == userId
            select f;

        if (fileResult.Any())
            return fileResult.First();
        throw new FileNotFoundException();
    }

    public async Task<IFileService.Result> UploadFileAsync(string fileName, string extension, string userId,
        bool isPublic, MultipartSection section)
    {
        try
        {
            var secureName = Guid.NewGuid().ToString();
            var path =
                $"{string.Format(_storageService.GetCurrentStoragePath(), Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))}/{secureName}{extension}";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            byte[] buffer = new byte[1024];
            var bytesRead = 0;
            try
            {
                do
                {
                    bytesRead = await section.Body.ReadAsync(buffer, 0, buffer.Length);
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                } while (bytesRead > 0);
                fileStream.Close();
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
            }

            var fileEntity = new StoredFile(fileName, path, userId, isPublic, section.Body.Length);
            var userEntity = await _userManager.FindByIdAsync(userId);

            if (fileEntity.Size > userEntity.AvailableSpace)
            {
                File.Delete(path);
                return IFileService.Result.Error;
            }

            userEntity.AvailableSpace -= fileEntity.Size;

            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            return IFileService.Result.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return IFileService.Result.Error;
        }
    }

    public List<StoredFile> GetUserFiles(string userId)
    {
        var filesResult = from f in _dbContext.Files
            where f.UserId.Equals(userId)
            select f;

        return filesResult.ToList();
    }

    public async Task DeleteUserFilesAsync(string userId)
    {
        List<StoredFile> userFiles = (from f in _dbContext.Files
            where f.UserId.Equals(userId)
            select f).ToList();

        foreach (var file in userFiles)
        {
            await DeleteFileAsync(file.Id, userId, true);
        }
    }
}