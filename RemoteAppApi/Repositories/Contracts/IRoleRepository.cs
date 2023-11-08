using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRoleById(int RoleId);
        Task<Role> AddRole(Role role);
        // Task<Event> AddEvent(Event event);
        Task<Role> UpdateRole(Role role);
        Task DeleteRole(int roleId);
    }
}
