
using DxBlazorApplication1API.Controllers;
using RemoteAppApi.Controllers;
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RemoteAppWeb.Services
{

    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;

        public UserDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
           // _httpClient.BaseAddress = new Uri("https://localhost:7023/");
        }
        public  async Task<UserDto> AddUser(UserDto user)
        {
            Console.WriteLine($"The new user in the Dataservice : {user}");
            Console.WriteLine($"The new usr Id in the Dataservice: {user.UserId}");
            Console.WriteLine($"The new user Login in the DataService : {user.UserLogin}") ;
            var userJson =
              new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7149/api/User", userJson);
            Console.WriteLine($"Responcse code : {response.StatusCode}");
            Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of the New  user: {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<UserDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async  Task DeleteUser(int userId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7149/api/User/{userId}");
        }

        public async  Task<IEnumerable<UserDto>> GetAllUsers()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7149/api/User");

                // Handle the HTTP response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response and perform any other related actions
                    Console.WriteLine($"Responcse code : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var Users = await JsonSerializer.DeserializeAsync<List<UserDto>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    // Use 'folders' or perform additional actions
                    return (IEnumerable<UserDto>)Users;
                }
                else
                {
                    // Handle the case when the response is not successful
                    // You might want to throw an exception, log an error, or take other appropriate actions here.
                    // In this case, we return an empty list of folders as a default value.
                    return new List<UserDto>();
                }
            }



         
        }

        public async Task UpdateUser(UserDto user)
        {
           
                Console.WriteLine($"The update user  in the service : {user}");
                Console.WriteLine($"The updated usr Id in the service : {user.UserId}");
                Console.WriteLine($"The updated user Login in the service : {user.UserLogin}");
            Console.WriteLine($"The updated user Role Id  in the service : {user.RoleId}");
            var userJson =
                   new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("https://localhost:7149/api/User", userJson);
                Console.WriteLine($"Responcse code of update user : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent of update user  : {response.ReasonPhrase}"); // Your API logic
            Console.WriteLine($"Responcse  of update user  : {response}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response body of update user: {responseBody}");


        }
        public async Task<UserDto> GetUser(int userId)
        {
            return await JsonSerializer.DeserializeAsync<UserDto>
             (await _httpClient.GetStreamAsync($"https://localhost:7149/api/User/{userId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
     /*  public async Task SaveNewPassword(int uId, string password)
        {
            await _httpClient.GetAsync("api/User/"); 
        }+*/
        public async Task SaveNewPassword(int uId, string password)
        {
            try
            {
                // Create an object to send in the request body
                var changePasswordRequest = new ChangePasswordRequest
                {
                    UserId = uId,
                    NewPassword = password
                };

                // Serialize the request object to JSON
                var json = JsonSerializer.Serialize(changePasswordRequest);

                // Create a StringContent object with the JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the ChangePassword endpoint
                var response = await _httpClient.PostAsync("https://localhost:7149/api/User/changepassword", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Responcse code Request Change Password : {response.StatusCode}");
                    Console.WriteLine($"Responcse contenent Request Change Password : {response.ReasonPhrase}"); 
                    Console.WriteLine($"Responcse  Request Change Password : {response}");
                    // Password changed successfully
                    return; // You can return or perform additional actions here
                }
                else
                {
                    // Handle the case where the password change request was not successful
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        // You can parse and handle the error response, if any
                        Console.WriteLine("Bad request: " + errorResponse);
                        // Throw an exception or perform other error handling as needed
                    }
                    else
                    {
                        Console.WriteLine($"Password change failed with status code: {response.StatusCode}");
                        // Handle other error cases
                        // Throw an exception or perform other error handling as needed
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any HTTP request exceptions here
                Console.WriteLine("HTTP request exception: " + ex.Message);
                // Throw an exception or perform other error handling as needed
            }
        }

        /*     public async Task<User> FindUserByCredentials(string username, string password)
             {

             }*/
        public async Task<UserDto> Login(string username,string password)
        {

            try
            {
                // Ensure the request contains the necessary data (username, password)
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null; // Return null or handle the case of invalid login credentials.
                }
                var request = new LoginRequest
                {
                    Username = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(request); // Serialize the request object

                // Create a StringContent object with the JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //_httpClient.BaseAddress = new Uri("https://localhost:7023/");
                // Find the user by credentials (username and password)
                var response = await _httpClient.PostAsync($"https://localhost:7149/api/User/login", content);
                Console.WriteLine($"Responcse code : {response.StatusCode}");
                Console.WriteLine($"Responcse contenent : {response.ReasonPhrase}");
                Console.WriteLine($"Responcse : {response}");
                Console.WriteLine($"Responcse deserialized : {response.Content}");



                if (response.IsSuccessStatusCode)
                {
                    var user = await JsonSerializer.DeserializeAsync<UserDto>(await response.Content.ReadAsStreamAsync());
                    // Now 'user' contains the deserialized User object.
                    return user;
                }
                else
                {
                    // Handle error here, e.g., log the error or throw an exception.
                    // You can also inspect the response content for debugging purposes.
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API returned an error: {response.StatusCode}, Content: {errorContent}");
                    return null;
                    throw new Exception($"API request failed: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any HTTP request exceptions here
                Console.WriteLine("HTTP request exception: " + ex.Message);
                return null; // Return null or handle the exception case.
            }
        }

    }
}
