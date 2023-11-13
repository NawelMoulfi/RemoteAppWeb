using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IRapportInterventionRepository
    {
        Task<IEnumerable<RapportInterventionDto>> GetAllRapportInterventions();
        Task<RapportInterventionDto> GetRapportInterventionrById(long RapportInterventionId);
        Task<RapportInterventionDto> AddRapportIntervention(RapportInterventionDto RapportIntervention);
        // Task<Event> AddEvent(Event event);
        Task<RapportInterventionDto> UpdateRapportIntervention(RapportInterventionDto RapportIntervention);
        Task DeleteRapportIntervention(long RapportInterventionId);
    }
}
