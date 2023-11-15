

using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class RoleDataService : IRoleDataService
    {
        private readonly HttpClient _httpClient;

        public RoleDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public  async Task<RoleDto> AddRole(RoleDto role)
        {
            var entryJson =
             new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/Role", entryJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<RoleDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }
        public async Task<RoleDto> GetRole(int RoleId)
        {
            return await JsonSerializer.DeserializeAsync<RoleDto>
             (await _httpClient.GetStreamAsync($"https://localhost:7149/api/Role/{RoleId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public  async Task DeleteRole(int roleId)
        {

            var response  =  await _httpClient.DeleteAsync($"https://localhost:7149/api/Role/{roleId}");
            Console.WriteLine($"Responcse code for the deleted Role : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent for the deleted Role : {response.ReasonPhrase}");
            Console.WriteLine($"Responcse  stream  for the deleted Role : {await response.Content.ReadAsStreamAsync()}");
        }

        public async Task<IEnumerable<RoleDto>> GetRolesList()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7149/api/Role");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Responcse code Get Role >>>>>>>>>>>>>> : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent Get Role : {response.ReasonPhrase}");
                    // Deserialize the response and perform any other related actions
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Roles = await JsonSerializer.DeserializeAsync<List<RoleDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<RoleDto>)Roles;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<RoleDto>();
                }

            }
        }

        public async Task UpdateRole(RoleDto role)
        {
            var roleJson =
             new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");


            await _httpClient.PutAsync("https://localhost:7149/api/Role", roleJson);
        }
    }
}
