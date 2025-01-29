using AutoMapper;
using Identity.DTOs;
using SitoDeiSiti.DAL.Models;

namespace Identity.Models.Mapper
{
    public class AutoMapperAbbonamentoProfile : Profile
    {
        public AutoMapperAbbonamentoProfile()
        {
            CreateMap<Abbonamento, Subscription>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoAbbonamento, opt => opt.MapFrom(src => src.TipoAbbonamentoNavigation.Descrizione))
                .ForMember(dest => dest.DataIscrizione, opt => opt.MapFrom(src => src.DataIscrizione))
                .ForMember(dest => dest.DataScadenza, opt => opt.MapFrom(src => src.DataScadenza))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.DataIscrizione <= DateOnly.FromDateTime(DateTime.Now) && src.DataScadenza >= DateOnly.FromDateTime(DateTime.Now)));

            CreateMap<Subscription, Abbonamento>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoAbbonamento, opt => opt.MapFrom(src => src.IdTipoAbbonamento))
                .ForMember(dest => dest.DataIscrizione, opt => opt.MapFrom(src => src.DataIscrizione))
                .ForMember(dest => dest.DataScadenza, opt => opt.MapFrom(src => src.DataScadenza));

            CreateMap<TipoAbbonamento, Subscription>()
                .ForMember(dest => dest.IdTipoAbbonamento, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoAbbonamento, opt => opt.MapFrom(src => src.Descrizione));

            CreateMap<TipoAbbonamento, SubscriptionType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione));
        }
    }
}
