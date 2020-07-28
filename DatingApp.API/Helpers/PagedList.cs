using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            //example 13/5 = 3, we need 3 pages (2 full, one with 3 items) to display every item hence the celling function.
            TotalPages = (int)Math.Ceiling(count /(double)pageSize);
            this.AddRange(items);
        }
        //Method used to create new PagedList 
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            //Example with 13 items returing page 2 : (2 - 1) * 5 = skip first 5  (page 1) and take next 5 (page 2);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}