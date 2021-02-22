using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WebTask_1.Models;

namespace WebTask_1.Models.Impl
{
     public class Settings:ISettings
     {
        [JsonPropertyName("primesFrom")]
        public int PrimesFrom { get; set; }
        [JsonPropertyName("primesTo")]
        public int PrimesTo { get; set; }
     }
}
