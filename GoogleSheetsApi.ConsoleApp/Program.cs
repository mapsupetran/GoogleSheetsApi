using GoogleSheetsApi.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi
{
    class Program
    {
        static IGoogleApiClient client = new ServiceAccountClientV3();

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                client.Start(args);
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
