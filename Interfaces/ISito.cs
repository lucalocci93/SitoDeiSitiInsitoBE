using SitoDeiSiti.Backend.DTOs;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace SitoDeiSiti.Backend.Interfaces
{
    public interface ISito
    {
        Task<Response<List<Graphics>>> GetGrafiche();
        Task<Response<List<Graphics>>> GetGraficheByPagina(int Pagina);
        Task<Response<Graphics>> AddGrafica(Graphics immagine);
        Task<Response<Graphics>> RemoveGrafica(int Id);
        Task<Response<Graphics>> ToggleGrafica(Graphics immagine);

        Task<Response<List<Pages>>> GetPagine();

        Task<Response<List<Redirections>>> GetRedirezioni();
        Task<Response<Redirections>> GetRedirezione(int Id);
        Task<Response<Redirections>> AddRedirezione(Redirections redirezione);
        Task<Response<Redirections>> RemoveRedirezione(int Id);
        Task<Response<Redirections>> UpdateRedirezione(Redirections redirezione);
        Task<Response<Redirections>> ToggleRedirezione(Redirections redirezione);

        Task<Response<List<Videos>>> GetVideos();
        Task<Response<Videos>> GetVideo(int Id);
        Task<Response<Videos>> AddVideo(Videos video);
        Task<Response<Videos>> RemoveVideo(int Id);
        Task<Response<Videos>> ToggleVideo(Videos video);

        Task<Response<Notification>> CreateNotification(Notification notification);
        Task<Response<Notification>> UpdateNotification(Notification notification);
        Task<Response<List<Notification>>> GetNotificheByPagina(int IdPagina);
        Task<Response<List<Notification>>> GetNotifiche();

        Task<Response<List<TemplateDTO>>> GetTemplates();
        Task<Response<TemplateDTO>> UpdateTemplate(TemplateDTO template);
        Task<Response<TemplateDTO>> CreateTemplate(TemplateDTO template);


    }
}
