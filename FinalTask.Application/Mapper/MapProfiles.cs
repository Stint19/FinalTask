using AutoMapper;
using FinalTask.Application.Dtos;
using FinalTask.Domain.Models;

namespace FinalTask.Application.Mapper
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<ProductModel, Product>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Product, ProductModel>();
        }
    }
}
