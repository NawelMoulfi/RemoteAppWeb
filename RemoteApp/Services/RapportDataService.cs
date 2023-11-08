using RemoteApp.Data.Models;
using RemoteApp.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace RemoteApp.Services
{
    public class RapportDataService : IRapportDataService
    {
        private readonly HttpClient _httpClient;

        public RapportDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // _httpClient.BaseAddress = new Uri("https://localhost:7023/");
        }
        public  async Task AddRapport(RapportIntervention rapportIntervention)
        {
            Console.WriteLine($"The new rapportIntervention in the Dataservice : {rapportIntervention}");
            Console.WriteLine($"The new rapportIntervention Id in the Dataservice: {rapportIntervention.RapportId}");
            Console.WriteLine($"The new rapportIntervention UserId in the DataService : {rapportIntervention.CreatedByUserId}");
            var rapportInterventionJson =
              new StringContent(JsonSerializer.Serialize(rapportIntervention), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/RapportIntervention", rapportInterventionJson);
            Console.WriteLine($"Responcse code for rapportIntervention : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent for rapportIntervention : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  rapportIntervention: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response body  from the server of the New  rapportIntervention: {await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync())}");
                //return await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync());
            }

            ///return null;
        }

        public async Task DeleteRapport(RapportIntervention rapportIntervention)
        {
            await _httpClient.DeleteAsync($"https://localhost:7023/api/RapportIntervention/{rapportIntervention.RapportId}");
        }

        public async Task<IEnumerable<RapportIntervention>> GetAllRepports()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7023/api/RapportIntervention");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code RapportIntervention : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent  RapportIntervention: {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var RapportInterventions = await JsonSerializer.DeserializeAsync<List<RapportIntervention>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<RapportIntervention>)RapportInterventions;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<RapportIntervention>();
                }
            }
        }
      
        public  async Task UpdateRapport(RapportIntervention rapportIntervention)
        {
            Console.WriteLine($"The update rapportIntervention  in the service : {rapportIntervention}");
            Console.WriteLine($"The updated rapportIntervention Id in the service : {rapportIntervention.RapportId}");
            Console.WriteLine($"The updated rapportIntervention User Id in the service : {rapportIntervention.CreatedByUserId}");
            //Console.WriteLine($"The updated user Role Id  in the service : {user.RoleId}");
            var rapportInterventionJson =
                   new StringContent(JsonSerializer.Serialize(rapportIntervention), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:7023/api/RapportIntervention", rapportInterventionJson);
            Console.WriteLine($"Responcse code of update RapportIntervention: {response.StatusCode}");
            Console.WriteLine($"Responcse contenent of update RapportIntervention : {response.ReasonPhrase}"); // Your API logic
            Console.WriteLine($"Responcse  of update RapportIntervention : {response}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of update RapportIntervention: {responseBody}");
        }
    }
}
