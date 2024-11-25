using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerCreateDto, Owner>().ReverseMap();
            CreateMap<OwnerUpdateDto, Owner>().ReverseMap();
        }
    }
}
