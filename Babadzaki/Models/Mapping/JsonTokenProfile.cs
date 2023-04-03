using AutoMapper;

namespace Babadzaki.Models.Mapping
{
    public class JsonTokenProfile : Profile
    {
        public JsonTokenProfile()
        {
            this.CreateMap<JsonToken, Token>()
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dst => dst.Image, opt => opt.MapFrom(src => src.image));

            this.CreateMap<Attribute, TokensFilters>()
                .ForMember(dst => dst.Filter.Name, opt => opt.MapFrom(src => src.trait_type))
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.value));
        }
    }
}
