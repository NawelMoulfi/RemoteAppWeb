

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IModuleActionRoleDataService
    {
       
        Task<ModuleActionRoleDto> AddModuleActionRole(ModuleActionRoleDto moduleActionRole);
        Task<ModuleActionRoleDto> GetModuleActionRoleByRoleAndModuleActionId(int RoleId, int ModuleActionId);
        Task DeleteModuleActionRole(int ModuleActionRoleId);
    }
}
