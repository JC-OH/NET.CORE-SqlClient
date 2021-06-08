using Microsoft.Extensions.Configuration;
using SqlClient.DataAccessLayer;
using System;
using System.IO;

namespace SqlClient
{
    class Program
    {
        private static IConfiguration _configuration;
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            GetAppSettingsFile();
            PrintCountries();
        }

        private static void PrintCountries()
        {
            var countryDAL = new CountryDAL(_configuration);
            var listCountryModel = countryDAL.GetList();

            listCountryModel.ForEach(item =>
            {
                Console.WriteLine(item.Country);
            });

            Console.WriteLine("Press any key to stop.");
            Console.ReadKey();
        }

        private static void GetAppSettingsFile()
        {

            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
            //Console.WriteLine(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
