using Application.DTO;
using Application.DTO.BaseDTO;
using Application.DTO.ProductDTO;
using AutoMapper;
using Domain.Entities;
using Application.DTO.BaseDTO;


namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        // Configure object mappings o day
        // Them CreateMap<[From], [To]>.ReverseMap() vao parameterless constructor
        // Dung ReverseMap() de co 2-way mapping

        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<Product, BaseProductDto>()
             .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Media)) // Mapping Media -> ImageUrl
             .ForMember(dest => dest.CategoryDto, opt => opt.MapFrom(src => src.Category)); // Mapping Category -> CategoryDto
           
            CreateMap<Product, FigureDetailDTO>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Media)); // Mapping Media -> ImageUrl

            CreateMap<Media, MediaDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Bỏ qua ID (sẽ tự tạo)
            .ForMember(dest => dest.Media, opt => opt.Ignore()) // Media sẽ được xử lý riêng nếu cần
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => Guid.Parse(src.CategoryId))) // Chuyển đổi CategoryId từ string sang Guid
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? "")) // Nếu Description null, gán chuỗi rỗng
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Bỏ qua vì là BaseEntity
            .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore()); // Bỏ qua vì là BaseEntity

        }
    }
}
