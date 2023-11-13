using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RemoteAppWeb.Services.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RemoteAppWeb.Login
{
    public class BlazorCookieLoginMiddleware<TUser> where TUser : class
    {
        #region Static Login Cache

        static IDictionary<Guid, LoginModel<TUser>> Logins { get; set; }

            = new ConcurrentDictionary<Guid, LoginModel<TUser>>();

        public static Guid AnnounceLogin(LoginModel<TUser> loginInfo)
        {
            loginInfo.LoginStarted = DateTime.Now;
            var key = Guid.NewGuid();
            Logins[key] = loginInfo;
            return key;
        }
        public static LoginModel<TUser> GetLoginInProgress(string key)
        {
            return GetLoginInProgress(Guid.Parse(key));
        }

        public static LoginModel<TUser> GetLoginInProgress(Guid key)
        {
            if (Logins.ContainsKey(key))
            {
                return Logins[key];
            }
            else
            {
                return null;
            }
        }

        #endregion

        private readonly RequestDelegate _next;

        public BlazorCookieLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var serviceProvider = context.RequestServices;
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];
                var loginManager = new LoginManager(
    serviceProvider.GetRequiredService<IUserDataService>());
                var result =  await loginManager.TryLogin(info.login, info.password, info.isSuperUser);

                //Uncache password for security:
                info.password = null;

                if (result != null)
                {
                    Logins.Remove(key);
                    context.Response.Redirect("/");
                    return;
                }
                else
                {
                    info.error = "Login failed. Check your username and password.";
                    await _next.Invoke(context);
                }
            }
          
            else if (context.Request.Path.StartsWithSegments("/logout"))
            {
               // await LoginManager.SignOutAsync();
                context.Response.Redirect("/login");
                return;
            }

            //Continue http middleware chain:
            await _next.Invoke(context);
        }
    }
}
