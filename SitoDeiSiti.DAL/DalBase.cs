using SitoDeiSiti.DAL.Models;

namespace SitoDeiSiti.DAL
{
    public class DalBase
    {
        protected readonly SitoDeiSitiInsitoContext Db;

        public DalBase(SitoDeiSitiInsitoContext context)
        {
            Db = context;
        }
    }
}
