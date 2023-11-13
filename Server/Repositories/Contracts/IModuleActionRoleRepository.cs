using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleActionRoleRepository
    {
        Task<IEnumerable<ModuleActionRoleDto>> GetAllModulActionRoles();
        Task<ModuleActionRoleDto> GetModuleActionRoleById(int ModuleActionRoleId);
        Task<ModuleActionRoleDto> GetModuleActionRoleByRoleAndModuleActionId(int RoleId ,int ModuleActionId);
        Task<ModuleActionRoleDto> AddModuleActionRole(ModuleActionRoleDto moduleactionrole);
        // Task<Event> AddEvent(Event event);
        Task<ModuleActionRoleDto> UpdateModuleActionRole(ModuleActionRoleDto moduleactionrole);
        Task DeleteModuleActionRole(int moduleactionroleId);
    }
}
