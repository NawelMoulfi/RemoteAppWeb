using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int UserId);
        Task<User> AddUser(User user);
        // Task<Event> AddEvent(Event event);
        Task<User> UpdateUser(User user);
        Task DeleteUser(int userId);
        Task SaveNewPassword(int userId , string UserPassword);
        Task<User> FindUserByCredentials(string username, string password);
        Task<User> FindUserByUID(string username);

    }
}
