using Bogus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using API_Controller_03_console.Models;

namespace API_Controller_03_console
{
    //config properties
    public class AppConfig
    {
        public string connectionString = ConfigurationManager.AppSettings["dbConnectionString"];
        public string connectionAPI = ConfigurationManager.AppSettings["addressAPI"];
    }
    //program exe
    class Program
    {
        //if database exists - dropped, creating database
        public static void CreateDatabase()
        {
            Console.Write("\nCreating DB... ");
            AppConfig config = new AppConfig();
            using (SqlConnection connection = new SqlConnection(config.connectionString))
            {
                //For MS SQL Server 2000, 2005, 2008
                connection.Open();
                var command = connection.CreateCommand();
                //kill all connections
                command.CommandText = "IF EXISTS (SELECT * FROM sys.databases WHERE name = 'dbAPI') BEGIN USE master; DECLARE @kill varchar(8000); SET @kill = ''; SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';' FROM master..sysprocesses WHERE dbid = db_id('dbAPI') EXEC(@kill); END";
                //execute
                command.ExecuteNonQuery();
                //drop database if exists
                command.CommandText = "IF EXISTS (SELECT * FROM sys.databases WHERE name = 'dbAPI') BEGIN DROP DATABASE dbAPI END";
                //execute
                command.ExecuteNonQuery();
                //create database
                command.CommandText = "CREATE DATABASE dbAPI";
                //info
                Console.WriteLine("OK");
                //execute
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //creating table
        public static void MigrateTable()
        {
            Console.Write("\nMigrating table... ");
            AppConfig config = new AppConfig();
            using (SqlConnection connection = new SqlConnection(config.connectionString))
            {
                //For MS SQL Server 2000, 2005, 2008
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE dbAPI.dbo.Requests(
                [Index] INT NOT NULL PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                Visits INT NULL,
                Date DATETIME NOT NULL
            )";
                //execute
                command.ExecuteNonQuery();
                connection.Close();
                //info
                Console.WriteLine("OK");
            }
        }
        //creating JSON object
        public static string SerializeData(List<RequestModel> data)
        {
            var returnMe = JsonConvert.SerializeObject(data);
            return returnMe;
        }
        //sending JSON data to the API
        public static async Task SendingPostRequest(string data)
        {
            Console.Write("\nSending data to the API... ");
            AppConfig config = new AppConfig();
            //http requests
            HttpClient client = new HttpClient();
            //API address
            client.BaseAddress = new Uri(config.connectionAPI);
            using (client)
            {
                var response = await client.PostAsync(
                    "api/data",
                     new StringContent(data, Encoding.UTF8, "application/json"));
            }
            Console.WriteLine("OK");
        }
        //sends GET request to the API
        public static async Task SendingGetRequest()
        {
            Console.Write("\nSending GET request... ");
            AppConfig config = new AppConfig();
            //http requests
            HttpClient client = new HttpClient();
            //API address
            client.BaseAddress = new Uri(config.connectionAPI);
            using (client)
            {
                var response = await client.GetAsync("api/jobs/saveFiles");
            }
            Console.WriteLine("OK");
        }
        //take qty of records from the user
        public static int EnterTheQty()
        {
            int number;
            bool check;
            do
            {
                //msg
                Console.WriteLine("\nHow many records you do want to generate? (Choose int > 0)");
                //parsing
                check = int.TryParse(Console.ReadLine(), out number);
            }
            //validation
            while ((!check) || (number <= 0));
            Console.Write("\nGenerating {0} models... ", number);
            Console.WriteLine("OK");
            return number;
        }
        //getting random data and fillig up the list
        public static List<RequestModel> GenerateRandomData(int qty)
        {
            List<RequestModel> items = new List<RequestModel>();
            var faker = new Faker("en");
            for(int i=0; i < qty; i++)
            {
                var newReservation = new RequestModel()
                {
                    //Index value plus one
                    Index = i + 1,
                    //Name value randomized
                    Name = faker.Name.FirstName(),
                    //Visits values 0 to 1 - for no visits 0, parsed below to null (for XML task),
                    Visits = faker.Random.Number(0, 1),
                    //Date value randomized 10 y back
                    Date = faker.Date.Past(yearsToGoBack: 10)
                };
                //making zeroes to null
                if (newReservation.Visits == 0) newReservation.Visits = null;
                //add object
                items.Add(newReservation);
            }
            return items;
        }
        public static void WriteHeader()
        {
            Console.Title = "XML Serializer";
            string title = @" __  ____  __ _      ___ ___ ___ ___   _   _    ___ _______ ___ 
 \ \/ /  \/  | |    / __| __| _ \_ _| /_\ | |  |_ _|_  / __| _ \
  >  <| |\/| | |__  \__ \ _||   /| | / _ \| |__ | | / /| _||   /
 /_/\_\_|  |_|____| |___/___|_|_\___/_/ \_\____|___/___|___|_|_\

    console v. 1.0 - by Przemyslaw Bak
";
            Console.WriteLine(title);

        }
        //egzecuting program
        static void Main(string[] args)
        {
            //header
            WriteHeader();
            //taking qty of records (not specified how many in project specs)
            int qtyOfRecords = EnterTheQty();
            //creating qtyOfRecords records
            var dataCollection = GenerateRandomData(qtyOfRecords);
            //data serialization
            var serializedData = SerializeData(dataCollection);
            //creates DB if not exists
            CreateDatabase();
            //creates table - spr czy istnieje
            MigrateTable();
            //sending data to the API
            SendingPostRequest(serializedData).Wait();
            //sending request to save XML files
            SendingGetRequest().Wait();
            Console.WriteLine("\nPress any key to finish.");
            Console.ReadKey();
        }

    }
}
