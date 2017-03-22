using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Programs
{
    internal class OauthClientV3 : IGoogleApiClient
    {
        // Fields
        //private const string ServiceAccountEmail = @"realtair-referrals@realtair-referrals.iam.gserviceaccount.com";
        private const string ApplicationName = "Google Sheets API .Net using Service Account";
        private string ApplicationScopes = @"https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

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

            var requestFactory = new GDataRequestFactory(null);
            requestFactory.CustomHeaders.Add("Authorization: Bearer " + credential.Token.AccessToken);

            var service = new SpreadsheetsService(null) { RequestFactory = requestFactory };

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery();

            // Make a request to the API and get all spreadsheets.
            SpreadsheetFeed feed = service.Query(query);

            if (feed.Entries.Count == 0)
            {
                Console.WriteLine("There are no sheets");
            }

            // Iterate through all of the spreadsheets returned
            foreach (SpreadsheetEntry sheet in feed.Entries)
            {
                // Print the title of this spreadsheet to the screen
                Console.WriteLine(sheet.Title.Text);

                // Make a request to the API to fetch information about all
                // worksheets in the spreadsheet.
                WorksheetFeed wsFeed = sheet.Worksheets;

                // Iterate through each worksheet in the spreadsheet.
                foreach (WorksheetEntry entry in wsFeed.Entries)
                {
                    // Get the worksheet's title, row count, and column count.
                    string title = entry.Title.Text;
                    var rowCount = entry.Rows;
                    var colCount = entry.Cols;

                    // Print the fetched information to the screen for this worksheet.
                    Console.WriteLine(title + "- rows:" + rowCount + " cols: " + colCount);

                    //// Create a local representation of the new worksheet.
                    //WorksheetEntry worksheet = new WorksheetEntry();
                    //worksheet.Title.Text = "New Worksheet";
                    //worksheet.Cols = 10;
                    //worksheet.Rows = 20;

                    //// Send the local representation of the worksheet to the API for
                    //// creation.  The URL to use here is the worksheet feed URL of our
                    //// spreadsheet.
                    //WorksheetFeed NewwsFeed = sheet.Worksheets;
                    //service.Insert(NewwsFeed, worksheet);
                }
            }
        }
    }
}
