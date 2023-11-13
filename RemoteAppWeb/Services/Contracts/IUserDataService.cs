using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IUserDataService
    {
        Task<IEnumerable<User>> GetAllUsers();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<User> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int userId);
        Task<User> GetUser(int userId);
        Task SaveNewPassword(int uId, string password);
        Task<User> Login(string username, string password);

    }
}
