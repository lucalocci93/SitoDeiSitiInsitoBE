using AutoMapper;
using SitoDeiSiti.Backend.DTOs;
using SitoDeiSiti.DAL.Models;
using System.Text.RegularExpressions;

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


            CreateMap<Graphics, Grafiche>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UrlImmagine, opt => opt.MapFrom(src => src.UrlImage))
                .ForMember(dest => dest.Pagina, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.Sezione, opt => opt.MapFrom(src => src.Section))
                .ForMember(dest => dest.UrlFromGoogleDrive, opt => opt.MapFrom(src => src.UrlFromGoogleDrive))
                .ForMember(dest => dest.Titolo, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TestoAggiuntivo, opt => opt.MapFrom(src => src.AdditionalText))
                .ForMember(dest => dest.IsTestoAggiuntivoMarkdown, opt => opt.MapFrom(src => IsMarkdown(src.AdditionalText)))
                .ForMember(dest => dest.Ordine, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.Attiva, opt => opt.MapFrom(src => src.Active));

            CreateMap<Grafiche, Graphics>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImmagine))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Pagina))
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.Sezione))
                .ForMember(dest => dest.UrlFromGoogleDrive, opt => opt.MapFrom(src => src.UrlFromGoogleDrive))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titolo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.AdditionalText, opt => opt.MapFrom(src => src.TestoAggiuntivo))
                .ForMember(dest => dest.IsAdditionalTextMarkdown, opt => opt.MapFrom(src => IsMarkdown(src.TestoAggiuntivo)))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Ordine))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Attiva));

            CreateMap<Redirezioni, Redirections>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.active, opt => opt.MapFrom(src => src.Attiva));

            CreateMap<Redirections, Redirezioni>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id.HasValue ? src.id.Value : 0))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.url))
                .ForMember(dest => dest.Attiva, opt => opt.MapFrom(src => src.active));

            CreateMap<Video, Videos>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titolo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descrizione))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Attivo));

            CreateMap<Videos, Video>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Titolo, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider))
                .ForMember(dest => dest.Attivo, opt => opt.MapFrom(src => src.Active));

            CreateMap<Template, TemplateDTO>()
                .ForMember(dest => dest.TemplateName, opt => opt.MapFrom(src => src.TemplateName))
                .ForMember(dest => dest.TemplateBodyHtml, opt => opt.MapFrom(src => src.TemplateBodyHtml))
                .ForMember(dest => dest.TemplateHeaderHtml, opt => opt.MapFrom(src => src.TemplateHeaderHtml))
                .ForMember(dest => dest.TemplateFooterHtml, opt => opt.MapFrom(src => src.TemplateFooterHtml));

            CreateMap<TemplateDTO, Template>()
                .ForMember(dest => dest.TemplateName, opt => opt.MapFrom(src => src.TemplateName))
                .ForMember(dest => dest.TemplateBodyHtml, opt => opt.MapFrom(src => src.TemplateBodyHtml))
                .ForMember(dest => dest.TemplateHeaderHtml, opt => opt.MapFrom(src => src.TemplateHeaderHtml))
                .ForMember(dest => dest.TemplateFooterHtml, opt => opt.MapFrom(src => src.TemplateFooterHtml));

            CreateMap<Notification, Notifica>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Pagina, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.Testo, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Abilitata, opt => opt.MapFrom(src => src.Active));

            CreateMap<Notifica, Notification>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Pagina))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Testo))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Abilitata));

        }

        private static bool IsMarkdown(string text)
        {
            // Regular expressions to match common Markdown syntax
            string[] markdownPatterns = {
            @"^#{1,6}\s", // Headers
            @"\*\*.*\*\*", // Bold
            @"\*.*\*", // Italics
            @"\!\[.*\]\(.*\)", // Images
            @"\[.*\]\(.*\)", // Links
            @"\`.*\`", // Inline code
            @"\n\s*[-*+]\s", // Lists
            @"\n\s*\d+\.\s" // Ordered lists
        };

            foreach (var pattern in markdownPatterns)
            {
                if (Regex.IsMatch(text, pattern))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
