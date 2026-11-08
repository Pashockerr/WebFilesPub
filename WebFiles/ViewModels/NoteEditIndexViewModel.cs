namespace WebFiles.ViewModels;

public class NoteEditIndexViewModel
{
    public int NoteId;
    public string Name;
    public string Title;
    public string Text;
    public bool IsPublic;

    public NoteEditIndexViewModel(int noteId, string name, string title, string text, bool isPublic)
    {
        NoteId = noteId;
        Name = name;
        Title = title;
        Text = text;
        IsPublic = isPublic;
    }
}