using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public static async Task<string> SaveImageAsync(IFormFile image, IWebHostEnvironment _env)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"{uniqueFileName}"; 
        }

        public static async Task<bool> DeleteImageAsync(string fileName, IWebHostEnvironment _env)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("File name is invalid.");
                return false;
            }

            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"Image deleted: {fileName}");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting image {fileName}: {ex.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"File not found: {fileName}");
                return false;
            }
        }

    }
}
