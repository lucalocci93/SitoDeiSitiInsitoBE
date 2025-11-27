using AutoMapper;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using OfficeOpenXml.Drawing;
using SitoDeiSiti.Backend.Interfaces;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.DTOs.ConfigSettings;
using System.Threading.Channels;

namespace SitoDeiSiti.Backend.Services
{
    public class EventServiceManager : BaseManager, IEventServicemanager
    {
        public Channel<bool> EventChannel;

        public EventServiceManager(IMapper mapper, HybridCache hybridCache)
            : base(mapper, hybridCache)
        {
            EventChannel = Channel.CreateUnbounded<bool>();
        }

        public bool AddEvent<T>(T eventType)
        {
            throw new NotImplementedException();
        }

        public bool ReadEvent()
        {
            throw new NotImplementedException();
        }
    }
}
