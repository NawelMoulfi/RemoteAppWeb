using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleActionRepository
    {
        Task<IEnumerable<ModuleAction>> GetAllModulActions();
        Task<ModuleAction> GetModuleActionAById(int ModuleActionId);
        Task<ModuleAction> AddModuleAction(ModuleAction moduleaction);
        Task<ModuleAction> UpdateModuleAction(ModuleAction moduleaction);
        Task DeleteModuleAction(int moduleactionId);
        Task<IEnumerable<ModuleAction>> GetResourceAction(Resource resource, RemoteApp.Data.Models.Action action);
        Task<IEnumerable<ModuleAction>> GetListModuleActionsByResource(Resource resource);
        Task<IEnumerable<RemoteApp.Data.Models.Action>> GetActionsByResource(Resource resource);
       Task< IEnumerable<ModuleAction>> GetActionsByResourceExcludingFirst();
        Task<IEnumerable<Resource>> GetUsedResources();
    }
}
