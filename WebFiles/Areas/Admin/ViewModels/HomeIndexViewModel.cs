namespace WebFiles.Areas.Admin.ViewModels;

public class HomeIndexViewModel
{
    public long FreeSpace;
    
    public HomeIndexViewModel(long freeSpace)
    {
        FreeSpace = freeSpace;
    }
}