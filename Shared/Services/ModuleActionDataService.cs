
using RemoteAppApi.Controllers;
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class ModuleActionDataService : IModuleActionDataService
    {
        private readonly HttpClient _httpClient;

        public ModuleActionDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
          
        }
        public async  Task<ModuleActionDto> AddModuleAction(ModuleActionDto moduleaction)
        {
            Console.WriteLine($"The new module  Action in the Dataservice : {moduleaction}");
            Console.WriteLine($"The new moduleAction Id  in the Dataservice: {moduleaction.ModuleActionID}");
            Console.WriteLine($"The new module Action  Name in the DataService : {moduleaction.ModuleActionNom}");
            var moduleJson =
              new StringContent(JsonSerializer.Serialize(moduleaction), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/ModuleAction", moduleJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  ModuleAction: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
               return  await JsonSerializer.DeserializeAsync<ModuleActionDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async  Task DeleteModuleAction(int moduleactionId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7149/api/ModuleAction/{moduleactionId}");
        }

        public async Task<IEnumerable<Shared.Dto.Action>> GetActionsByResource(Resource resource)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7149/api/ModuleAction/resource/{resource}/resource-actions");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var Actions = await JsonSerializer.DeserializeAsync<List<Shared.Dto.Action>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<Shared.Dto.Action>)Actions;
            }
            else
            {
                return new List<Shared.Dto.Action>();
            }
        }

        public async Task<IEnumerable<ModuleActionDto>> GetActionsByResourceExcludingFirst()
        {
            var response = await _httpClient.GetAsync("https://localhost:7149/api/ModuleAction/resource/actions/excluding-first");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleActionDto>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleActionDto>)ModuleActions;
            }
            else
            {
                return new List<ModuleActionDto>();
            }
        
    }

        public async Task<IEnumerable<ModuleActionDto>> GetListModuleActionsByResource(Resource resource)
        {

            var response = await _httpClient.GetAsync($"https://localhost:7149/api/ModuleAction/resource/{resource}/actions");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleActionDto>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleActionDto>)ModuleActions;
            }
            else
            {
                return new List<ModuleActionDto>();
            }
        }

        public async  Task<ModuleActionDto> GetModuleActionById(int ModuleActionId)
        {

            return await JsonSerializer.DeserializeAsync<ModuleActionDto>
             (await _httpClient.GetStreamAsync($"https://localhost:7149/api/ModuleAction/{ModuleActionId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<ModuleActionDto>> GetModuleActions()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7149/api/ModuleAction");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleActionDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );


                    return (IEnumerable<ModuleActionDto>)ModuleActions;
                }
                else
                {
                    return new List<ModuleActionDto>();
                }
            }
        }

        public  async Task<IEnumerable<ModuleActionDto>> GetResourceAction(ResourceActionParameters parameters)
        {
            var response = await _httpClient.GetAsync("https://localhost:7149/api/ModuleAction/resource/{parameters}");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code of Used Resource  : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent of Used Resource  : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleActionDto>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleActionDto>)ModuleActions;
            }
            else
            {
                return new List<ModuleActionDto>();
            }
        }

        public async Task<IEnumerable<Resource>> GetUsedResources()
        {
            var response = await _httpClient.GetAsync("https://localhost:7149/api/ModuleAction/resource/used");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code of Used Resource  : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent of Used Resource  : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var Resources = await JsonSerializer.DeserializeAsync<List<Resource>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return Resources;
            }
            else
            {
                return new List<Resource>();
            }
        }

        public  async  Task UpdateModuleAction(ModuleActionDto moduleaction)
        {
            Console.WriteLine($"The update moduleaction  in the service : {moduleaction}");
            Console.WriteLine($"The updated moduleaction Id in the service : {moduleaction.ModuleActionID}");
            Console.WriteLine($"The updated moduleaction Name in the service : {moduleaction.ModuleActionNom}");
            Console.WriteLine($"The updated moduleaction Module Id in the service : {moduleaction.ModuleID}");
            var moduleactionJson =
                   new StringContent(JsonSerializer.Serialize(moduleaction), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:7149/api/ModuleAction", moduleactionJson);
            Console.WriteLine($"Responcse code of update user : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent of update user  : {response.ReasonPhrase}"); // Your API logic
            Console.WriteLine($"Responcse  of update moduleaction  : {response}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of update user: {responseBody}");
        }
    }
}
