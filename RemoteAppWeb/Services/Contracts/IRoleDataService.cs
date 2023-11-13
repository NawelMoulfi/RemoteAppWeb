using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IRoleDataService
    {
        Task<IEnumerable<Role>> GetRolesList();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<Role> AddRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(int roleId);
        Task<Role> GetRole(int RoleId);

    }
}
