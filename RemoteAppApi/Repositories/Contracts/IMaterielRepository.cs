using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IMaterielRepository
    {
        Task<IEnumerable<Materiel>> GetAllMateriels();
        Task<Materiel> GetMaterielById(long MaterielId);
        Task<Materiel> AddMateriel(Materiel materiel);
        // Task<Event> AddEvent(Event event);
        Task<Materiel> UpdateMateriel(Materiel materiel);
        Task DeleteMateriel(long materielId);
        // Task<IEnumerable<MaterielRapport>> GetMateriels(long RapportId);
    }
}
