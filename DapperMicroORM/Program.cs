using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using DapperMicroORM.Model;
using Microsoft.Extensions.Configuration;

namespace DapperMicroORM
{
    class Program
    {
        private static IConfiguration _configuration;
        static void Main(string[] args)
        {
            GetAppSettingFile();

            //Console.WriteLine(_configuration.GetConnectionString("DefaultConnection"));
            var countryId = InsertCountry("Maxico");
            PrintCountries();
            UpdateCountry(countryId);
            PrintCountries();
            DeleteCountry(countryId);
            PrintCountries();
        }

        private static void UpdateCountry(long? countryId)
        {
            if (countryId != null)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var country = db.QueryFirstOrDefault<CountryModel>("SELECT * FROM tb_country WHERE Id = @Id", new { Id = countryId });

                    if (country != null)
                    {
                        country.Country = "Updated Maxico";
                        var result = db.Update(country);

                        Console.WriteLine($"Updated: {result}, {countryId}");
                    }
                }
            }
        }

        private static void DeleteCountry(long? countryId)
        {

            if (countryId != null)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var result = db.Delete(new CountryModel()
                    {
                        Id = (int)countryId
                    });

                    Console.WriteLine($"Deleted: {result}, {countryId}");
                }
            }
        }

        private static long? InsertCountry(String name)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //var country = db.Query<CountryModel>("SELECT * FROM tb_country WHERE country = @Country", new { Country = name }).FirstOrDefault();
                var country = db.QueryFirstOrDefault<CountryModel>("SELECT * FROM tb_country WHERE country = @Country", new { Country = name });
                
                
                if (country == null)
                {
                    db.Open();
                    var countryId =  db.Insert(new CountryModel { 
                        Country = name,
                        Active = true
                    });

                    Console.WriteLine($"Inserted: {countryId}");
                    return countryId;
                }
            };

            return null;

        }

        private static void PrintCountries()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            //var countries = new List<CountryModel>();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<CountryModel>("Select * From  tb_country").ToList();
                foreach (var country in countries)
                {
                    Console.WriteLine(country.Country.ToString());
                }
            };
        }

        private static void GetAppSettingFile()
        {
            var builder = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }
    }
}
