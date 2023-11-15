
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class MaterielRapportDataService : IMaterielRapportDataService
    {
        private readonly HttpClient _httpClient;

        public MaterielRapportDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // _httpClient.BaseAddress = new Uri("https://localhost:7023/");
        }
        public async  Task AddMaterielRapport(MaterielRapportDto materielRapport)
        {
            Console.WriteLine($"The new  materielRapport in the Dataservice : {materielRapport}");
            Console.WriteLine($"The new materielRapport Id in the Dataservice: {materielRapport.MaterielRapportId}");
            Console.WriteLine($"The new materielRapport RapportIntervention Id in the DataService : {materielRapport.RapportInterventionId}");
            var materielRapportJson =
              new StringContent(JsonSerializer.Serialize(materielRapport), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/MaterielRapport", materielRapportJson);
            Console.WriteLine($"Responcse code materielRapport: {response.StatusCode}");
            Console.WriteLine($"Responcse contenent materielRapport : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  materielRapport: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response body from the server of the New  materielRapport: {await JsonSerializer.DeserializeAsync<MaterielRapportDto>(await response.Content.ReadAsStreamAsync())}");
               
            }

           
        }

        public async Task<IEnumerable<MaterielRapportDto>> GetMateriels(long RapportId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStreamAsync($"https://localhost:7149/api/MaterielRapport/{RapportId}");
                var MaterielRapports = await JsonSerializer.DeserializeAsync<List<MaterielRapportDto>>(
                    response,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ;
                return (IEnumerable<MaterielRapportDto>)MaterielRapports;
                // Handle the HTTP response
             
            }
        }
    }
}
