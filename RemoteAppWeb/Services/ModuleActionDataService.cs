using RemoteApp.Data.Models;
using RemoteAppAp.Data;
using RemoteAppWeb.Services.Contracts;

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
        public async  Task<ModuleAction> AddModuleAction(ModuleAction moduleaction)
        {
            Console.WriteLine($"The new module  Action in the Dataservice : {moduleaction}");
            Console.WriteLine($"The new moduleAction Id  in the Dataservice: {moduleaction.ModuleActionID}");
            Console.WriteLine($"The new module Action  Name in the DataService : {moduleaction.ModuleActionNom}");
            var moduleJson =
              new StringContent(JsonSerializer.Serialize(moduleaction), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/ModuleAction", moduleJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  ModuleAction: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
               return  await JsonSerializer.DeserializeAsync<ModuleAction>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async  Task DeleteModuleAction(int moduleactionId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7023/api/ModuleAction/{moduleactionId}");
        }

        public async Task<IEnumerable<RemoteApp.Data.Models.Action>> GetActionsByResource(Resource resource)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7023/api/ModuleAction/resource/{resource}/resource-actions");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var Actions = await JsonSerializer.DeserializeAsync<List<RemoteApp.Data.Models.Action>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<RemoteApp.Data.Models.Action>)Actions;
            }
            else
            {
                return new List<RemoteApp.Data.Models.Action>();
            }
        }

        public async Task<IEnumerable<ModuleAction>> GetActionsByResourceExcludingFirst()
        {
            var response = await _httpClient.GetAsync("https://localhost:7023/api/ModuleAction/resource/actions/excluding-first");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleAction>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleAction>)ModuleActions;
            }
            else
            {
                return new List<ModuleAction>();
            }
        
    }

        public async Task<IEnumerable<ModuleAction>> GetListModuleActionsByResource(Resource resource)
        {

            var response = await _httpClient.GetAsync($"https://localhost:7023/api/ModuleAction/resource/{resource}/actions");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleAction>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleAction>)ModuleActions;
            }
            else
            {
                return new List<ModuleAction>();
            }
        }

        public async  Task<ModuleAction> GetModuleActionById(int ModuleActionId)
        {

            return await JsonSerializer.DeserializeAsync<ModuleAction>
             (await _httpClient.GetStreamAsync($"https://localhost:7023/api/ModuleAction/{ModuleActionId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<ModuleAction>> GetModuleActions()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7023/api/ModuleAction");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleAction>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );


                    return (IEnumerable<ModuleAction>)ModuleActions;
                }
                else
                {
                    return new List<ModuleAction>();
                }
            }
        }

        public  async Task<IEnumerable<ModuleAction>> GetResourceAction(ResourceActionParameters parameters)
        {
            var response = await _httpClient.GetAsync("https://localhost:7023/api/ModuleAction/resource/{parameters}");

            // Handle the HTTP response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response and perform any other related actions
                Console.WriteLine($"Responcse code of Used Resource  : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent of Used Resource  : {response.ReasonPhrase}");
                var responseStream = await response.Content.ReadAsStreamAsync();
                var ModuleActions = await JsonSerializer.DeserializeAsync<List<ModuleAction>>(
                    responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                return (IEnumerable<ModuleAction>)ModuleActions;
            }
            else
            {
                return new List<ModuleAction>();
            }
        }

        public async Task<IEnumerable<Resource>> GetUsedResources()
        {
            var response = await _httpClient.GetAsync("https://localhost:7023/api/ModuleAction/resource/used");

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

        public  async  Task UpdateModuleAction(ModuleAction moduleaction)
        {
            Console.WriteLine($"The update moduleaction  in the service : {moduleaction}");
            Console.WriteLine($"The updated moduleaction Id in the service : {moduleaction.ModuleActionID}");
            Console.WriteLine($"The updated moduleaction Name in the service : {moduleaction.ModuleActionNom}");
            Console.WriteLine($"The updated moduleaction Module Id in the service : {moduleaction.ModuleID}");
            var moduleactionJson =
                   new StringContent(JsonSerializer.Serialize(moduleaction), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:7023/api/ModuleAction", moduleactionJson);
            Console.WriteLine($"Responcse code of update user : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent of update user  : {response.ReasonPhrase}"); // Your API logic
            Console.WriteLine($"Responcse  of update moduleaction  : {response}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of update user: {responseBody}");
        }
    }
}
