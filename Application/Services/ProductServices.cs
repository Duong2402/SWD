using Application.Common.Pagination;
using Application.DTO;
using Application.DTO.BaseDTO;
using Application.DTO.ProductDTO;
using Application.Interfaces.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Services
{
    public class ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Product> _figureRepository = unitOfWork.GetRepository<Product>();
        private readonly IMapper _mapper = mapper;
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

            await _figureRepository.AddAsync(figure);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateFigureAsync(Guid productId, ProductCreateDto dto)
        {
            var figure = await _figureRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Could not find requested product.");

            _mapper.Map(dto, figure);

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

            var products = await _figureRepository.FilterAll(filter, "Media");
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


    }
}
