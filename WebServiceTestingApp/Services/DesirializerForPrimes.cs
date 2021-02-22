using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebServiceTestingApp.Services
{
    public class DesirializerForPrimes
    {
        public DesirializerForPrimes()
        {
        }
        public Dictionary<string, int> GetOptionsList()
        {
            Desrialization();
            return optionsResult;
        }
        private void Desrialization()
        {
            var jsonSettings = File.ReadAllText("primeOptions.json");
            optionsResult = JsonSerializer.Deserialize<Dictionary<string,int>>(jsonSettings);
        }
        private Dictionary<string, int> optionsResult { get; set; }
    }
}
