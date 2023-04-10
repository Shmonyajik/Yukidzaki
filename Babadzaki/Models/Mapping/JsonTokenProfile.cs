using AutoMapper;

namespace Babadzaki.Models.Mapping
{
    public class JsonTokenProfile : Profile
    {
        public JsonTokenProfile()
        {
            this.CreateMap<JsonToken, Token>().ForMember(dst => dst.dna, opt => opt.MapFrom(src => long.Parse(src.dna)));
                
                


            //this.CreateMap<Attribute, TokensFilters>()
            //    .ForMember(dst => dst.Filter.Name, opt => opt.MapFrom(src => src.trait_type))
            //    .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.value));
        }
    }
}
