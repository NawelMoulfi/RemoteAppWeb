using RemoteApp.Data.Models;

namespace RemoteApp.Services.Contracts
{
    public interface IMaterielRapportDataService
    {
        Task<IEnumerable<MaterielRapport>> GetMateriels(long RapportId);
        Task AddMaterielRapport(MaterielRapport materielRapport);
    }
}
