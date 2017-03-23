
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleSheetsApi.Authorization;

namespace GoogleSheetsApi.Programs
{
    public class OauthClient : IGoogleApiClient
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        const string ApiKeyPath = "client_secret.json";
        const string ApplicationName = "Google Sheets API .NET Quickstart";
        const string ApplicationUser = "marlons@campaigntrack.com";
        static string[] ApplicationScopes = { SheetsService.Scope.SpreadsheetsReadonly };


        public void Start(string[] args)
        {
            // Get credential
            var credential = AuthorizationManager<UserCredential>.GetCredential(
                new UserCredentialProvider(),
                ApiKeyPath,
                ApplicationScopes,
                ApplicationUser);

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            //String spreadsheetId = "1dAa5tHdqkq2brQ5pfoFjiv1GkVxge52bn99Oz2AgrQw";
            String spreadsheetId = "1dAa5tHdqkq2brQ5pfoFjiv1GkVxge52bn99Oz2AgrQw";
            String range = "Data!A2:D";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Any())
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("ID = {0}, Name = {1}", row[0], row[2]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }
    }
}