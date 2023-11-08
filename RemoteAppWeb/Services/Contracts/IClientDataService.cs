using RemoteApp.Data.Models;
namespace RemoteAppWeb.Services.Contracts
{
    public interface IClientDataService
    {
        Task<IEnumerable<Client>> GetAllClients();
       // Task<Client> GetEmployeeDetails(int employeeId);
        Task<Client> AddClient(Client client);
        Task UpdateClient(Client client);
        Task DeleteClient(int clientId);
    }
}
