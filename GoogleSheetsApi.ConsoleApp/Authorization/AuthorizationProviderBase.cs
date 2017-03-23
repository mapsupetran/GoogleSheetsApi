using GoogleSheetsApi.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace GoogleSheetsApi.Authorization
{
    internal abstract class AuthorizationProviderBase<T> : IAuthorizationProvider<T> where T : ICredential
    {
        public abstract ICredential CreateCredential(string apiKeyPath, string scopes, string user = null);
    }
}
