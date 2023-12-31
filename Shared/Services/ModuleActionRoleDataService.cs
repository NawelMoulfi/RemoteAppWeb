﻿
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class ModuleActionRoleDataService : IModuleActionRoleDataService
    {
        private readonly HttpClient _httpClient;

        public ModuleActionRoleDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async  Task<ModuleActionRoleDto> AddModuleActionRole(ModuleActionRoleDto moduleActionRole)
        {
            Console.WriteLine($"The new module  Action Role in the Dataservice : {moduleActionRole}");
            Console.WriteLine($"The new moduleAction Role Id  in the Dataservice: {moduleActionRole.ModuleActionRoleId}");
            //Console.WriteLine($"The new module Action Role  Name in the DataService : {moduleaction.ModuleActionNom}");
            var moduleActionRoleJson =
              new StringContent(JsonSerializer.Serialize(moduleActionRole), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/ModuleActionRole", moduleActionRoleJson);
            Console.WriteLine($"Responcse code  moduleActionRole : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent  moduleActionRole: {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  ModuleActionRole: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ModuleActionRoleDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }
        public async Task<ModuleActionRoleDto> GetModuleActionRoleByRoleAndModuleActionId(int RoleId, int ModuleActionId)
        {
            return await JsonSerializer.DeserializeAsync<ModuleActionRoleDto>
           (await _httpClient.GetStreamAsync($"https://localhost:7149/api/ModuleActionRole/role/{RoleId}/module-action/{ModuleActionId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task DeleteModuleActionRole(int ModuleActionRoleId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7149/api/User/{ModuleActionRoleId}");
        }
    }
}
