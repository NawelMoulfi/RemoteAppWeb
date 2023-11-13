using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
        Task<RoleDto> GetRoleById(int RoleId);
        Task<RoleDto> AddRole(RoleDto role);
        // Task<Event> AddEvent(Event event);
        Task<RoleDto> UpdateRole(RoleDto role);
        Task DeleteRole(int roleId);
    }
}
