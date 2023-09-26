using AutoMapper;
using Yukidzaki_Domain.Models;

namespace Yukidzaki_Domain.Mappings
{
    public class JsonTokenProfile : Profile
    {
        public JsonTokenProfile()
        {
            CreateMap<JsonToken, Token>()

                .ForMember(dst => dst.TokensFilters, opt => opt.Ignore())
                .ForMember(dst => dst.edition, opt => opt.MapFrom(src => src.edition))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.image));
                





            //this.CreateMap<Attribute, TokensFilters>()
            //    .ForMember(dst => dst.Filter.Name, opt => opt.MapFrom(src => src.trait_type))
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.value));
        }
    }
}
