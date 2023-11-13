using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModuls();
        Task<Module> GetModuleById(int ModuleId);
        Task<Module> AddModule(Module module);
        // Task<Event> AddEvent(Event event);
        Task<Module> UpdateModule(Module module);
        Task DeleteModule(int moduleId);
    }
}
