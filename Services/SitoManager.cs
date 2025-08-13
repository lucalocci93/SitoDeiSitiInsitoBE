using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using SitoDeiSiti.Backend.DTOs;
using SitoDeiSiti.Backend.Interfaces;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Enums;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.DTOs.ConfigSettings;
using SitoDeiSiti.Models;
using SitoDeiSiti.Validators;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace SitoDeiSiti.Backend.Services
{
    public class SitoManager : BaseManager, ISito
    {
        private readonly IDalSito dalSito;
        private readonly IOptions<UrlRedirezione> Config;

        public SitoManager(SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache,
            IOptions<UrlRedirezione> config)
            : base(mapper, hybridCache)
        {
            dalSito = new DalSito(context);
            Config = config;
        }

        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;

        private enum CacheKey
        {
            GetAllImages,
            GetImagesByPage,
            GetPages,
            GetRedirections,
            GetVideos,
            GetNotification,
            GetTemplates
        }

        public async Task<Response<List<Graphics>>> GetGrafiche()
        {
            List<Graphics> immagini = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetAllImages);//GetActualAsyncMethodName();

                immagini = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Graphics>>(await dalSito.GetGrafiche().ConfigureAwait(false)));

                if (immagini != null && immagini.Any())
                {
                    return new Response<List<Graphics>>(true, immagini);
                }
                else
                {
                    return new Response<List<Graphics>>(false, new Error("Nessuna grafica trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<List<Graphics>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Graphics>>> GetGraficheByPagina(int Pagina)
        {
            List<Graphics> immagini = null;
            try
            {
                string cacheKey = string.Concat(nameof(CacheKey.GetImagesByPage), '_', Pagina);//GetActualAsyncMethodName();

                immagini = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Graphics>>(await dalSito.GetGraficheByPagina(Pagina).ConfigureAwait(false)));

                if (immagini != null && immagini.Any())
                {
                    return new Response<List<Graphics>>(true, immagini);
                }
                else
                {
                    return new Response<List<Graphics>>(true, immagini);
                }

            }
            catch (Exception ex)
            {
                return new Response<List<Graphics>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Pages>>> GetPagine()
        {
            List<Pages> pages = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetPages);//GetActualAsyncMethodName();

                pages = await HybridCache.GetOrCreateAsync<List<Pages>>(cacheKey, async result => Mapper.Map<List<Pages>>(await dalSito.GetPagine().ConfigureAwait(false)));

                if (pages != null && pages.Any())
                {
                    return new Response<List<Pages>>(true, pages);
                }
                else
                {
                    return new Response<List<Pages>>(false, new Error("Nessuna pagina trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Pages>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Graphics>> AddGrafica(Graphics immagine)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                foreach (var page in Enum.GetValues(typeof(DAL.Enums.Pagine)))
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetImagesByPage), '_', (int)page)).ConfigureAwait(false);
                }

                await dalSito.AddGrafica(Mapper.Map<Grafiche>(immagine)).ConfigureAwait(false);

                return new Response<Graphics>(true, immagine);

            }
            catch (Exception ex)
            {
                return new Response<Graphics>(false, new Error(ex.Message));
            }
        }

        public async Task<bool> EmptyCache()
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);

                foreach(var page in Enum.GetValues(typeof(DAL.Enums.Pagine)))
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetImagesByPage), '_', (int)page)).ConfigureAwait(false);
                }

                await HybridCache.RemoveAsync(nameof(CacheKey.GetNotification)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetRedirections)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetPages)).ConfigureAwait(false);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public async Task<Response<Graphics>> RemoveGrafica(int Id)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                foreach (var page in Enum.GetValues(typeof(DAL.Enums.Pagine)))
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetImagesByPage), '_', (int)page)).ConfigureAwait(false);
                }

                var result = await dalSito.RemoveGrafica(Id).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Graphics>(true, new Graphics() { Id = Id});
                }
                else
                {
                    return new Response<Graphics>(false, new Error("Immagine non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Graphics>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Graphics>> ToggleGrafica(Graphics graphic)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                foreach (var page in Enum.GetValues(typeof(DAL.Enums.Pagine)))
                {
                    await HybridCache.RemoveAsync(string.Concat(nameof(CacheKey.GetImagesByPage), '_', (int)page)).ConfigureAwait(false);
                }

                var result = await dalSito.AttivaDisattivaGrafica(Mapper.Map<Grafiche>(graphic)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Graphics>(true, new Graphics() { Id = graphic.Id });
                }
                else
                {
                    return new Response<Graphics>(false, new Error("Grafica non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Graphics>(false, new Error(ex.Message));
            }
        }


        public async Task<Response<List<Redirections>>> GetRedirezioni()
        {
            List<Redirections> redirections = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetRedirections);//GetActualAsyncMethodName();

                redirections = await HybridCache.GetOrCreateAsync<List<Redirections>>(cacheKey, async result => Mapper.Map<List<Redirections>>(await dalSito.GetRedirezioni().ConfigureAwait(false)));

                if (redirections != null && redirections.Any())
                {
                    foreach(var redirection in redirections)
                    {
                        redirection.redirectUrl = string.Concat(Config.Value.Url, redirection.id);
                    }

                    return new Response<List<Redirections>>(true, redirections);
                }
                else
                {
                    return new Response<List<Redirections>>(false, new Error("Nessuna redirezione trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Redirections>>(false, new Error(ex.Message));
            }

        }

        public async Task<Response<Redirections>> AddRedirezione(Redirections redirezione)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetRedirections)).ConfigureAwait(false);

                redirezione.redirectUrl = Config.Value.Url;

                await dalSito.AddRedirezione(Mapper.Map<Redirezioni>(redirezione)).ConfigureAwait(false);

                return new Response<Redirections>(true, redirezione);

            }
            catch (Exception ex)
            {
                return new Response<Redirections>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Redirections>> RemoveRedirezione(int Id)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetRedirections)).ConfigureAwait(false);

                var result = await dalSito.RemoveRedirezione(Id).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Redirections>(true, new Redirections() { id = Id });
                }
                else
                {
                    return new Response<Redirections>(false, new Error("Redirezione non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Redirections>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Redirections>> UpdateRedirezione(Redirections redirezione)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetRedirections)).ConfigureAwait(false);

                var result = await dalSito.UpdateRedirezione(Mapper.Map<Redirezioni>(redirezione)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Redirections>(true, new Redirections() { id = redirezione.id.Value });
                }
                else
                {
                    return new Response<Redirections>(false, new Error("Redirezione non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Redirections>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Redirections>> GetRedirezione(int Id)
        {
            List<Redirections> redirections = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetRedirections);//GetActualAsyncMethodName();

                redirections = await HybridCache.GetOrCreateAsync<List<Redirections>>(cacheKey, async result => Mapper.Map<List<Redirections>>(await dalSito.GetRedirezioni().ConfigureAwait(false)));

                if (redirections != null && redirections.Any())
                {
                    var redirection = redirections.FirstOrDefault(r => r.id == Id);

                    if (redirection != null)
                    {
                        if (redirection.active)
                        {
                            return new Response<Redirections>(true, redirection);
                        }
                        else
                        {
                            return new Response<Redirections>(false, new Error("Redirezione non attiva"));
                        }
                    }
                    else
                    {
                        return new Response<Redirections>(false, new Error("Redirezione non trovata"));
                    }
                }
                else
                {
                    return new Response<Redirections>(false, new Error("Nessuna redirezione trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<Redirections>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Redirections>> ToggleRedirezione(Redirections redirezione)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetRedirections)).ConfigureAwait(false);

                var result = await dalSito.AttivaDisattivaRedirezione(Mapper.Map<Redirezioni>(redirezione)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Redirections>(true, new Redirections() { id = redirezione.id });
                }
                else
                {
                    return new Response<Redirections>(false, new Error("Redirezione non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Redirections>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Videos>>> GetVideos()
        {
            List<Videos> videos = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetVideos);//GetActualAsyncMethodName();

                videos = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Videos>>(await dalSito.GetVideo().ConfigureAwait(false)));

                if (videos != null && videos.Any())
                {
                    return new Response<List<Videos>>(true, videos);
                }
                else
                {
                    return new Response<List<Videos>>(false, new Error("Nessun video trovato"));
                }

            }
            catch (Exception ex)
            {
                return new Response<List<Videos>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Videos>> GetVideo(int Id)
        {
            List<Videos> videos = new();
            try
            {
                string cacheKey = nameof(CacheKey.GetVideos);//GetActualAsyncMethodName();

                videos = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Videos>>(await dalSito.GetVideo().ConfigureAwait(false)));

                if (videos != null && videos.Any())
                {
                    Videos? video = videos.FirstOrDefault(v => v.Id == Id);

                    if (video != null)
                    {
                        return new Response<Videos>(true, video);
                    }
                    else
                    {
                        return new Response<Videos>(false, new Error("Video non trovato"));
                    }
                }
                else
                {
                    return new Response<Videos>(false, new Error("Nessun video trovato"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Videos>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Videos>> AddVideo(Videos video)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetVideos)).ConfigureAwait(false);

                await dalSito.AddVideo(Mapper.Map<Video>(video)).ConfigureAwait(false);

                return new Response<Videos>(true, video);

            }
            catch (Exception ex)
            {
                return new Response<Videos>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Videos>> RemoveVideo(int Id)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetVideos)).ConfigureAwait(false);

                var result = await dalSito.RemoveVideo(Id).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Videos>(true, new Videos() { Id = Id });
                }
                else
                {
                    return new Response<Videos>(false, new Error("Video non trovato"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Videos>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Videos>> ToggleVideo(Videos video)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetVideos)).ConfigureAwait(false);

                var result = await dalSito.AttivaDisattivaVideo(Mapper.Map<Video>(video)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Videos>(true, new Videos() { Id = video.Id });
                }
                else
                {
                    return new Response<Videos>(false, new Error("Video non trovato"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Videos>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Notification>> CreateNotification(Notification notification)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetNotification)).ConfigureAwait(false);

                bool result = await dalSito.AddNotifica(Mapper.Map<Notifica>(notification)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Notification>(true, notification);
                }
                else
                {
                    return new Response<Notification>(false, new Error("Notifica non creata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Notification>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<Notification>> UpdateNotification(Notification notification)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetNotification)).ConfigureAwait(false);

                var result = await dalSito.UpdateNotifica(Mapper.Map<Notifica>(notification)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Notification>(true, notification);
                }
                else
                {
                    return new Response<Notification>(false, new Error("Notifica non aggiornata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Notification>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Notification>>> GetNotificheByPagina(int IdPagina)
        {
            List<Notification> notification = new();

            try
            {
                string cacheKey = nameof(CacheKey.GetNotification);

                notification = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Notification>>(await dalSito.GetNotifiche().ConfigureAwait(false)));

                if (notification != null && notification.Any())
                {
                    return new Response<List<Notification>>(true, notification.Where(n => n.Active && n.Page.Equals(IdPagina)).ToList());
                }
                else
                {
                    return new Response<List<Notification>>(false, new Error("Nessuna notifica trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Notification>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Notification>>> GetNotifiche()
        {
            List<Notification> notification = new();

            try
            {
                string cacheKey = nameof(CacheKey.GetNotification);

                notification = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Notification>>(await dalSito.GetNotifiche().ConfigureAwait(false)));

                if (notification != null && notification.Any())
                {
                    return new Response<List<Notification>>(true, notification.ToList());
                }
                else
                {
                    return new Response<List<Notification>>(false, new Error("Nessun video trovato"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Notification>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<TemplateDTO>>> GetTemplates()
        {
            List<TemplateDTO> templates = new();

            try
            {
                string cacheKey = nameof(CacheKey.GetTemplates);

                templates = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<TemplateDTO>>(await dalSito.GetTemplates().ConfigureAwait(false)));

                if (templates != null && templates.Any())
                {
                    return new Response<List<TemplateDTO>>(true, templates.ToList());
                }
                else
                {
                    return new Response<List<TemplateDTO>>(false, new Error("Nessun video trovato"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<TemplateDTO>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<TemplateDTO>> UpdateTemplate(TemplateDTO template)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetTemplates)).ConfigureAwait(false);

                var result = await dalSito.UpdateTemplate(Mapper.Map<Template>(template)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<TemplateDTO>(true, template);
                }
                else
                {
                    return new Response<TemplateDTO>(false, new Error("Notifica non aggiornata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<TemplateDTO>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<TemplateDTO>> CreateTemplate(TemplateDTO template)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetTemplates)).ConfigureAwait(false);

                bool result = await dalSito.AddTemplate(Mapper.Map<Template>(template)).ConfigureAwait(false);

                if (result)
                {
                    return new Response<TemplateDTO>(true, template);
                }
                else
                {
                    return new Response<TemplateDTO>(false, new Error("Notifica non creata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<TemplateDTO>(false, new Error(ex.Message));
            }
        }
    }
}
