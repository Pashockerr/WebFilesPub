namespace WebFiles.ViewModels
{
    public class HomeIndexViewModel
    {
        public bool IsAdmin { get; set; }

        public HomeIndexViewModel(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }
}
