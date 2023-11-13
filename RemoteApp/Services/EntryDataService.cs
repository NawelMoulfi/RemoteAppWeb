﻿using RemoteApp.Data.Models;
using RemoteApp.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace RemoteApp.Services
{
    public class EntryDataService : IEntryDataService
    {
        private readonly HttpClient _httpClient;

        public EntryDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Entry> AddEntry(Entry entry)
        {
            var entryJson =
               new StringContent(JsonSerializer.Serialize(entry), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7023/api/Entry", entryJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Entry>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteEntry(long entryId)
        {

            await _httpClient.DeleteAsync($"https://localhost:7023/api/Entry/{entryId}");
        }

        public async Task<IEnumerable<Entry>> GetAllEntries()
        {
            //   _httpClient.BaseAddress = new Uri("https://localhost:7023/");
            //return await JsonSerializer.DeserializeAsync<IEnumerable<Entry>>
            // (await _httpClient.GetStreamAsync($"https://localhost:7023/api/Entry"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7023/api/Entry");
            response.EnsureSuccessStatusCode();
            Console.WriteLine($"Entry Content : {response.Content}");
            Console.WriteLine($"Responcse Entry code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
            // Console.WriteLine($"Entry Liste : {await response.Content.ReadFromJsonAsync<IEnumerable<Entry>>()}");
            return await response.Content.ReadFromJsonAsync<IEnumerable<Entry>>();
        }

        public async Task<Entry> GetEntry(long EntryId)
        {
            return await JsonSerializer.DeserializeAsync<Entry>
         (await _httpClient.GetStreamAsync($"api/Entry/{EntryId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public  async Task UpdateEntry(Entry entry)
        {
            var entryJson =
             new StringContent(JsonSerializer.Serialize(entry), Encoding.UTF8, "application/json");


             await _httpClient.PutAsync("https://localhost:7023/api/Entry", entryJson);
           
        }
    }
}
