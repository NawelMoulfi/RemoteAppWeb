using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public ClientRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<Client> AddClient(Client client)
        {
            var addedEntity = await _appDbContext.Clients.AddAsync(client);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteClient(int clientId)
        {
            var foundClient = await _appDbContext.Clients.FirstOrDefaultAsync(e => e.ClientId == clientId);
            if (foundClient == null) return;

            _appDbContext.Clients.Remove(foundClient);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await this._appDbContext.Clients.ToListAsync();
        }

        public async Task<Client> GetClientById(int ClientId)
        {
            return await _appDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId== ClientId);
        }

        public async Task<Client> UpdateClient(Client client)
        {
            var foundClient = await _appDbContext.Clients.FirstOrDefaultAsync(e => e.ClientId == client.ClientId);

            if (foundClient != null)
            {
                foundClient.FirstName = client.FirstName;
                foundClient.LastName = client.LastName;
                foundClient.PhoneNumber = client.PhoneNumber;
                foundClient.Adresse = client.Adresse;
                foundClient.PID = client.PID;
                foundClient.Wilaya= client.Wilaya;
               

                await _appDbContext.SaveChangesAsync();

                return foundClient;
            }

            return null;
        }
    }
}
