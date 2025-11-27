using DAL.Enums;
using SitoDeiSiti.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitoDeiSiti.DAL.Interface
{
    public interface IDalUtente
    {
        public Task<bool> GetUtenteByMail(string mail);
        public Task<bool> CreateUtente(Utente utente, UtenteInfo utenteInfo, UtentePrivacy utentePrivacy, UtenteAtleta utenteAtleta);
        public Task<Utente?> CheckUtenteUserAndPassword(string User, string Password);
        public Task<List<Utente?>> GetUtenti();
        public Task<Utente?> GetUtente(Guid Rowguid);
        public Task<int> UpdateUser(UserDbOperationEnum operation, Utente utente, UtenteInfo utenteInfo, UtentePrivacy utentePrivacy, UtenteAtleta utenteAtleta);
        public Task<List<Cinture>> GetCinture();
        public Task<List<Organizzazioni>> GetOrganizzazioni();
        public Task<List<Utente>> GetUtentiOrganizzazioni(Guid Org);

    }
}
