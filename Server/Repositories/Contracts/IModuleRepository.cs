using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleRepository
    {
        Task<IEnumerable<ModuleDto>> GetAllModuls();
        Task<ModuleDto> GetModuleById(int ModuleId);
        Task<ModuleDto> AddModule(ModuleDto module);
        // Task<Event> AddEvent(Event event);
        Task<ModuleDto> UpdateModule(ModuleDto module);
        Task DeleteModule(int moduleId);
    }
}
