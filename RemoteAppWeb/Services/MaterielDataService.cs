﻿using RemoteApp.Data.Models;
using RemoteAppWeb.Services.Contracts;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class MaterielDataService : IMaterielDataService
    {
        private readonly HttpClient _httpClient;

        public MaterielDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async  Task<IEnumerable<Materiel>> GetAllMateriels()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Materiel>>
            (await _httpClient.GetStreamAsync($"https://localhost:7023/api/Materiel"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
