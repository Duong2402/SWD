﻿using Application.DTO;
using Application.DTO.BaseDTO;
using AutoMapper;
using Domain.Entities;


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
            CreateMap<Figure, FigureCreateDto>().ReverseMap();
            CreateMap<Figure, BaseProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
