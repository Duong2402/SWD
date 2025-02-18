using Application.DTO.BaseDTO;

namespace Application.DTO
{
    public class FigureCreateDto : BaseProductDto
    {
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
