namespace WebFiles.Domain.Entities
{
    public class Note
    {
        public Note(string Name, string Title, string Text, bool IsPublic, string UserId)
        {
            if (Name.Length <= 128)
                this.Name = Name;
            else
                throw new ArgumentException("Name can't be more than 128 characters lenght");

            if (Title.Length <= 512)
                this.Title = Title;
            else
                throw new ArgumentException("Title can't be more than 512 characters lenght");

            if (Text.Length <= 10_000)
                this.Text = Text;
            else
                throw new ArgumentException("Text can't be more than 10_000 characters lenght");

            this.IsPublic = IsPublic;
            this.UserId = UserId;
        }

        public int Id { get; set; }
        public string Name { get; private set; } //Limit is 128 characters
        public string Title { get; private set; } //Limit is 512 characters
        public string Text { get; private set; } //Limit is 10_000 characters

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public bool IsPublic { get; private set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string UserId { get; set; }

        public void SetName(string newName)
        {
            if (newName.Length <= 128)
                Name = newName;
            else
                throw new ArgumentException("Name can't be more than 128 characters lenght");
        }

        public void SetTitle(string newTitle)
        {
            if (newTitle.Length <= 512)
                Title = newTitle;
            else
                throw new ArgumentException("Title can't be more than 512 characters lenght");
        }

        public void SetText(string newText)
        {
            if (newText.Length <= 10_000)
                Text = newText;
            else
                throw new ArgumentException("Text can't be more than 10_000 characters lenght");
        }

        public void SetAvailability(bool isPublic)
        {
            IsPublic = isPublic;
        }
    }
}


