using Application.Common.Pagination;
using Application.DTO;
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

        public async Task<IPagedResult<ProductDto>> Filter(string? name, string? type, 
            string? category,decimal? minPrice, decimal? maxPrice, int page, int sizePage = 10 )
        {
            if(page <= 0 )
            {
                page = 1;
            }


            Expression<Func<Product, bool>> filter = p =>
                 (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
                 (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                 (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
                 (string.IsNullOrEmpty(category) || p.Category.Name.Contains(category));

            var products = await _figureRepository.FilterAll(filter,"Category");
            foreach(var product in products)
            {
                product.Media.Url =  URLImageRoot + product.Media.Url;
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            var result = PageMethod.ToPaginatedList<ProductDto>(productsDto, page, sizePage);

            return result;

        }
    }
}
