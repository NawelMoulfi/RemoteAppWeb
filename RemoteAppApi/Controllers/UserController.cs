

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Controllers;
using RemoteAppApi.Repositories.Contracts;

namespace DxBlazorApplication1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userRepository.GetUserById(id));
        }
        [HttpGet("ByUserName/{UserName}")]
        public async Task<IActionResult> GetUserByUID(string UserName)
        {
            return Ok(await _userRepository.FindUserByUID(UserName));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if ((user.UserNom == string.Empty)||(user.UserPrenom == string.Empty) )
            {
                ModelState.AddModelError("UserNom et UserPrenom", "UserPrenom et UserNom shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userRepository.AddUser(user);

            return Created("user", createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if ((user.UserNom == string.Empty) || (user.UserPrenom == string.Empty))
            {
                ModelState.AddModelError("UserName", " Username shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToUpdate = await _userRepository.GetUserById(user.UserId);

            if (userToUpdate == null)
                return NotFound();

            await _userRepository.UpdateUser(user);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == 0)
                return BadRequest();

            var userToDelete = await _userRepository.GetUserById(id);
            if (userToDelete == null)
                return NotFound();

            await _userRepository.DeleteUser(id);

            return NoContent();//success
        }
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            // Ensure the request contains the necessary data (userId, newPassword)
            if (request.UserId <= 0 || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Invalid request data.");
            }


            // Save the new password
            await _userRepository.SaveNewPassword(request.UserId, request.NewPassword);

            return Ok("Password changed successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {// HTTP POST request is used for the login operation, not to create data,
         // to securely send sensitive credentials (username and password) to the server.
         // This practice enhances security by protecting credentials from being exposed in the URL.

            // Ensure the request contains the necessary data (username, password)
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid login credentials.");
            }

            // Find the user by credentials (username and password)

            var user = await _userRepository.FindUserByCredentials(request.Username, request.Password);
            if(user!=null) {
                Console.WriteLine($" Userlogin in user controller: {user.UserLogin}");
                Console.WriteLine($" UserId in user controller: {user.UserId}");

                Console.WriteLine($" User response in json : {Ok(user)}");
                var jsonUser = System.Text.Json.JsonSerializer.Serialize(user);
                Console.WriteLine($" User response in json manuale  : {Content(jsonUser, "application/json")}");

                // Return the JSON string as content with the appropriate content type
                return Content(jsonUser, "application/json");
            }
            else
            {
                return null;
            }
          

          
            //return Ok(user);
        }

    }

   
}
