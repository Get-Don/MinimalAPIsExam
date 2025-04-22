using ApiServer.GameData;
using ApiServer.Protocol;
using AutoMapper;
using Repository.Entyties;

namespace ApiServer;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Source, Destination>
        CreateMap<Account, AccountDTO>().ReverseMap();
        
        CreateMap<Money, MoneyDTO>()
            .ForMember(dest => dest.MoneyType, opt => opt.MapFrom(src => (MoneyType)src.MoneyType))
            .ReverseMap()
            .ForMember(dest => dest.MoneyType, opt => opt.MapFrom(src => (byte)src.MoneyType));
        
        CreateMap<Stat, StatDTO>()
            .ForMember(dest => dest.StatType, opt => opt.MapFrom(src => (StatType)src.StatType))
            .ReverseMap()
            .ForMember(dest => dest.StatType, opt => opt.MapFrom(src => (byte)src.StatType));
    }
}