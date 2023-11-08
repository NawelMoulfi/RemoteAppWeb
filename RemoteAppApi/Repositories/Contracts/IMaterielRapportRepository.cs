using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IMaterielRapportRepository
    {
        Task<IEnumerable<MaterielRapport>> GetMateriels(long RapportId);
        Task<MaterielRapport> AddMaterielRapport(MaterielRapport materielRapport);
    }
}
