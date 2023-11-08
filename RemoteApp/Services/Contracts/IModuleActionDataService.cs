using RemoteApp.Data.Models;
using RemoteAppApi.Controllers;

namespace RemoteApp.Services.Contracts
{
    public interface IModuleActionDataService
    {
        Task<IEnumerable<ModuleAction>> GetModuleActions();
        Task<ModuleAction> GetModuleActionById(int ModuleActionId);
        Task<ModuleAction> AddModuleAction(ModuleAction moduleaction);
        Task UpdateModuleAction(ModuleAction moduleaction);
        Task DeleteModuleAction(int moduleactionId);
        Task<IEnumerable<ModuleAction>> GetResourceAction(ResourceActionParameters parameters);
        Task<IEnumerable<ModuleAction>> GetListModuleActionsByResource(Resource resource);
        Task<IEnumerable<RemoteApp.Data.Models.Action>> GetActionsByResource(Resource resource);
        Task<IEnumerable<ModuleAction>> GetActionsByResourceExcludingFirst();
        Task<IEnumerable<Resource>> GetUsedResources();
    }
}
