using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Rental.Models;
using System.Data.SqlClient;

namespace Sample
{
    internal class Program
    {
        private static readonly string _dataPath = Path.GetFullPath(@"..\..\..\Data\DailyDemand.mdf");
        private static readonly string _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dataPath};Integrated Security=True;Connect Timeout=30;";

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            //load from mdf file
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<RentalData>();
            var query = "SELECT RentalDate, CAST(Year as REAL) as Year, CAST(TotalRentals as REAL) as TotalRentals FROM Rentals";
            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, _connectionString, query);
            IDataView dataView = loader.Load(dbSource);
        }
    }
}