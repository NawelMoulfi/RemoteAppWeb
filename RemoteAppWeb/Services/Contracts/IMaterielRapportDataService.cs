using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IMaterielRapportDataService
    {
        Task<IEnumerable<MaterielRapport>> GetMateriels(long RapportId);
        Task AddMaterielRapport(MaterielRapport materielRapport);
    }
}
