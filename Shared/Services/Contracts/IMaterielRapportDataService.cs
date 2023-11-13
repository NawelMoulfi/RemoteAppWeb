

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IMaterielRapportDataService
    {
        Task<IEnumerable<MaterielRapportDto>> GetMateriels(long RapportId);
        Task AddMaterielRapport(MaterielRapportDto materielRapport);
    }
}
