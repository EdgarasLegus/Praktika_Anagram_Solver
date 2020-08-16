using AnagramSolver.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AnagramSolver.UI
{
    public class Configuration
    {
        private static readonly IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(@"./appsettings.json")
                .Build();

        public static int BuilderConfigurations()
        {
            var minInputWordLength = Int32.Parse(configuration["Settings:minInputWordLength"]);
            return minInputWordLength;
        }

        public static string GetFileNameFromConfiguration()
        {
            var FileName = configuration["Settings:FileName"];
            return FileName;
        }

        public static string GetConnectionStringDBFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringDBFirst"];
            return connectionString;
        }

        public static string GetConnectionStringCodeFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringCodeFirst"];
            return connectionString;
        }
    }
}

