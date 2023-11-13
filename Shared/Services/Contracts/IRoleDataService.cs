

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IRoleDataService
    {
        Task<IEnumerable<RoleDto>> GetRolesList();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<RoleDto> AddRole(RoleDto role);
        Task UpdateRole(RoleDto role);
        Task DeleteRole(int roleId);
        Task<RoleDto> GetRole(int RoleId);

    }
}
