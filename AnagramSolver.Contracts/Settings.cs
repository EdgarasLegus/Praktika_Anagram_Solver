using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts
{
    public class Settings
    {
        private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(@"./appsettings.json")
            .Build();

        //public static string ConnectionString { get; set; }
        //public static int MaxNumberOfAnagrams { get; set; }
        //public static int MinInputWordLength { get; set; }
        //public static int MinNumberOfAnagrams { get; set; }
        public static string FileName { get; set; }

        public static string GetSettingsConnectionStringDBFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringDBFirst"];
            return connectionString;
        }

        public static string GetSettingsConnectionStringCodeFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringCodeFirst"];
            return connectionString;
        }

        public static int GetSettingsMaxSearchesForIP()
        {
            var maxSearchesForIP = Int32.Parse(configuration["Settings:MaxSearchesForIP"]);
            return maxSearchesForIP;
        }
    }
}

