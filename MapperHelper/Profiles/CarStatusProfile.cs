using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class CarStatusProfile : Profile
    {
        public CarStatusProfile()
        {
            CreateMap<CarStatus, CarStatusDto>();
            CreateMap<CarStatusCreateDto, CarStatus>().ReverseMap();
            CreateMap<CarStatusUpdateDto, CarStatus>().ReverseMap();
        }
    }
}
