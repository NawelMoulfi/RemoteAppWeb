

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IUserDataService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<UserDto> AddUser(UserDto user);
        Task UpdateUser(UserDto user);
        Task DeleteUser(int userId);
        Task<UserDto> GetUser(int userId);
        Task SaveNewPassword(int uId, string password);
        Task<UserDto> Login(string username, string password);

    }
}
