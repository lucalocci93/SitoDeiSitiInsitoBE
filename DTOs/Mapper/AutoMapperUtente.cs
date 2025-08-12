using AutoMapper;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;

namespace Identity.Models.Mapper
{
    public class AutoMapperUtenteProfile : Profile
    {
        public AutoMapperUtenteProfile()
        {
            CreateMap<Utente, User>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Cognome, opt => opt.MapFrom(src => src.Cognome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CodFiscale, opt => opt.MapFrom(src => src.CodFiscale))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin))
                .ForMember(dest => dest.IsMaestro, opt => opt.MapFrom(src => src.IsMaestro))
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.DataNascita, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UtenteInfo.DataNascita.Value)))
                .ForMember(dest => dest.Via, opt => opt.MapFrom(src => src.UtenteInfo.Via))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.UtenteInfo.Numero))
                .ForMember(dest => dest.Citta, opt => opt.MapFrom(src => src.UtenteInfo.Citta))
                .ForMember(dest => dest.Regione, opt => opt.MapFrom(src => src.UtenteInfo.Regione))
                .ForMember(dest => dest.Nazione, opt => opt.MapFrom(src => src.UtenteInfo.Nazione))
                .ForMember(dest => dest.Organizzazione, opt => opt.MapFrom(src => src.UtenteAtleta.Organizzazione))
                .ForMember(dest => dest.Cintura, opt => opt.MapFrom(src => src.UtenteAtleta.Cintura))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.Empty));

            CreateMap<User, Utente>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Cognome, opt => opt.MapFrom(src => src.Cognome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CodFiscale, opt => opt.MapFrom(src => src.CodFiscale))
                //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin))
                .ForMember(dest => dest.IsMaestro, opt => opt.MapFrom(src => src.IsMaestro))
                .ForMember(dest => dest.Abbonamento, opt => opt.MapFrom(src => src.Abbonamenti));

            CreateMap<User, UtenteInfo>()
                .ForMember(dest => dest.DataNascita, opt => opt.MapFrom(src => new DateTime(src.DataNascita.Value.Year, src.DataNascita.Value.Month, src.DataNascita.Value.Day)))
                .ForMember(dest => dest.Via, opt => opt.MapFrom(src => src.Via))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.Citta, opt => opt.MapFrom(src => src.Citta))
                .ForMember(dest => dest.Regione, opt => opt.MapFrom(src => src.Regione))
                .ForMember(dest => dest.Nazione, opt => opt.MapFrom(src => src.Nazione))
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid));

            CreateMap<UtenteInfo, User>()
                .ForMember(dest => dest.DataNascita, opt => opt.MapFrom(src => src.DataNascita))
                .ForMember(dest => dest.Via, opt => opt.MapFrom(src => src.Via))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.Citta, opt => opt.MapFrom(src => src.Citta))
                .ForMember(dest => dest.Regione, opt => opt.MapFrom(src => src.Regione))
                .ForMember(dest => dest.Nazione, opt => opt.MapFrom(src => src.Nazione))
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid));

            CreateMap<User, UtentePrivacy>()
                .ForMember(dest => dest.ConsensoInvioMail, opt => opt.MapFrom(src => src.ConsensoInvioMail))
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid));

            CreateMap<User, UtenteAtleta>()
                .ForMember(dest => dest.Rowguid, opt => opt.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.Cintura, opt => opt.MapFrom(src => src.Cintura))
                .ForMember(dest => dest.Organizzazione, opt => opt.MapFrom(src => src.Organizzazione));

            CreateMap<Cinture, Belts>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Tipo));

            CreateMap<Belts, Cinture>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Type));

            CreateMap<Organizzazioni, Organization>()
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Indirizzo))
                .ForMember(dest => dest.PIva, opt => opt.MapFrom(src => src.PartitaIva));

            CreateMap<Organization, Organizzazioni>()
                .ForMember(dest => dest.RowGuid, opt => opt.MapFrom(src => src.RowGuid))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Indirizzo, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PartitaIva, opt => opt.MapFrom(src => src.PIva));

        }
    }
}
