using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebFiles.Domain.Entities;
using WebFiles.Services.Abstract;

namespace WebFiles.Services
{
    public class PageService : IPageService
    {
        public List<T> GetPageSlice<T>(List<T> results, int itemsPerPage, int pageNumber)
        {
            var pages = getPages(results, itemsPerPage);
            if (!pages.IsNullOrEmpty())
                return pageNumber >= pages.Count ? pages.Last() : pages[pageNumber];
            else
                return new List<T>();
        }

        public int GetPagesCount<T>(List<T> results, int itemsPerPage = 10)
        {
            var pages = (int)Math.Ceiling(results.Count / (double)itemsPerPage);  //Total encount of pages
            return pages;
        }

        private List<List<T>> getPages<T>(List<T> data, int itemsPerPage)
        {
            List<List<T>> res = new List<List<T>>();
            while (data.Any())
            {
                res.Add(data.Take(itemsPerPage).ToList());
                data = data.Skip(itemsPerPage).ToList();
            }
            
            return res;
        }
    }
}
