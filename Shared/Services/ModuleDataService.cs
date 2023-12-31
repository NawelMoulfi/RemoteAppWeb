﻿
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class ModuleDataService : IModuleDataService
    {
        private readonly HttpClient _httpClient;

        public ModuleDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
           
        }
        
        public async  Task<ModuleDto> Add(ModuleDto module)
        {
            Console.WriteLine($"The new module in the Dataservice : {module}");
            Console.WriteLine($"The new module Id in the Dataservice: {module.ModuleID}");
            Console.WriteLine($"The new module  Name in the DataService : {module.ModuleNom}");
            var moduleJson =
              new StringContent(JsonSerializer.Serialize(module), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/Module", moduleJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  user: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ModuleDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public  async Task DeleteModule(int id)
        { 
           await _httpClient.DeleteAsync($"https://localhost:7149/api/Module/{id}");
        }

        public async  Task<ModuleDto> GetModuleById(int ModuleId)
        {
            return await JsonSerializer.DeserializeAsync<ModuleDto>
             (await _httpClient.GetStreamAsync($"https://localhost:7149/api/Module/{ModuleId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public  async Task<IEnumerable<ModuleDto>> GetModules()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7149/api/Module");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Modules = await JsonSerializer.DeserializeAsync<List<ModuleDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    
                    return (IEnumerable<ModuleDto>)Modules;
                }
                else
                {
                    return new List<ModuleDto>();
                }
            }

        }

        public async  Task UpdateModule(ModuleDto module)
        {
            Console.WriteLine($"The update module  in the service : {module}");
            Console.WriteLine($"The updated module Id in the service : {module.ModuleID}");
            Console.WriteLine($"The updated module Nom in the service : {module.ModuleNom}");
            Console.WriteLine($"The updated  module Groupe  in the service : {module.ModuleGroup}");
            var moduleJson =
                   new StringContent(JsonSerializer.Serialize(module), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:7149/api/Module", moduleJson);
            Console.WriteLine($"Responcse code of update module : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent of update module  : {response.ReasonPhrase}"); // Your API logic
            Console.WriteLine($"Responcse  of update module  : {response}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of update module: {responseBody}");
        }
    }
}
