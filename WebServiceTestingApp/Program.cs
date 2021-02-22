using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebServiceTestingApp.Models;
using WebServiceTestingApp.Services;

namespace WebServiceTestingApp
{
    class Program
    {
        private static HttpClient client;
        static async Task Main(string[] args)
        {
            var jsonSettings = await File.ReadAllTextAsync("connectionSettings.json");
            var customUri = JsonSerializer.Deserialize<CustomUri>(jsonSettings);
            client = new HttpClient()
            {
                BaseAddress = new Uri(customUri.Uri)
            };
            await DefaultEndpointTesting(client);
            await IsNumberPrimeCheckingEndpoint(client);
            await IsPrimeNumberRangeCorrectCheckingEndpoint(client);
        }

        private static async Task DefaultEndpointTesting(HttpClient client)
        {
            try
            {
                var response = (await client.GetAsync(client.BaseAddress)).EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.ForegroundColor = ConsoleColor.Yellow;
                string expected = "Hello World!\n" +
                        "This web service was made by Michael Terekhov!\n" +
                        "You can easily search for prime numbers,\n" +
                        "pass certain parameters, or do a simple number check (Is it prime)";
                Console.WriteLine($"URI{client.BaseAddress}\n" +
                    $"Testing basic text output: [RESULT]{body.Equals(expected)}");
                Console.ForegroundColor = ConsoleColor.Red;             
                Console.WriteLine("Special checker of false equal message");
                Console.ForegroundColor = ConsoleColor.Green;
                string falseExpected = String.Empty;
                if(body != falseExpected)
                    Console.WriteLine("Congratulations!!! Api sending true response [CHECK UPPER TEXT]");
                Console.ResetColor();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Smth went wrong\n" +
                    $"Exception type{ex}");
            }
        }
        private static async Task IsNumberPrimeCheckingEndpoint(HttpClient client)
        {
            //locally tried to get passed test
            await IsPrimeProcessor.PrimeChecker(client, "primes/1", 404);
            await IsPrimeProcessor.PrimeChecker(client, "primes/13", 200);
            Console.WriteLine("Chechking some optional get requests for prime numbers");
            var testingDictionary = new DesirializerForPrimes().GetOptionsList();
            foreach (var m in testingDictionary)
            {
                await Task.Run(() => IsPrimeProcessor.PrimeChecker(client, m.Key, m.Value));
            }
            await Task.WhenAll();
        }
        private static async Task IsPrimeNumberRangeCorrectCheckingEndpoint(HttpClient client)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            await PrimesRangeProcessor.CollectPrimeRanges(client,"primes/?from=abc",new List<int>() {2,3,5});
            
            Console.WriteLine("Chechking some optional get requests for prime lists");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var jsonSettings = await  File.ReadAllTextAsync("primesLists.json");
            var testingDictionary =  JsonSerializer.Deserialize<Dictionary<string, List<int>>>(jsonSettings);
            /* var tasks = testingDictionary.Select(test=>
             PrimesRangeProcessor.CollectPrimeRanges(client,test.Key,test.Value));
             await Task.WhenAll(tasks);*/ //commented because spotted some weird buggs and i cant cooped with them
            //For example, sometimes get bad requset? where its imposible :D
            var tasks = await Task.Factory.StartNew(() => testingDictionary.Select(test =>
              PrimesRangeProcessor.CollectPrimeRanges(client, test.Key, test.Value)));
            Console.ResetColor();
            await Task.WhenAll(tasks);
        }
    }
}
