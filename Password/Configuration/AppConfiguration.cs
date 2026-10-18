using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Password.Configuration
{
    internal static class AppConfiguration
    {
        private static IConfiguration _configuration;


        static AppConfiguration()
        {
            var path =
                AppDomain.CurrentDomain.BaseDirectory;


            _configuration =
                new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .Build();
        }


        public static IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }
    }
}