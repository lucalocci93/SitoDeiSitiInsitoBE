using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SitoDeiSiti.DAL.Enums;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL
{
    public class DalSito: DalBase, IDalSito
    {
        public DalSito(SitoDeiSitiInsitoContext sitoDeiSitiInsitoContext) :
            base(sitoDeiSitiInsitoContext)
        {

        }

        public async Task<Grafiche> AddGrafica(Grafiche immagine)
        {
            try
            {
                Db.Grafiche.Add(immagine);
                await Db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
            return immagine;
        }

        public async Task<List<Grafiche>> GetGrafiche()
        {
            List<Grafiche> immagini = new();
            try
            {
                immagini = await Db.Grafiche
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

            return immagini;
        }

        public Task<Grafiche> GetGrafica(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Grafiche>> GetGraficheByPagina(int Pagina)
        {
            List<Grafiche> immagini = new();
            try
            {
                immagini = await Db.Grafiche
                    .AsNoTracking()
                    .Where(i => i.Pagina == Pagina)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

            return immagini;
        }

        public async Task<List<Models.Pagine>> GetPagine()
        {
            List<Models.Pagine> pagine = new();
            try
            {
                pagine = await Db.Pagine
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

            return pagine;
        }

        public async Task<bool> RemoveGrafica(int id)
        {
            try
            {
                var delete = await Db.Grafiche.Where(i => i.Id == id).ExecuteDeleteAsync();

                if(delete > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AttivaDisattivaGrafica(Grafiche grafica)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Grafiche
                    .Where(r => r.Id.Equals(grafica.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Attiva, grafica.Attiva));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Redirezioni>> GetRedirezioni()
        {
            List<Redirezioni> redirezioni = new();
            try
            {
                redirezioni = await Db.Redirezioni
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
            return redirezioni;
        }

        public async Task<Redirezioni> AddRedirezione(Redirezioni redirezione)
        {
            try
            {
                Db.Redirezioni.Add(redirezione);
                await Db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

            return redirezione;
        }

        public async Task<bool> RemoveRedirezione(int id)
        {
            try
            {
                var delete = await Db.Redirezioni.Where(i => i.Id == id).ExecuteDeleteAsync();

                if (delete > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRedirezione(Redirezioni redirezione)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Redirezioni
                    .Where(r => r.Id.Equals(redirezione.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Url, redirezione.Url));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Redirezioni> GetRedirezione(int Id)
        {
            try
            {
                Redirezioni? redirezioni = await Db.Redirezioni
                    .FirstOrDefaultAsync(r => r.Id.Equals(Id))
                    .ConfigureAwait(false);

                if (redirezioni != null)
                {
                    return redirezioni;
                }

                return new() { Id = -1 };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AttivaDisattivaRedirezione(Redirezioni redirezione)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Redirezioni
                    .Where(r => r.Id.Equals(redirezione.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Attiva, redirezione.Attiva));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Video>> GetVideo()
        {
            List<Video> video = new();
            try
            {
                video = await Db.Video
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
            return video;
        }

        public async Task<Video> GetVideo(int Id)
        {
            Video? video = new();
            try
            {
                video = await Db.Video
                    .FirstOrDefaultAsync(i => i.Id.Equals(Id))
                    .ConfigureAwait(false);

                if (video != null)
                {
                    return video;
                }
                else
                {
                    return new() { Id = -1 };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Video> AddVideo(Video video)
        {
            try
            {
                Db.Video.Add(video);
                await Db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

            return video;
        }

        public async Task<bool> RemoveVideo(int Id)
        {
            try
            {
                var delete = await Db.Video.Where(i => i.Id == Id).ExecuteDeleteAsync();

                if (delete > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateVideo(Video video)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Video
                    .Where(r => r.Id.Equals(video.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Url, video.Url));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AttivaDisattivaVideo(Video video)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Video
                    .Where(r => r.Id.Equals(video.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Attivo, video.Attivo));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Template> GetTemplateByName(string TemplateName)
        {
            Template? template = new();
            try
            {
                template = await Db.Template
                    .FindAsync(TemplateName)
                    .ConfigureAwait(false);

                return template ?? new Template { TemplateName = string.Empty };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddNotifica(Notifica notifica)
        {
            int addedRows = 0;

            try
            {
                Db.Notifica.Add(notifica);
                addedRows = await Db.SaveChangesAsync().ConfigureAwait(false);

                if(addedRows > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public async Task<bool> UpdateNotifica(Notifica notifica)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Notifica
                    .Where(r => r.Id.Equals(notifica.Id))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.Pagina, notifica.Pagina)
                        .SetProperty(r => r.Testo, notifica.Testo)
                        .SetProperty(r => r.Abilitata, notifica.Abilitata));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Notifica>> GetNotificheByPagina(int IdPagina)
        {
            List<Notifica>? notifiche = new();
            try
            {
                notifiche = await Db.Notifica
                    .Where(n => n.Pagina == IdPagina)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return notifiche;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Notifica>> GetNotifiche()
        {
            List<Notifica>? notifiche = new();
            try
            {
                notifiche = await Db.Notifica
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return notifiche;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Template>> GetTemplates()
        {
            List<Template>? templates = new();
            try
            {
                templates = await Db.Template
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return templates;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTemplate(Template template)
        {
            int updatedRows = 0;
            try
            {
                updatedRows = await Db.Template
                    .Where(r => r.TemplateName.Equals(template.TemplateName))
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(r => r.TemplateHeaderHtml, template.TemplateHeaderHtml)
                        .SetProperty(r => r.TemplateBodyHtml, template.TemplateBodyHtml)
                        .SetProperty(r => r.TemplateFooterHtml, template.TemplateFooterHtml));

                if (updatedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddTemplate(Template template)
        {
            int addedRows = 0;

            try
            {
                Db.Template.Add(template);
                addedRows = await Db.SaveChangesAsync().ConfigureAwait(false);

                if (addedRows > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }
    }
}
