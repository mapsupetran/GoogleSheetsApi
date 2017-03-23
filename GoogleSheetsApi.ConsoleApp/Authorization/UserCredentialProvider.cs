using GoogleSheetsApi.Authorization;
using GoogleSheetsApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;

namespace GoogleSheetsApi.Authorization
{
    internal class UserCredentialProvider : IAuthorizationProvider<UserCredential>
    {
        public ICredential CreateCredential(string apiKeyPath, string scopes, string user = null)
        {
            ICredential credential;

            using (var stream = new FileStream(apiKeyPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes.ToList(),
                    user,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }
    }
}
