using GoogleSheetsApi.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace GoogleSheetsApi.ConsoleApp.Authorization
{
    internal abstract class AutorizationProviderBase : IAuthorizationProvider<ServiceCredential>
    {
        public abstract ICredential CreateCredential(string apiKeyPath, string scopes, string user = null);
    }
}
