using AutoMapper;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;

namespace Identity.Models.Mapper
{
    public class AutoMapperDocumento : Profile
    {
        public AutoMapperDocumento()
        {
            CreateMap<Documento, Document>()
                .ForMember(dest => dest.idDocumento, opt => opt.MapFrom(src => src.IdDocumento))
                .ForMember(dest => dest.idTipoDocumento, opt => opt.MapFrom(src => src.TipoDocumentoId))
                .ForMember(dest => dest.nomeDocumento, opt => opt.MapFrom(src => src.NomeDocumento))
                .ForMember(dest => dest.dataCaricamento, opt => opt.MapFrom(src => src.DataCaricamento));
                //.ForMember(dest => dest.DatiDocumento, opt => opt.MapFrom(src => src.DatiDocumento));

            CreateMap<Document, Documento>()
                .ForMember(dest => dest.TipoDocumentoId, opt => opt.MapFrom(src => src.idTipoDocumento))
                .ForMember(dest => dest.NomeDocumento, opt => opt.MapFrom(src => src.nomeDocumento))
                .ForMember(dest => dest.DataCaricamento, opt => opt.MapFrom(src => src.dataCaricamento.HasValue ? src.dataCaricamento.Value : DateTime.Today));
                //.ForMember(dest => dest.DatiDocumento, opt => opt.MapFrom(src => src.DatiDocumento));

            CreateMap<Documento, DocumentExt>()
                .ForMember(dest => dest.idDocumento, opt => opt.MapFrom(src => src.IdDocumento))
                .ForMember(dest => dest.rowGuid, opt => opt.MapFrom(src => src.UtenteId))
                .ForMember(dest => dest.idTipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento))
                .ForMember(dest => dest.nomeDocumento, opt => opt.MapFrom(src => src.NomeDocumento))
                .ForMember(dest => dest.dataCaricamento, opt => opt.MapFrom(src => src.DataCaricamento))
                .ForMember(dest => dest.datiDocumento, opt => opt.MapFrom(src => Convert.ToBase64String(src.DatiDocumento)));

            CreateMap<TipoDocumento, DocumentType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Descrizione));
        }
    }
}
