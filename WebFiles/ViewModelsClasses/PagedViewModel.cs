namespace WebFiles.ViewModelsClasses
{
    public class PagedViewModel
    {
        public PagedViewModel(int pagesTotal, int currentPage, bool notEmpty)
        {
            PagesTotal = pagesTotal;
            CurrentPage = currentPage;
            NotEmpty = notEmpty;
        }

        public int PagesTotal;

        public int CurrentPage;

        public bool NotEmpty;
    }
}
