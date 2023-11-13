
using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IClientDataService
    {
        Task<IEnumerable<ClientDto>> GetAllClients();
       // Task<Client> GetEmployeeDetails(int employeeId);
        Task<ClientDto> AddClient(ClientDto client);
        Task UpdateClient(ClientDto client);
        Task DeleteClient(int clientId);
    }
}
