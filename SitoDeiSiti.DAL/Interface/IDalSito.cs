using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL.Interface
{
    public interface IDalSito
    {
        public Task<List<Pagine>> GetPagine();

        public Task<List<Grafiche>> GetGrafiche();
        public Task<List<Grafiche>> GetGraficheByPagina(int Pagina);
        public Task<Grafiche> GetGrafica(int Id);
        public Task<Grafiche> AddGrafica(Grafiche grafica);
        public Task<bool> RemoveGrafica(int Id);
        public Task<bool> AttivaDisattivaGrafica(Grafiche grafica);


        public Task<List<Redirezioni>> GetRedirezioni();
        public Task<Redirezioni> GetRedirezione(int Id);
        public Task<Redirezioni> AddRedirezione(Redirezioni redirezione);
        public Task<bool> RemoveRedirezione(int Id);
        public Task<bool> UpdateRedirezione(Redirezioni redirezione);
        public Task<bool> AttivaDisattivaRedirezione(Redirezioni redirezione);

        public Task<List<Video>> GetVideo();
        public Task<Video> GetVideo(int Id);
        public Task<Video> AddVideo(Video video);
        public Task<bool> RemoveVideo(int Id);
        public Task<bool> UpdateVideo(Video video);
        public Task<bool> AttivaDisattivaVideo(Video video);

        public Task<Template> GetTemplateByName(string TemplateName);

        public Task<bool> AddNotifica(Notifica notifica);
        public Task<bool> UpdateNotifica(Notifica notifica);
        public Task<List<Notifica>> GetNotificheByPagina(int idPagina);
        public Task<List<Notifica>> GetNotifiche();

        public Task<List<Template>> GetTemplates();
        public Task<bool> UpdateTemplate(Template template);
        public Task<bool> AddTemplate(Template template);

    }
}
