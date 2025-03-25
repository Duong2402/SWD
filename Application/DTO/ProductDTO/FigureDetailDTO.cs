using Application.DTO.BaseDTO;

namespace Application.DTO.ProductDTO
{
    public class FigureDetailDTO : BaseProductDto
    {
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
