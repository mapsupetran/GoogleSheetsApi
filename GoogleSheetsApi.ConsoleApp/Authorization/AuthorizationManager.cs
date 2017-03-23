using Google.Apis.Auth.OAuth2;
using GoogleSheetsApi.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Authorization
{
    internal class AuthorizationManager <T> where T : ICredential
    {
        // Fields
        private IAuthorizationProvider<T> authorizationProvider;
        private string scopes;
        private string apiKeyPath;
        private string user;
        
        // Properties
        public IAuthorizationProvider<T> AutorizationProvider
        {
            get { return authorizationProvider; }
        }
        public ICredential Crendential
        {
            get
            {
                return authorizationProvider.CreateCredential(apiKeyPath, scopes, user);
            }
        }
        public string Scopes
        {
            get { return scopes; }
            set { scopes = value; }
        }
        public string ApiKeyPath
        {
            get { return apiKeyPath; }
            set { apiKeyPath = value; }
        }
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        // Constructor
        public AuthorizationManager(IAuthorizationProvider<T> provider) 
        {
            this.authorizationProvider = provider;
        }
        public AuthorizationManager(IAuthorizationProvider<T> provider, string apiKeyPath) 
            : this(provider)
        {
            this.apiKeyPath = apiKeyPath;
        }
        public AuthorizationManager(IAuthorizationProvider<T> provider, string apiKeyPath, string scopes)
            : this(provider, apiKeyPath)
        {
            this.scopes = scopes;
        }
        public AuthorizationManager(IAuthorizationProvider<T> provider, string apiKeyPath, string[] scopes)
            : this(provider, apiKeyPath)
        {
            this.scopes = scopes.ToDelimitedString();
        }
        public AuthorizationManager(IAuthorizationProvider<T> provider, string apiKeyPath, string scopes, string user)
            : this(provider, apiKeyPath, scopes)
        {
            this.user = user;
        }
        public AuthorizationManager(IAuthorizationProvider<T> provider, string apiKeyPath, string[] scopes, string user)
            : this(provider, apiKeyPath, scopes)
        {
            this.user = user;
        }

        // Methods
        public static ICredential GetCredential(IAuthorizationProvider<T> provider, string apiKeyPath, string scopes, string user)
        {
            // Get credential
            var authorizationManager = new AuthorizationManager<T>(
                provider, 
                apiKeyPath, 
                scopes, 
                user);
            return authorizationManager.Crendential;
        }
        public static ICredential GetCredential(IAuthorizationProvider<T> provider, string apiKeyPath, string[] scopes, string user)
        {
            // Get credential
            var authorizationManager = new AuthorizationManager<T>(
                provider, 
                apiKeyPath, 
                scopes.ToDelimitedString(), 
                user);
            return authorizationManager.Crendential;
        }
        public static ICredential GetCredential(IAuthorizationProvider<T> provider, string apiKeyPath, string scopes)
        {
            // Get credential
            var authorizationManager = new AuthorizationManager<T>(
                provider, 
                apiKeyPath, 
                scopes);
            return authorizationManager.Crendential;
        }
        public static ICredential GetCredential(IAuthorizationProvider<T> provider, string apiKeyPath, string[] scopes)
        {
            // Get credential
            var authorizationManager = new AuthorizationManager<T>(
                provider, 
                apiKeyPath, 
                scopes.ToDelimitedString());
            return authorizationManager.Crendential;
        }
    }
}
