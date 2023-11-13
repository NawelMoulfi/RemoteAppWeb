using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IModuleActionRoleDataService
    {
       
        Task<ModuleActionRole> AddModuleActionRole(ModuleActionRole moduleActionRole);
        Task<ModuleActionRole> GetModuleActionRoleByRoleAndModuleActionId(int RoleId, int ModuleActionId);
        Task DeleteModuleActionRole(int ModuleActionRoleId);
    }
}
