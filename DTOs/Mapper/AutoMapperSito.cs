using AutoMapper;
using SitoDeiSiti.DAL.Models;

namespace SitoDeiSiti.DTOs.Mapper
{
    public class AutoMapperSito : Profile
    {
        public AutoMapperSito()
        {
            CreateMap<Pages, Pagine>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Pagina, opt => opt.MapFrom(src => src.Page));
            
            CreateMap<Pagine, Pages>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Pagina));


            CreateMap<Images, Immagini>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UrlImmagine, opt => opt.MapFrom(src => src.UrlImage))
                .ForMember(dest => dest.Pagina, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.Sezione, opt => opt.MapFrom(src => src.Section))
                .ForMember(dest => dest.UrlFromGoogleDrive, opt => opt.MapFrom(src => src.UrlFromGoogleDrive))
                .ForMember(dest => dest.Titolo, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TestoAggiuntivo, opt => opt.MapFrom(src => src.AdditionalText))
                .ForMember(dest => dest.Ordine, opt => opt.MapFrom(src => src.Order));

            CreateMap<Immagini, Images>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImmagine))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Pagina))
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.Sezione))
                .ForMember(dest => dest.UrlFromGoogleDrive, opt => opt.MapFrom(src => src.UrlFromGoogleDrive))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titolo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.AdditionalText, opt => opt.MapFrom(src => src.TestoAggiuntivo))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Ordine));

        }
    }
}
