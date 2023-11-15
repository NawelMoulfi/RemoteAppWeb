
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
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
        public async  Task<IEnumerable<MaterielDto>> GetAllMateriels()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<MaterielDto>>
            (await _httpClient.GetStreamAsync($"https://localhost:7149/api/Materiel"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
