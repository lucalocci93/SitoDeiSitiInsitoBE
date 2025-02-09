using AutoMapper;
using Identity.Models;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;

namespace SitoDeiSitiService.Models.Mapper
{
    public class AutoMapperEvento : Profile
    {
        public AutoMapperEvento()
        {
            CreateMap<Evento, Events>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                //.ForMember(dest => dest.Copertina, opt => opt.MapFrom(src => src.Copertina))
                .ForMember(dest => dest.NomeEvento, opt => opt.MapFrom(src => src.NomeEvento))
                //.ForMember(dest => dest.DataInizioEvento, opt => opt.MapFrom(src => src.DataFineEvento))
                //.ForMember(dest => dest.DataFineEvento, opt => opt.MapFrom(src => src.DataFineEvento))
                .ForMember(dest => dest.LuogoEvento, opt => opt.MapFrom(src => src.LuogoEvento))
                //.ForMember(dest => dest.Categorie, opt => opt.MapFrom(src => src.Categorie))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link));

            CreateMap<Events, Evento>()
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.HasValue ? src.Id.Value : Guid.Empty))
                //.ForMember(dest => dest.Copertina, opt => opt.MapFrom(src => src.Copertina))
                .ForMember(dest => dest.NomeEvento, opt => opt.MapFrom(src => src.NomeEvento))
                //.ForMember(dest => dest.DataInizioEvento, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataInizioEvento)))
                //.ForMember(dest => dest.DataFineEvento, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataFineEvento)))
                .ForMember(dest => dest.LuogoEvento, opt => opt.MapFrom(src => src.LuogoEvento))
                //.ForMember(dest => dest.Categorie, opt => opt.MapFrom(src => src.Categorie))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link));

            CreateMap<IscrizioneEvento, SingleEventSubscription>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.IdEvento))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));

            CreateMap<Categoria, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione));
        }
    }
}
