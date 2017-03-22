using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Programs
{
    internal class ServiceAccountClient : IGoogleApiClient
    {
        // Fields
        //private const string ServiceAccountEmail = @"realtair-referrals@realtair-referrals.iam.gserviceaccount.com";
        private const string ApplicationName = "Google Sheets API .Net using Service Account";
        private string[] ApplicationScopes = new[] { SheetsService.Scope.Spreadsheets, SheetsService.Scope.Drive };

        // Methods
        public void Start(string[] args)
        {
            ServiceAccountCredential credential;
            using (var stream = new FileStream("realtair-referrals-835141247636.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential
                    .FromStream(stream)
                    .CreateScoped(ApplicationScopes)
                    .UnderlyingCredential as ServiceAccountCredential;
                
                if (!credential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Result)
                {
                    throw new InvalidOperationException("Access token request failed.");
                }
            }
            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1dAa5tHdqkq2brQ5pfoFjiv1GkVxge52bn99Oz2AgrQw";
            String range = "Data!A2:G";
            
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            //request.Fields = "A, B, C, D, G, I, J, K";
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Any())
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}", row[0], row[1], row[2], row[3], row[6]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }
    }
}
