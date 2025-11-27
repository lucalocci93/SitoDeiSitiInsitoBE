using SitoDeiSiti.DAL.Models;

namespace SitoDeiSiti.DAL.Interface
{
    public interface IDalEventi
    {
        //Evento
        public Task<List<Evento>> GetEventi();
        public Task<Evento> GetEvent(Guid Id);
        public Task<bool> CreateEvento(Evento evento);
        public Task<bool> UpdateEvento(Evento evento);

        //Iscrizioni
        public Task<IscrizioneEvento?> CheckIscrizione(IscrizioneEvento iscrizioneEvento);
        public Task<bool> IscrizioneEvento(IscrizioneEvento IscrizioneEvento);
        public Task<bool> UpdateIscrizioneEvento(IscrizioneEvento IscrizioneEvento);
        public Task<List<IscrizioneEvento>> GetIscrizioni(Guid UserId);
        public Task<List<IscrizioneEvento>> GetIscrizioniByEvento(Guid EventId);
        public Task<List<IscrizioneEvento>> GetIscrizioniByEventoEUtente(Guid EventId, Guid UserId);
        public Task<List<IscrizioneEvento>> GetIscrizioniByEventoEOrg(Guid EventId, Guid Org);



        //Gare
        public Task<List<Gare>> GetGareByIdEvento(Guid EventId);
        public Task<Gare> GetGareById(Guid IdGara);
        public Task<bool> AddGara(Gare gara);
        public Task<bool> DeleteGara(Guid Id);


        public Task<List<Categoria>> GetCategorie();
    }
}
