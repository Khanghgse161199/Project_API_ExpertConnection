using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.PageServices
{
    public class PaginateList <T>: List<T>
    {
        public int PageIndex {  get; set; }
        public int TotalPage { get; set; }

        public PaginateList(List<T> items, int count, int indexPage , int pageSize)
        {
            PageIndex = indexPage;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PaginateList<T> CreatePaginateList(IQueryable<T> source, int pageSize, int pageIndex)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
            return new PaginateList<T>(items, count, pageIndex, pageSize);
        }
    }
}
