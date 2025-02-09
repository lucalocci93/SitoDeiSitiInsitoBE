using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SitoDeiSiti.Models.ConfigSettings;
using System.Runtime;

namespace Identity.Services
{
    public class BaseManager
    {
        protected readonly Token TokenSettings;
        protected readonly IMapper Mapper;
        protected readonly CacheManager CacheManager;

        public BaseManager(IOptions<Token> options, IMapper mapper, CacheManager cacheManager)
        {
            TokenSettings = options.Value;
            Mapper = mapper;
            this.CacheManager = cacheManager;
        }
    }
}
