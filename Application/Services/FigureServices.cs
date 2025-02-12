using Application.Common.Pagination;
using Application.DTO;
using Application.Interfaces.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class FigureServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Figure> _figureRepository = unitOfWork.GetRepository<Figure>();
        private readonly IMapper _mapper = mapper;

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

        public async Task<int> CreateFigureAsync(FigureDto dto)
        {
            var figure = _mapper.Map<Figure>(dto);

            await _figureRepository.AddAsync(figure);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateFigureAsync(Guid productId, FigureDto dto)
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
    }
}
