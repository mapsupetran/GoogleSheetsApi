using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace Google.Authorization
{
    public class ServiceAccountProvider : AuthorizationProviderBase<ServiceCredential>
    {
        public override ICredential CreateCredential(string apiKeyPath, string scopes, string user = null)
        {
            ServiceAccountCredential credential;
            using (var stream = new FileStream(apiKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential
                    .FromStream(stream)
                    .CreateScoped(scopes)
                    .UnderlyingCredential as ServiceAccountCredential;

                if (!credential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Result)
                {
                    throw new InvalidOperationException("Access token request failed.");
                }
            }

            return credential;
        }
    }
}
