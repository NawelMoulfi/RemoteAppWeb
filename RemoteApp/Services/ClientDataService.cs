

using RemoteApp.Services.Contracts;
using RemoteApp.Data.Models;
using System.Text;
using System.Text.Json;

namespace RemoteApp.Services
{
    public class ClientDataService : IClientDataService
    {
        private readonly HttpClient _httpClient;

        public ClientDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Client> AddClient(Client client)
        {
            var clientJson =
              new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/Client", clientJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Client>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async  Task DeleteClient(int clientId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7023/api/Client/{clientId}");
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7023/api/Client");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Clients = await JsonSerializer.DeserializeAsync<List<Client>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }

                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<Client>)Clients;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<Client>();
                }

            }
        }

    

    public async Task UpdateClient(Client client)
        {
            var clientJson =
               new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("https://localhost:7023/api/Client", clientJson);
        }
    }
}
