using RemoteApp.Data.Models;

namespace RemoteApp.Services.Contracts
{
    public interface IModuleDataService
    {
        Task<IEnumerable<Module>> GetModules();
        Task<Module> GetModuleById(int ModuleId);// Module GetModuleById(long id)
        Task<Module> Add(Module module);//Module Add(Module module)
        // Task<Event> AddEvent(Event event);
        Task  UpdateModule(Module module);
        Task  DeleteModule(int id);
    }
}
