using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public ClientRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<ClientDto> AddClient(ClientDto client)
        {
            var Client = mapper.Map<ClientDto, Client>(client);
            var addedEntity = await _appDbContext.Clients.AddAsync(Client);
            await _appDbContext.SaveChangesAsync();
            var ClientDTO = mapper.Map<Client, ClientDto>(addedEntity.Entity);
            return ClientDTO;
        }

        public async Task DeleteClient(int clientId)
        {
            var foundClient = await _appDbContext.Clients.FirstOrDefaultAsync(e => e.ClientId == clientId);
            if (foundClient == null) return;

            _appDbContext.Clients.Remove(foundClient);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClientDto>> GetAllClients()
        {
            var Clients = new List<Client>();
            Clients = await this._appDbContext.Clients.ToListAsync();
            List<ClientDto> ClientDtos = new List<ClientDto>();
            foreach (var client in Clients)
            {
                ClientDtos.Add(mapper.Map<Client, ClientDto>(client));
            }
            return ClientDtos;
        }

        public async Task<ClientDto> GetClientById(int ClientId)
        {
            var Client = await _appDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == ClientId);
            var ClientDTO = mapper.Map<Client, ClientDto>(Client);
            return ClientDTO;

        }

        public async Task<ClientDto> UpdateClient(ClientDto client)
        {
            var Client = mapper.Map<ClientDto, Client>(client);
            var foundClient = await _appDbContext.Clients.FirstOrDefaultAsync(e => e.ClientId == Client.ClientId);

            if (foundClient != null)
            {
                foundClient.FirstName = client.FirstName;
                foundClient.LastName = client.LastName;
                foundClient.PhoneNumber = client.PhoneNumber;
                foundClient.Adresse = client.Adresse;
                foundClient.PID = client.PID;
                foundClient.Wilaya= client.Wilaya;
               

                await _appDbContext.SaveChangesAsync();
                var ClientDTO = mapper.Map<Client, ClientDto>(foundClient);
                return ClientDTO;
               /// return ;
            }

            return null;
        }
    }
}
