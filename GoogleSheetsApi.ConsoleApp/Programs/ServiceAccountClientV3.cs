using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using GoogleSheetsApi.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Programs
{
    internal class ServiceAccountClientV3 : IGoogleApiClient
    {
        // Fields
        //private const string ServiceAccountEmail = @"realtair-referrals@realtair-referrals.iam.gserviceaccount.com";
        private const string ApplicationName = "Google Sheets API .Net using Service Account";
        private const string ApiKeyPath = "realtair-referrals-835141247636.json";
        private const string ApplicationScopes = @"https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        // Methods
        public void Start(string[] args)
        {
            // Get credential
            var credential = AuthorizationManager<ServiceCredential>.GetCredential(
                new ServiceAccountProvider(),
                ApiKeyPath,
                ApplicationScopes);
            
            // Create request
            var requestFactory = new GDataRequestFactory(null);
            requestFactory.CustomHeaders.Add("Authorization: Bearer " + ((ServiceAccountCredential)credential).Token.AccessToken);

            // Create service using the request
            var service = new SpreadsheetsService(null) { RequestFactory = requestFactory };

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            string docKey = "1dAa5tHdqkq2brQ5pfoFjiv1GkVxge52bn99Oz2AgrQw";
            string gDocsURL = "https://spreadsheets.google.com/feeds/spreadsheets/private/full/{0}";
            string docURL = String.Format(gDocsURL, docKey);
            SpreadsheetQuery query = new SpreadsheetQuery()
            {
                Uri = new Uri(docURL) 
            };

            // Make a request to the API and get all spreadsheets.
            SpreadsheetFeed feed = service.Query(query);
            if (feed.Entries.Count == 0)
            {
                Console.WriteLine("There are no sheets");
            }

            SpreadsheetEntry spreadsheet = feed.Entries.SingleOrDefault() as SpreadsheetEntry;
            var worksheet = spreadsheet.Worksheets.Entries.SingleOrDefault(e => (e as WorksheetEntry).Title.Text == "Data") as WorksheetEntry;

            // Define the URL to request the list feed of the worksheet.
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            listQuery.SpreadsheetQuery = "type=Sales";
            ListFeed listFeed = service.Query(listQuery);
            Console.WriteLine(listFeed.Entries.Count);

            return;

            // Iterate through each row, printing its cell values.
            foreach (ListEntry row in listFeed.Entries)
            {
                // Print the first column's cell value
                Console.WriteLine(row.Title.Text);
                // Iterate over the remaining columns, and print each cell value
                foreach (ListEntry.Custom element in row.Elements)
                {
                    Console.WriteLine(element.Value);
                }
            }

            return;

            // Iterate through all of the spreadsheets returned
            foreach (SpreadsheetEntry sheet in feed.Entries)
            {
                // Print the title of this spreadsheet to the screen
                Console.WriteLine(sheet.Title.Text);

                // Make a request to the API to fetch information about all
                // worksheets in the spreadsheet.
                var wsFeed = sheet.Worksheets;

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
