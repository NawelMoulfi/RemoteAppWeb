using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IMaterielRapportRepository
    {
        Task<IEnumerable<MaterielRapportDto>> GetMateriels(long RapportId);
        Task<MaterielRapportDto> AddMaterielRapport(MaterielRapportDto materielRapport);
    }
}
