namespace WebFiles.Domain.Entities;

public class StoredFile
{
    public StoredFile(string Name, string Path, string UserId, bool IsPublic, long size)
    {
        if (!Directory.Exists(Path))
        {
            this.Path = Path;
            this.Name = Name;
            this.UserId = UserId;
            this.IsPublic = IsPublic;
            Size = size;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    public int Id { get; private set; }
    public string Name { get; private set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    public string Path { get; private set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    public string UserId { get; private set; }
    public bool IsPublic { get; private set; }
    private long size;
    public long Size
    {
        get => size;
        set{
        if(value < 0){
            throw new System.Exception("Can't set size to value less than zero.");
        }
        else{
            size = value;
        }
    }}

    public void SetName(string newName)
    {
        Name = newName;
    }

    public void SetAvailability(bool isPublic)
    {
        IsPublic = isPublic;
    }
}