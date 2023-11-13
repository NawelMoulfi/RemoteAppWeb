
using RemoteAppApi.Controllers;
using Shared.Dto;
using Action = Shared.Dto.Action;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IModuleActionDataService
    {
        Task<IEnumerable<ModuleActionDto>> GetModuleActions();
        Task<ModuleActionDto> GetModuleActionById(int ModuleActionId);
        Task<ModuleActionDto> AddModuleAction(ModuleActionDto moduleaction);
        Task UpdateModuleAction(ModuleActionDto moduleaction);
        Task DeleteModuleAction(int moduleactionId);
        Task<IEnumerable<ModuleActionDto>> GetResourceAction(ResourceActionParameters parameters);
        Task<IEnumerable<ModuleActionDto>> GetListModuleActionsByResource(Resource resource);
        Task<IEnumerable<Action>> GetActionsByResource(Resource resource);
        Task<IEnumerable<ModuleActionDto>> GetActionsByResourceExcludingFirst();
        Task<IEnumerable<Resource>> GetUsedResources();
    }
}
