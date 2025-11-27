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

            CreateMap<IscrizioneEvento, EventSubscription>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.IdEvento))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.IdUtente))
                .ForMember(dest => dest.CompetitionId, opt => opt.MapFrom(src => src.Gara))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Cancellata))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));

            CreateMap<EventSubscription, IscrizioneEvento>()
                .ForMember(dest => dest.IdEvento, opt => opt.MapFrom(src => src.EventId.Value))
                .ForMember(dest => dest.IdUtente, opt => opt.MapFrom(src => src.UserId.Value))
                .ForMember(dest => dest.Gara, opt => opt.MapFrom(src => src.CompetitionId.Value))
                .ForMember(dest => dest.Cancellata, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));


            CreateMap<Categoria, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione));

            CreateMap<Competition, Gare>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Evento, opt => opt.MapFrom(src => src.Event))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.ImportoIscrizione, opt => opt.MapFrom(src => src.CompetitionFee))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Name));

            CreateMap<Gare, Competition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Evento))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categoria))
                .ForMember(dest => dest.CompetitionFee, opt => opt.MapFrom(src => src.ImportoIscrizione))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
