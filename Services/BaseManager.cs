using AutoMapper;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SitoDeiSiti.Models.ConfigSettings;
using System.Runtime;

namespace SitoDeiSiti.Backend.Services
{
    public class BaseManager
    {
        protected readonly Token TokenSettings;
        protected readonly IMapper Mapper;
        //protected readonly ICache CacheManager;
        protected readonly HybridCache HybridCache;

        public BaseManager(IMapper mapper, HybridCache hybridCache)
        {
            Mapper = mapper;
            HybridCache = hybridCache;
        }
    }
}
