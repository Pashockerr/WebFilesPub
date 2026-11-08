namespace WebFiles.ViewModels;

public class ErrorIndexViewModel
{
    public ErrorIndexViewModel(string title, string subtitle)
    {
        Title = title;
        Subtitle = subtitle;
    }

    public string Title { get; set; }
    public string Subtitle { get; set; }
}