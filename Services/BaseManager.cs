using AutoMapper;
using Identity.Models;
using Identity.Models.ConfigSettings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
