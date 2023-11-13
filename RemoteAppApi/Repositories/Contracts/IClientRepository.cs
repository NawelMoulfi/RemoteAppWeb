using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClients();
        Task<Client> GetClientById(int ClientId);
        Task<Client> AddClient(Client client);
        // Task<Event> AddEvent(Event event);
        Task<Client> UpdateClient(Client client);
        Task DeleteClient(int clientId);
    }
}
