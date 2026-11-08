namespace WebFiles.Services.Abstract
{
    public interface IPageService
    {
        public List<T> GetPageSlice<T>(List<T> results, int itemsPerPage,int pageNumber);
        public int GetPagesCount<T>(List<T> results, int itemsPerPage = 10);
    }
}
