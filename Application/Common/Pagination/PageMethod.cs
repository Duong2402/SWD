﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Common.Pagination
{
    public class PageMethod
    {
        public static PagedResult<T> ToPaginatedList<T>(IEnumerable<T> source, int currentPage, int pageSize)
        {
            if (currentPage < 1) currentPage = 1;
            if (pageSize < 1) pageSize = 6;

            Console.WriteLine($"page size in handle: {pageSize}");
            Console.WriteLine($"current page in handle: {currentPage}");
            Console.WriteLine($"Source: {source.Count()}");

            var totalCount = source.Count();
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            Console.WriteLine($"page size in handle: {items.Count()}");

            return new PagedResult<T>(items, totalCount, currentPage, pageSize);
        }

        public static IQueryable<T> IncludeClass<T> (IQueryable<T> query, string includeProperties) where T : class
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }
    }
}
