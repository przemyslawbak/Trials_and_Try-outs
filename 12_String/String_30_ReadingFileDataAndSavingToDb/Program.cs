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
            // Group by year and month, then take the first day of each month
            var filtered = records
                .GroupBy(r => new { r.Date.Year, r.Date.Month })
                .Select(g => g.OrderBy(r => r.Date).First())
                .ToList();

            // Convert to target model
            return filtered.Select(r => new DataIndexComponentHistoric
            {
                ComponentGuid = Guid.NewGuid(),
                IndexName = "S&P 500",
                ComponentsName = r.Name,
                ComponentsWeight = r.Weight,
                UtcTimeStamp = r.Date.ToUniversalTime()
            }).ToList();
        }

        private static void SaveToDatabase(List<DataIndexComponentHistoric> records)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            context.IndexComponents.AddRange(records);
            context.SaveChanges();
        }
    }

    // CSV record model
    public class CsvRecord
    {
        public DateTime Date { get; set; }
        public int Rank { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Sector { get; set; }
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
        public DbSet<DataIndexComponentHistoric> IndexComponents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here
            optionsBuilder.UseSqlServer("Your_Connection_String_Here");
        }
    }
}
