namespace WebFiles.ViewModels
{
    public class FileIndexViewModel
    {
        public string AllowedUploadSize { get; set; }
        
        public int RemainingSpace { get; set; }

        public FileIndexViewModel(string allowedUploadSize, int remainingSpace)
        {
            AllowedUploadSize = allowedUploadSize;
            RemainingSpace = remainingSpace;
        }
    }
}
