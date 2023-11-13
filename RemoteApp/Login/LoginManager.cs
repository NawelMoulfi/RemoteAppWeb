

using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteApp.Helpers;
using RemoteApp.Security;
using RemoteApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;



namespace RemoteApp.Login
{
    public class LoginManager
    {
        private readonly IUserDataService _userDataService;

        public  LoginManager(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<User> TryLogin(string username, string password, bool isSuperUser)
        {
            // Ensure the request contains the necessary data (username, password)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null; // Return null or handle the case of invalid login credentials.
            }

            // Call the Login method from the injected IUserDataService
            User user = await _userDataService.Login(username, password);
            Console.WriteLine($"userLogin : {user.UserLogin}");
            Console.WriteLine($"userId : {user.UserId}");

            if (user != null)
            {
                // You can perform login actions here and generate a token if needed.
                return user; // Return the user or perform additional actions here.
            }
            else
            {
                // Handle the case of invalid username or password
                return null; // Return null or handle the unauthorized case.
            }
        }
        public static ClaimsPrincipal GetClaimsPrincipal(User user, bool isSuperUser)
        {
            if(user!=null) {
                Console.WriteLine($"userLogin : {user.UserLogin}");
                Console.WriteLine($"userId : {user.UserId}");
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserLogin),

                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.RoleName??""),
            };
                // isSuperUser = true;
                if (isSuperUser)
                    claims.Add(new Claim("SuperUser", "true"));
                else
                {
                    var actions = user?.Role?.ModuleActions.ToList();
                    if (actions == null)
                        return null;
                    foreach (var p in actions)
                    {
                        if (p != null)
                            claims.Add(new Claim("P", $"{(int)p.Action}-{(int)p.Resource}"));
                        //claims.Add(new Claim("permission", $"{EnumExtension.GetDisplayValue<Action>(p.Action)}-{EnumExtension.GetDisplayValue<Resource>(p.Resource)}"));
                    }
                }
                var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var cp = new ClaimsPrincipal(id);
                return cp;
            }
            else
            {
                return null; 
            }

         
        }
    }

}