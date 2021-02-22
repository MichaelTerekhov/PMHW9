using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTask_1.Models;
using WebTask_1.Models.Impl;

namespace WebTask_1.Services
{
    public class PrimesFinderService
    {
        public PrimesFinderService(ISettings settings, ILogger<PrimesFinderService> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }
        public Task<bool> CheckIsPrime()
        {
            logger.LogInformation("Trying to get settings to check is it number is prime.");
            if (settings.PrimesFrom < 2)
            {
                logger.LogWarning($"Can`t to check, because settings param: {settings.PrimesFrom}\n" +
                    $"does not meet the possible conditions for searching prime numbers");
                return Task.FromResult(false);
            }
            else 
            {
                var result = PrimeAlgoFinder(settings.PrimesFrom);
                logger.LogInformation($"Operation succeded: Number -> {settings.PrimesFrom} Prime: {result}");
                return Task.FromResult(result);
            }
        }
         public async Task<Result> FindPrimesInRange()
         {
            return await Task.Run(() =>
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                logger.LogInformation("Trying to get settings to check is it number is prime.");
                List<int> result = new List<int>();
                if ((settings.PrimesFrom > settings.PrimesTo) && settings.PrimesFrom > 0)
                {
                    timer.Stop();

                    logger.LogInformation($"Primes in range from {settings.PrimesFrom} to {settings.PrimesTo} wasn`t found");
                    return new Result
                    {
                        Duration = TimeParser(timer),
                        Error = null,
                        Primes = result,
                        Success = true
                    };
                }
                if (settings.PrimesFrom < 0 && settings.PrimesTo < 0)
                {
                    timer.Stop();
                    logger.LogInformation($"Primes in range from {settings.PrimesFrom} to {settings.PrimesTo} wasn`t found");
                    return new Result
                    {
                        Duration = TimeParser(timer),
                        Error = null,
                        Primes = result,
                        Success = true
                    };
                }

                for (var i = settings.PrimesFrom; i < settings.PrimesTo + 1; i++)
                {
                    if (i <= 1) continue;
                    var isPrime = true;
                    for (var j = 2; j < i; j++)
                    {
                        if (i % j == 0)
                        {
                            isPrime = false;
                            break;
                        }
                    }
                    if (!isPrime) continue;
                    result.Add(i);
                }
                timer.Stop();
                logger.LogInformation($"Primes in range from {settings.PrimesFrom} to {settings.PrimesTo} was found succesfully");
                return new Result
                {
                    Duration = TimeParser(timer),
                    Error = null,
                    Primes = result,
                    Success = true
                };
            });
        }
        private bool PrimeAlgoFinder(int num)
        {
            for (int i = 2; i < num; i++)
                if (num % i == 0) 
                    return false;
            return true;
        }

        private static string TimeParser(Stopwatch time)
        {
            TimeSpan ts = time.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
            return elapsedTime;
        }
        private ISettings settings;
        private ILogger<PrimesFinderService> logger;
    }
}
