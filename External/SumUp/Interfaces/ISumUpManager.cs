using SitoDeiSiti.Backend.External.SumUp.Models.SumUp;
using SitoDeiSiti.Utils.HTTPHandlers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.External.SumUp.Interfaces
{
    public interface ISumUpManager
    {
        Task<HostedCheckoutOutput> CreateHostedCheckout(HostedCheckoutInput input);
        Task<List<SumUpListCheckoutOutput>> GetSumUpCheckoutList();
    }
}
