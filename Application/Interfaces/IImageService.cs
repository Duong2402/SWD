using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
    }
}
