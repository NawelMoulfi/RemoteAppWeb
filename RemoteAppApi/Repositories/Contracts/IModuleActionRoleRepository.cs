using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IModuleActionRoleRepository
    {
        Task<IEnumerable<ModuleActionRole>> GetAllModulActionRoles();
        Task<ModuleActionRole> GetModuleActionRoleById(int ModuleActionRoleId);
        Task<ModuleActionRole> GetModuleActionRoleByRoleAndModuleActionId(int RoleId ,int ModuleActionId);
        Task<ModuleActionRole> AddModuleActionRole(ModuleActionRole moduleactionrole);
        // Task<Event> AddEvent(Event event);
        Task<ModuleActionRole> UpdateModuleActionRole(ModuleActionRole moduleactionrole);
        Task DeleteModuleActionRole(int moduleactionroleId);
    }
}
