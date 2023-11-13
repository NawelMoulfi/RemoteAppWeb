

using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class ClientDataService : IClientDataService
    {
        private readonly HttpClient _httpClient;

        public ClientDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ClientDto> AddClient(ClientDto client)
        {
            var clientJson =
              new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/Client", clientJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ClientDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async  Task DeleteClient(int clientId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7023/api/Client/{clientId}");
        }

        public async Task<IEnumerable<ClientDto>> GetAllClients()
        {
            Console.WriteLine($"********We are trying to retrieve all the clients ****************");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7023/api/Client");

                //var response = await httpClient.GetAsync("api/Client");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Clients = await JsonSerializer.DeserializeAsync<List<ClientDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }

                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<ClientDto>)Clients;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<ClientDto>();
                }

            }
        }

    

    public async Task UpdateClient(ClientDto client)
        {
            var clientJson =
               new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("https://localhost:7023/api/Client", clientJson);
        }
    }
}
