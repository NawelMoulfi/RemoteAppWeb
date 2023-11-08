using System;
using System.Security.Principal;
using System.Threading;

namespace RemoteAppWeb.Security
{
    public class ClientSecurityContext
    {
        public static IPrincipal CurrentPrincipal
        {
            get;
            set;
        }

        public static IDisposable CreateSecurityContext()
        {
            //
            // Save a copy of the previous IPrincipal so we can restore it
            // when the caller is finished.
            //
            var previous = Thread.CurrentPrincipal;

            //
            // Set the current thread's principal.
            //
            Thread.CurrentPrincipal = CurrentPrincipal;

            //
            // Return a notifier that will call the action when the caller
            // disposes of it. This will be used to restore the previous
            // IPrinicpal back on the current thread.
            //
            return new DisposeNotifier(
                () => Thread.CurrentPrincipal = previous);
        }
    }
}
