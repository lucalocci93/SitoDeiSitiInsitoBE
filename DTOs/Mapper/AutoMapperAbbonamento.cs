using AutoMapper;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;

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
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Attivo.HasValue ? src.Attivo.Value : false))
                .ForMember(dest => dest.UrlPagamento, opt => opt.MapFrom(src => src.UrlPagamento))
                .ForMember(dest => dest.Importo, opt => opt.MapFrom(src => src.Importo))
                .ForMember(dest => dest.IsPayed, opt => opt.MapFrom(src => src.Pagato))
                .ForMember(dest => dest.IdCheckout, opt => opt.MapFrom(src => src.IdCheckout));

            CreateMap<Subscription, Abbonamento>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoAbbonamento, opt => opt.MapFrom(src => src.IdTipoAbbonamento))
                .ForMember(dest => dest.DataIscrizione, opt => opt.MapFrom(src => src.DataIscrizione))
                .ForMember(dest => dest.DataScadenza, opt => opt.MapFrom(src => src.DataScadenza))
                .ForMember(dest => dest.Attivo, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.UrlPagamento, opt => opt.MapFrom(src => src.UrlPagamento))
                .ForMember(dest => dest.Pagato, opt => opt.MapFrom(src => src.IsPayed))
                .ForMember(dest => dest.IdCheckout, opt => opt.MapFrom(src => src.IdCheckout));

            CreateMap<Subscription, TipoAbbonamento>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdTipoAbbonamento))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.TipoAbbonamento));

            CreateMap<TipoAbbonamento, Subscription>()
                .ForMember(dest => dest.IdTipoAbbonamento, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoAbbonamento, opt => opt.MapFrom(src => src.Descrizione));

            CreateMap<TipoAbbonamento, SubscriptionType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione));
        }
    }
}
