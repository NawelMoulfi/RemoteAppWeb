using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using RemoteApp.Data.Models;
using RemoteApp.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace RemoteApp.Services
{
    public class RoleDataService : IRoleDataService
    {
        private readonly HttpClient _httpClient;

        public RoleDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public  async Task<Role> AddRole(Role role)
        {
            var entryJson =
             new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/Role", entryJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Role>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }
        public async Task<Role> GetRole(int RoleId)
        {
            return await JsonSerializer.DeserializeAsync<Role>
             (await _httpClient.GetStreamAsync($"https://localhost:7023/api/Role/{RoleId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public  async Task DeleteRole(int roleId)
        {

            var response  =  await _httpClient.DeleteAsync($"https://localhost:7023/api/Role/{roleId}");
            Console.WriteLine($"Responcse code for the deleted Role : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent for the deleted Role : {response.ReasonPhrase}");
            Console.WriteLine($"Responcse  stream  for the deleted Role : {await response.Content.ReadAsStreamAsync()}");
        }

        public async Task<IEnumerable<Role>> GetRolesList()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7023/api/Role");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    // Deserialize the response and perform any other related actions
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Roles = await JsonSerializer.DeserializeAsync<List<Role>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<Role>)Roles;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<Role>();
                }

            }
        }

        public async Task UpdateRole(Role role)
        {
            var roleJson =
             new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");


            await _httpClient.PutAsync("https://localhost:7023/api/Role", roleJson);
        }
    }
}
