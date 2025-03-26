using Application.Common.Pagination;
using Application.DTO;
using Application.DTO.BaseDTO;
using Application.DTO.ProductDTO;
using Application.Interfaces.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services
{
    public class ProductServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Product> _figureRepository = unitOfWork.GetRepository<Product>();
        private readonly IGenericRepository<Category> _cateRepository = unitOfWork.GetRepository<Category>();
        private readonly IGenericRepository<Media> _mediaRepo = unitOfWork.GetRepository<Media>();
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _env = env;
        private const string URLImageRoot = "https://localhost:7241/images/";

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _figureRepository.GetAllAsync();
        }

        public async Task<IPagedResult<Product>> GetPagedAsync(int page, int size = 10)
        {
            var (items, totalCount) = await _figureRepository.GetPagedAsync(page, size);
            return new PagedResult<Product>(items, totalCount, page, size);
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await _figureRepository.GetByIdAsync(productId);
        }

        public async Task<int> CreateFigureAsync(ProductCreateDto dto)
        {
            var figure = _mapper.Map<Product>(dto);

            figure.Media = new List<Media>();

            if(dto.Media != null && dto.Media.Any())
            {
                foreach(var image in dto.Media)
                {
                    var imagePath = await PageMethod.SaveImageAsync(image,_env);
                    figure.Media.Add(new Media { Url = imagePath });
                }
            }

            await _figureRepository.AddAsync(figure);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateFigureAsync(Guid productId, ProductCreateDto dto)
        {
            var figures = await _figureRepository.FilterAll(f => f.Id == productId);
            var figure = figures.AsQueryable()
                .Include(f => f.Media)
                .FirstOrDefault()
                     ?? throw new KeyNotFoundException("Could not find requested product.");

            if (figure.Media == null)
            {
                figure.Media = new List<Media>();
            }

            var oldMedia = figure.Media.ToList();
            var newMediaFiles = dto.Media ?? new List<IFormFile>();
            var newMediaNames = newMediaFiles.Select(m => m.FileName).ToList();

            var imagesToDelete = oldMedia.Where(o => !newMediaNames.Contains(Path.GetFileName(o.Url))).ToList();

            foreach (var image in imagesToDelete)
            {
                await PageMethod.DeleteImageAsync(image.Url, _env);
                figure.Media.Remove(image); 
            }

            foreach (var media in newMediaFiles)
            {
                var imagePath = await PageMethod.SaveImageAsync(media, _env);
                figure.Media.Add(new Media { Url = imagePath });
            }

            _mapper.Map(dto, figure);
            await _figureRepository.UpdateAsync(figure);
            return await _unitOfWork.SaveChangesAsync();
        }



        public async Task<int> DeleteFigureAsync(Guid productId)
        {
            _ = await _figureRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Could not find requested product.");

            await _figureRepository.DeleteAsync(productId);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IPagedResult<BaseProductDto>> Filter(string? name,
            string? category, double? minPrice, double? maxPrice, int page, int sizePage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }

            Expression<Func<Product, bool>> filter = f =>
                 (string.IsNullOrEmpty(name) || f.Name.Contains(name)) &&
                 (!minPrice.HasValue || f.Price >= minPrice.Value) &&
                 (!maxPrice.HasValue || f.Price <= maxPrice.Value) &&
                 (string.IsNullOrEmpty(category) || f.Category.Name.Contains(category));

            var products = await _figureRepository.FilterAll(filter, "Media,Category");
            foreach (var product in products)
            {
                var mediaList = product.Media.ToList();
                if (mediaList != null)
                {
                    foreach (var item in mediaList)
                    {
                        item.Url = URLImageRoot + item.Url;
                    }
                }

            }

            var productsDto = _mapper.Map<IEnumerable<BaseProductDto>>(products);

            var result = PageMethod.ToPaginatedList<BaseProductDto>(productsDto, page, sizePage);

            return result;

        }

        public async Task<FigureDetailDTO> FigureDetailDTO(Guid productId)
        {
            var product = await _figureRepository.GetDetailById(productId, "Media");
            if (product == null)
            {
                throw new KeyNotFoundException("Could not find requested product.");
            }


            var mediaList = product.Media.ToList();
            Console.WriteLine("Media profile: "+mediaList.Count());
            if (mediaList != null)
            {
                foreach (var item in mediaList)
                {
                    item.Url = URLImageRoot + item.Url;
                    Console.WriteLine(item.Url);
                }
            }
            var productDto = _mapper.Map<FigureDetailDTO>(product);
            return productDto;
        }

        public async Task<IEnumerable<CategoryDto>> CategoryList()
        {
            try
            {
                var categories = await _cateRepository.GetAllAsync();

                if (categories == null || !categories.Any())
                {
                    return new List<CategoryDto>(); 
                }

                var cateDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return cateDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách Category: {ex.Message}");

                return new List<CategoryDto>();
            }
        }



    }
}
