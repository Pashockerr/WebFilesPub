using Microsoft.AspNetCore.WebUtilities;
using WebFiles.Domain.Entities;

namespace WebFiles.Services.Abstract;

public interface IFileService
{
    public enum Result
    {
        Success,
        NotAllowed,
        Error,
        NotFound
    }

    public Task<Result> UploadFileAsync(string fileName, string extension, string userId, bool isPublic,
         MultipartSection section);

    public StoredFile GetFile(int id);
    public StoredFile GetFile(int id, string userId);
    public Task<Result> DeleteFileAsync(int id, string userId, bool forced = false);
    public List<StoredFile> GetUserFiles(string userId);
    public Task DeleteUserFilesAsync(string userId);
}