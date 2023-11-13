

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IRapportDataService
    {
        Task<IEnumerable<RapportInterventionDto>> GetAllRepports() ;
        Task AddRapport(RapportInterventionDto rapportIntervention);
        Task UpdateRapport(RapportInterventionDto rapportIntervention);
        Task DeleteRapport(RapportInterventionDto rapportIntervention);
    }
}
