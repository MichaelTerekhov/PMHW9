using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceTestingApp.Services
{
    public static class IsPrimeProcessor
    {
        public static async Task PrimeChecker(HttpClient client, string partOfUrn, int status)
        {
            try
            {
                var newUri = client.BaseAddress + partOfUrn;
                var response = await client.GetAsync(newUri);
                
                if((int)response.StatusCode == status )
                    Console.WriteLine($"Test passed: {client.BaseAddress + partOfUrn} getting true response status code!");
                else
                    Console.WriteLine($"Was expected:{status}" +
                        $"But you got {response.StatusCode} fron this URI: [{client.BaseAddress + partOfUrn}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Smth bad :(\n" +
                    $"Exception type: {ex}");
            }
        }
    }
}
