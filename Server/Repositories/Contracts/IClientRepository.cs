using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<ClientDto>> GetAllClients();
        Task<ClientDto> GetClientById(int ClientId);
        Task<ClientDto> AddClient(ClientDto client);
        // Task<Event> AddEvent(Event event);
        Task<ClientDto> UpdateClient(ClientDto client);
        Task DeleteClient(int clientId);
    }
}
