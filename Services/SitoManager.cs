using AutoMapper;
using Identity.Interfaces;
using Identity.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Hybrid;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Interfaces;
using SitoDeiSiti.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SitoDeiSiti.Services
{
    public class SitoManager : BaseManager, ISito
    {
        private readonly DalSito dalSito;
        private readonly HybridCache HybridCache;

        public SitoManager(SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache)
            : base(mapper, hybridCache)
        {
            dalSito = new DalSito(context);
            HybridCache = hybridCache;
        }

        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;

        private enum CacheKey
        {
            GetAllImages,
            GetImagesByPage,
            GetPages
        }

        public async Task<Response<List<Images>>> GetImmagini()
        {
            List<Images> immagini = null;
            try
            {
                string cacheKey = nameof(CacheKey.GetAllImages);//GetActualAsyncMethodName();

                immagini = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Images>>(await dalSito.GetImmagini().ConfigureAwait(false)));

                if (immagini != null && immagini.Any())
                {
                    return new Response<List<Images>>(true, immagini);
                }
                else
                {
                    return new Response<List<Images>>(false, new Error("Nessuna immagine trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<List<Images>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Images>>> GetImmaginiByPagina(int Pagina)
        {
            List<Images> immagini = null;
            try
            {
                string cacheKey = nameof(CacheKey.GetImagesByPage);//GetActualAsyncMethodName();

                immagini = await HybridCache.GetOrCreateAsync(cacheKey, async result => Mapper.Map<List<Images>>(await dalSito.GetImmaginiByPagina(Pagina).ConfigureAwait(false)));

                if (immagini != null && immagini.Any())
                {
                    return new Response<List<Images>>(true, immagini);
                }
                else
                {
                    return new Response<List<Images>>(true, immagini);
                }

            }
            catch (Exception ex)
            {
                return new Response<List<Images>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Pages>>> GetPagine()
        {
            List<Pages> pages = null;
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

        public async Task<Response<Images>> AddImmagine(Images immagine)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetImagesByPage)).ConfigureAwait(false);

                await dalSito.AddImmagine(Mapper.Map<Immagini>(immagine)).ConfigureAwait(false);

                return new Response<Images>(true, immagine);

            }
            catch (Exception ex)
            {
                return new Response<Images>(false, new Error(ex.Message));
            }
        }

        public async Task<bool> EmptyCache()
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetImagesByPage)).ConfigureAwait(false);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public async Task<Response<Images>> RemoveImmagine(int Id)
        {
            try
            {
                await HybridCache.RemoveAsync(nameof(CacheKey.GetAllImages)).ConfigureAwait(false);
                await HybridCache.RemoveAsync(nameof(CacheKey.GetImagesByPage)).ConfigureAwait(false);

                var result = await dalSito.RemoveImmagine(Id).ConfigureAwait(false);

                if (result)
                {
                    return new Response<Images>(true, new Images() { Id = Id});
                }
                else
                {
                    return new Response<Images>(false, new Error("Immagine non trovata"));
                }

            }
            catch (Exception ex)
            {
                return new Response<Images>(false, new Error(ex.Message));
            }
        }
    }
}
