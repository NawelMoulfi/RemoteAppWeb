
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RemoteApp.Login;
using Audit.Core;
using RemoteApp.Data.Models;
using RemoteApp.Services.Contracts;

namespace RemoteApp.Pages
{
    public class LoginModel : PageModel
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [BindProperty]
        public string UserLogin { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string LoginErrorMessage { get; set; }
        public bool ShowErrorMsg { get; set; }
        private readonly IUserDataService _userDataService;
        public LoginModel(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }


        public IActionResult OnGet()
        {
            // Here i am constructing the bounded model using the page directives id and name
          
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // var expectedPw = LoginManager.ExpectedSuPw;
            var isSuperUser = false  ;//(UserLogin.ToLower() == "su" || UserLogin.ToLower().StartsWith("su")) && Password == expectedPw;
            var machineName = GetMachineName();
            User user = await _userDataService.Login(UserLogin, Password); 
            //LoginManager.TryLogin(UserLogin, Password, isSuperUser);
            var principal = LoginManager.GetClaimsPrincipal(user, isSuperUser);
            if (user == null)
            {
                LoginErrorMessage = "Le nom d'utilisateur ou le mot de passe est incorrect !";
                AuditScope.Log("Login", new { LoginUserName = UserLogin, LoginSuccess = "Failed", Message = LoginErrorMessage, MachineName = machineName });
                return Page();
            }
            else
            {
                LoginErrorMessage = "";
                AuditScope.Log("Login", new { LoginUserName = UserLogin, LoginSuccess = "Succeeded", Message = $"Success login  {UserLogin}", MachineName = machineName });
               // var user = ; //LoginManager.FindUser(UserLogin, Password, isSuperUser);
                var expiresUtc = user.UserMaxCapacity == 0
                    ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddMinutes(user.UserMaxCapacity);

                await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal),
                  new AuthenticationProperties { ExpiresUtc = expiresUtc, IsPersistent = false });

                return LocalRedirect(Url.Content("~/Entries"));
            }
           
        }

        public string GetMachineName()
        {

            string machineName = "";
            try
            {
                var address = HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (address != null)
                {
                    var hostEntry = System.Net.Dns.GetHostEntry(address);
                    if (hostEntry != null && !hostEntry.HostName.Trim().ToLower().Contains("domain.name"))
                    {
                        machineName = hostEntry.HostName;
                    }
                }

                return machineName;
            }
            catch (Exception e)
            {
                
                return "";
            }


        }
    }
}