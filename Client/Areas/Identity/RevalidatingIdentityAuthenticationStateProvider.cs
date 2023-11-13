using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace RemoteAppWeb.Areas.Identity
{
    public class RevalidatingIdentityAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        // AuthenticationState authState;


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1500);
            // Implement your logic here to dynamically fetch the user's authentication state
            /*  var userClaims = await AuthenticationState.Get;

              // Create a new ClaimsPrincipal with the retrieved claims
              var claimsIdentity = new ClaimsIdentity(userClaims, "customAuthType");
              var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

              // Return the authentication state
              return await Task.FromResult(new AuthenticationState(claimsPrincipal));*/
            var claims = new List<Claim>
          {
              new Claim(ClaimTypes.Name, "user"),
                new Claim(ClaimTypes.Email, "in_moulfi@esi.dz"),
                 new Claim(ClaimTypes.NameIdentifier, "4"),
              new Claim(ClaimTypes.Role, "Admin")
          };
            var anonymous = new ClaimsIdentity(claims, "testAuthType");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
            // return await Task.FromResult(authState);
        }
        public async Task SetAuthenticationStateAsync(ClaimsPrincipal claimsPrincipal)
        {
            var authState = new AuthenticationState(claimsPrincipal);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }


    }
}
