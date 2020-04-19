using AutoMapper;
using ParkyApi.Models;
using ParkyApi.Models.Dto;

namespace ParkyApi.ParkyMapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
