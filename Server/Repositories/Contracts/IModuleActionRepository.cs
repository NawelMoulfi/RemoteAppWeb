using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleActionRepository
    {
        Task<IEnumerable<ModuleActionDto>> GetAllModulActions();
        Task<ModuleActionDto> GetModuleActionAById(int ModuleActionId);
        Task<ModuleActionDto> AddModuleAction(ModuleActionDto moduleaction);
        Task<ModuleActionDto> UpdateModuleAction(ModuleActionDto moduleaction);
        Task DeleteModuleAction(int moduleactionId);
        Task<IEnumerable<ModuleActionDto>> GetResourceAction(Resource resource, Shared.Dto.Action action);
        Task<IEnumerable<ModuleActionDto>> GetListModuleActionsByResource(Resource resource);
        Task<IEnumerable<Shared.Dto.Action>> GetActionsByResource(Resource resource);
       Task< IEnumerable<ModuleActionDto>> GetActionsByResourceExcludingFirst();
        Task<IEnumerable<Resource>> GetUsedResources();
    }
}
