

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IModuleDataService
    {
        Task<IEnumerable<ModuleDto>> GetModules();
        Task<ModuleDto> GetModuleById(int ModuleId);// Module GetModuleById(long id)
        Task<ModuleDto> Add(ModuleDto module);//Module Add(Module module)
        // Task<Event> AddEvent(Event event);
        Task  UpdateModule(ModuleDto module);
        Task  DeleteModule(int id);
    }
}
