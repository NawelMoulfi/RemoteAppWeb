using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IRapportInterventionRepository
    {
        Task<IEnumerable<RapportIntervention>> GetAllRapportInterventions();
        Task<RapportIntervention> GetRapportInterventionrById(long RapportInterventionId);
        Task<RapportIntervention> AddRapportIntervention(RapportIntervention RapportIntervention);
        // Task<Event> AddEvent(Event event);
        Task<RapportIntervention> UpdateRapportIntervention(RapportIntervention RapportIntervention);
        Task DeleteRapportIntervention(long RapportInterventionId);
    }
}
