using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int UserId);
        Task<UserDto> AddUser(UserDto user);
        // Task<Event> AddEvent(Event event);
        Task<UserDto> UpdateUser(UserDto user);
        Task DeleteUser(int userId);
        Task SaveNewPassword(int userId , string UserPassword);
        Task<UserDto> FindUserByCredentials(string username, string password);
        Task<UserDto> FindUserByUID(string username);

    }
}
