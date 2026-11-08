namespace WebFiles.Services.Abstract;

public interface IStorageService
{
    public string GetCurrentStoragePath();
    public long GetCurrentFreeSpace();
}