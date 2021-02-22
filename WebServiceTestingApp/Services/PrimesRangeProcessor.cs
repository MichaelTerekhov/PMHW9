using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebServiceTestingApp.Dtos;

namespace WebServiceTestingApp.Services
{
    public static class PrimesRangeProcessor
    {
        public static async Task CollectPrimeRanges(HttpClient client, string partOfUrn, List<int> primes)
        {
            try
            {
                var newUri = client.BaseAddress + partOfUrn;
                var response = await client.GetAsync(newUri);
                string body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var dto = JsonSerializer.Deserialize<ResponseResult>(body);
                    bool AreEqualFlag = false;
                    if (dto.Primes.Count == 0 && primes.Count == 0)
                    {
                        AreEqualFlag = true;
                        Console.WriteLine("Test completed: Gotten list are equal\n" +
                            $"Requested Uri {newUri}");
                        return;
                    }
                    for (var i = 0; i < dto.Primes.Count; i++)
                    {
                        if (dto.Primes[i] == primes[i])
                        {
                            AreEqualFlag = true;
                            continue;
                        }
                        else
                        {
                            AreEqualFlag = false;
                            break;
                        }
                    }
                    if (AreEqualFlag)
                        Console.WriteLine("Test completed: Gotten list are equal\n" +
                            $"Requested Uri {newUri}");
                    else
                    {
                        Console.WriteLine("Test ERROR: Gotten list are NOT equal\n" +
                                $"Requested Uri{newUri}\n" +
                                $"Expected: ");
                        foreach (var m in primes)
                            Console.Write(m + " ");
                        Console.WriteLine("Received:");
                        foreach (var m in dto.Primes)
                            Console.Write(m + " ");
                    }
                }
                else 
                {
                    Console.WriteLine("Sorry! But we cant process this response\n" +
                        $"BECAUSE status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OOPs smth went wrong\n" +
                    $"Check type of exception: {ex}\n" +
                    $"{ex.Message}");
            }
        }
    }
}
