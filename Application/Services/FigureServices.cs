﻿using Application.Common.Pagination;
using Application.DTO;
using Application.DTO.BaseDTO;
using Application.Interfaces.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Services
{
    public class FigureServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Figure> _figureRepository = unitOfWork.GetRepository<Figure>();
        private readonly IMapper _mapper = mapper;
        private const string URLImage = "https://localhost:7241/images/";

        public async Task<IEnumerable<Figure>> GetAllAsync()
        {
            return await _figureRepository.GetAllAsync();
        }

        public async Task<IPagedResult<Figure>> GetPagedAsync(int page, int size = 10)
        {
            var (items, totalCount) = await _figureRepository.GetPagedAsync(page, size);
            return new PagedResult<Figure>(items, totalCount, page, size);
        }

        public async Task<Figure?> GetByIdAsync(Guid productId)
        {
            return await _figureRepository.GetByIdAsync(productId);
        }

        public async Task<int> CreateFigureAsync(FigureCreateDto dto)
        {
            var figure = _mapper.Map<Figure>(dto);

            await _figureRepository.AddAsync(figure);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateFigureAsync(Guid productId, FigureCreateDto dto)
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

        public async Task<IPagedResult<BaseProductDto>> Filter(string? name, string? type,string? vendor, 
            string? category,decimal? minPrice, decimal? maxPrice, int page, int sizePage = 10 )
        {
            if(page <= 0 )
            {
                page = 1;
            }


            Expression<Func<Figure, bool>> filter = f =>
                 (string.IsNullOrEmpty(name) || f.Name.Contains(name)) &&
                 (!minPrice.HasValue || f.Price >= minPrice.Value) &&
                 (!maxPrice.HasValue || f.Price <= maxPrice.Value) &&
                 (string.IsNullOrEmpty(vendor) || f.Vendors.Contains(vendor)) &&
                 (string.IsNullOrEmpty(category) || f.Category.Name.Contains(category));

            var products = await _figureRepository.FilterAll(filter,"Category");
            foreach(var product in products)
            {
                product.ImageUrl =  URLImage + product.ImageUrl;
            }

            var productsDto = _mapper.Map<IEnumerable<BaseProductDto>>(products);

            var result = PageMethod.ToPaginatedList<BaseProductDto>(productsDto, page, sizePage);

            return result;

        }

       
    }
}
