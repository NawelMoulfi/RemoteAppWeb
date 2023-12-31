﻿
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{
    public class FolderDataService : IFolderDataService
    {
        private readonly HttpClient _httpClient;

        public FolderDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
           // _httpClient.BaseAddress = new Uri("https://localhost:7023/");
        }
        public  async Task<FolderDto> AddFolder(FolderDto folder)
        {

            Console.WriteLine($"The new folder  in the service : {folder}");
            Console.WriteLine($"The new folder Id in the service : {folder.FolderId}");
            Console.WriteLine($"The new folder Name in the service : {folder.FolderName}");
            Console.WriteLine($"The new folder Description in the service : {folder.FolderDescription}");
            Console.WriteLine($"The new folder Parent folder Id in the service  : {folder.ParentFolderId}");
            var entryJson =
              new StringContent(JsonSerializer.Serialize(folder), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/Folder", entryJson);
            Console.WriteLine($"Responcse code  of Add folder: {response.StatusCode}");
            Console.WriteLine($"Responcse contenent of Add folder : {response.ReasonPhrase}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<FolderDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteFolder(int folderId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7149/api/Folder/{folderId}");
        }

        public  async Task<IEnumerable<FolderDto>> GetAllFolders()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7149/api/Folder");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var folders = await JsonSerializer.DeserializeAsync<List<FolderDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    // Use 'folders' or perform additional actions
                    return folders;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<FolderDto>();
                }
            }

            //_httpClient.BaseAddress = new Uri("https://localhost:7023/");
            //  return await JsonSerializer.DeserializeAsync<IEnumerable<Folder>>
            // (await _httpClient.GetStreamAsync($"https://localhost:7023/api/Folder"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }


        public async Task<FolderDto> GetFolder(int folderId)
        {
            return await JsonSerializer.DeserializeAsync<FolderDto>
        (await _httpClient.GetStreamAsync($"https://localhost:7149/api/Folder/{folderId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateFolder(FolderDto folder)
        {
            var folderJson =
             new StringContent(JsonSerializer.Serialize(folder), Encoding.UTF8, "application/json");


            await _httpClient.PutAsync("https://localhost:7149/api/Folder", folderJson);
        }
    }
}
