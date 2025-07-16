using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SampleCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Read and parse CSV data
                var records = ReadCsvFile("sp500_historical.csv");

                // Filter for first day of each month
                var filteredRecords = FilterFirstDayOfMonth(records);

                // Save to database
                SaveToDatabase(filteredRecords);

                Console.WriteLine("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static List<CsvRecord> ReadCsvFile(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<CsvRecord>().ToList();
        }

        private static List<DataIndexComponentHistoric> FilterFirstDayOfMonth(List<CsvRecord> records)
        {
            var toReturn = new List<DataIndexComponentHistoric>();

            // Group by year and month, then take the first day of each month
            var filteredByMonth = records
                .GroupBy(r => new { r.Date.Year, r.Date.Month })
                .ToList();

            foreach (var record in filteredByMonth)
            {
                var firstDateItem = record.OrderBy(x => x.Date).First();
                var date = firstDateItem.Date;
                var firstDateMultipleItems = record.Where(x => x.Date == date);
                var toAdd = firstDateMultipleItems.Select(r => new DataIndexComponentHistoric
                {
                    ComponentGuid = Guid.NewGuid(),
                    IndexName = "SPX",
                    ComponentsName = r.Ticker,
                    ComponentsWeight = r.Weight,
                    UtcTimeStamp = r.Date
                });

                toReturn.AddRange(toAdd);
            }

            return toReturn;
        }

        private static void SaveToDatabase(List<DataIndexComponentHistoric> records)
        {
            var secret = new Secret();
            using var context = new AppDbContext(secret.ConnectionString);
            context.Database.EnsureCreated();

            context.ComponentsHistoric.AddRange(records);
            context.SaveChanges();
        }
    }

    // CSV record model
    public class CsvRecord
    {
        public DateTime Date { get; set; }
        public decimal Rank { get; set; }
        public string Ticker { get; set; }
        public decimal Weight { get; set; }
    }

    // Database model
    public class DataIndexComponentHistoric
    {
        [Key]
        public Guid ComponentGuid { get; set; }
        public string IndexName { get; set; }
        public string ComponentsName { get; set; }
        public decimal ComponentsWeight { get; set; }
        public DateTime UtcTimeStamp { get; set; }
    }

    // DbContext
    public class AppDbContext : DbContext
    {
        private string connectionString;

        public AppDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<DataIndexComponentHistoric> ComponentsHistoric { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
