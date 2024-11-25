using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CarCreateDto, Car>().ReverseMap();
            CreateMap<CarUpdateDto, Car>().ReverseMap();
        }
    }
}
