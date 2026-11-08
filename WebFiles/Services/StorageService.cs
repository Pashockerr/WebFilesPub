using Castle.Core.Internal;
using WebFiles.Services.Abstract;

namespace WebFiles.Services;

public class StorageService : IStorageService
{
    private IConfiguration _configuration;
    private readonly List<string>? _availablePaths;
    private string _currentPath;
    private readonly long _spaceLimit;
    
    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _availablePaths = new List<string>();
        
        _availablePaths = _configuration.GetSection("AlternativeStorePaths").Get<string[]>()?.ToList();

        if (_availablePaths == null)
            _availablePaths = new List<string>();
        
        _spaceLimit = _configuration.GetValue<long>("StorageFreeSizeLimit");

        var defaultPath = _configuration.GetValue<string>("StorePath");

        _availablePaths.Insert(0, defaultPath);
        _currentPath = defaultPath;
    } 
    
    public string GetCurrentStoragePath()
    {
        var drives = DriveInfo.GetDrives();

        foreach (var path in _availablePaths)
        {
            if (path.IsNullOrEmpty())
                continue;
            var fileInfo = new FileInfo(path);
            var diskName = Path.GetPathRoot(fileInfo.FullName);
        
            foreach (var drive in drives)
            {
                if (drive.Name.Equals(diskName))
                {
                    if (drive.AvailableFreeSpace >= _spaceLimit)
                    {
                        _currentPath = path;
                    }
                }
            }
        }
        
        return _currentPath;
    }

    public long GetCurrentFreeSpace()
    {
        var drives = DriveInfo.GetDrives();
        var path = GetCurrentStoragePath();
        var fileInfo = new FileInfo(path);
        var diskName = Path.GetPathRoot(fileInfo.FullName);
        
        foreach (var drive in drives)
        {
            if (drive.Name.Equals(diskName))
            {
                return drive.AvailableFreeSpace;
            }
        }

        return 0;
    }
}