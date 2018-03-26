using RestSharp.Authenticators;
using TfsWebApi.Services.Entities;

namespace TfsWebApi.Restsharp
{
    public static class RestsharpExtensions
    {
        public static IAuthenticator ToRestsharpAuthenticator(this Authentication authentication)
        {
            if (authentication is BasicAuthentication auth)
            {
                return new HttpBasicAuthenticator(auth.username, auth.password);
            }
            else if (authentication is NtlmAuthentication)
            {
                return new NtlmAuthenticator();
            }

            return null;
        }
    }
}