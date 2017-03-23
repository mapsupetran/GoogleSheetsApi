﻿using Google.Apis.Auth.OAuth2;

namespace Google.Authorization
{
    public interface IAuthorizationProvider<T> where T : ICredential
    {
        ICredential CreateCredential(string apiKeyPath, string scopes, string user = null);
    }
}