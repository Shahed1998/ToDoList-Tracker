﻿using Microsoft.EntityFrameworkCore;

namespace Web.Models.Business_Entities
{
    public class Pager<T> : List<T> where T : class
    {
        public int PageIndex  { get; private set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public Pager() { }  

        public Pager(List<T> items, int count, int pageIndex, int pageSize)
        { 
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            TotalCount = count;
            PageSize = pageSize;
            this.AddRange(items);
        }

        public bool PreviousPage
        {
            get
            {
                return PageIndex > 1;
            }
        }

        public bool NextPage
        {
            get
            {
                return PageIndex < TotalPages;
            }
        }

        public static async Task<Pager<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex-1) * pageSize).Take(pageSize).ToListAsync();
            return new Pager<T>(items, count, pageIndex, pageSize);
        }
    }
}
