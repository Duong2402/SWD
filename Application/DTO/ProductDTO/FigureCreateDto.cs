using Application.DTO.BaseDTO;

namespace Application.DTO.ProductDTO
{
    public class FigureCreateDto : BaseProductDto
    {
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
