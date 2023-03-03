using System;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    class Configuration
    {
        public bool? TestMode { get; set; }
        public int? MillisecondsBetweenMoves { get; set; }
        public int? Rounds { get; set; }
        public int? RandomSeed { get; set; }

        public static Configuration ReadConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", false, true)
                .Build();

            return configurationBuilder.GetSection("Configuration").Get<Configuration>();
        }
    }
}
