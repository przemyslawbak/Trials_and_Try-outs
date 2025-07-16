using System.Globalization;

namespace SampleCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Load the data from both files
                var octoberData = LoadData("october_2024.txt");
                var julyData = LoadData("july_2025.txt");

                // Get all unique components from both files
                var allComponents = octoberData.Keys.Union(julyData.Keys).OrderBy(c => c).ToList();

                // Define the date range
                var startDate = new DateTime(2024, 10, 1);
                var endDate = new DateTime(2025, 7, 1);

                // Create output directory if it doesn't exist
                Directory.CreateDirectory("MonthlyWeights");

                // Process each month in the range
                for (var date = startDate; date <= endDate; date = date.AddMonths(1))
                {
                    // Calculate the interpolation factor (0 = October, 1 = July)
                    double factor = CalculateInterpolationFactor(date, startDate, endDate);

                    // Interpolate weights for this month
                    var interpolatedWeights = InterpolateWeights(octoberData, julyData, allComponents, factor);

                    // Generate the CSV file
                    string fileName = $"MonthlyWeights/{date:yyyy_MM}.csv";
                    SaveToCsv(interpolatedWeights, fileName, date);

                    Console.WriteLine($"Generated: {fileName}");
                }

                Console.WriteLine("All monthly files generated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static Dictionary<string, double> LoadData(string filePath)
        {
            var data = new Dictionary<string, double>();

            foreach (var line in File.ReadAllLines(filePath).Skip(1)) // Skip header
            {
                var parts = line.Split('\t');
                if (parts.Length >= 3)
                {
                    var component = parts[2];
                    if (double.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var weight))
                    {
                        data[component] = weight;
                    }
                }
            }

            return data;
        }

        static double CalculateInterpolationFactor(DateTime currentDate, DateTime startDate, DateTime endDate)
        {
            if (currentDate <= startDate) return 0;
            if (currentDate >= endDate) return 1;

            double totalDays = (endDate - startDate).TotalDays;
            double elapsedDays = (currentDate - startDate).TotalDays;

            return elapsedDays / totalDays;
        }

        static Dictionary<string, double> InterpolateWeights(
            Dictionary<string, double> startWeights,
            Dictionary<string, double> endWeights,
            List<string> allComponents,
            double factor)
        {
            var result = new Dictionary<string, double>();

            foreach (var component in allComponents)
            {
                double startWeight = startWeights.TryGetValue(component, out var sw) ? sw : 0;
                double endWeight = endWeights.TryGetValue(component, out var ew) ? ew : 0;

                // Linear interpolation
                double interpolatedWeight = startWeight + (endWeight - startWeight) * factor;
                result[component] = interpolatedWeight;
            }

            return result;
        }

        static void SaveToCsv(Dictionary<string, double> weights, string filePath, DateTime date)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Component,Weight");

                foreach (var kvp in weights.OrderByDescending(kvp => kvp.Value))
                {
                    writer.WriteLine($"{kvp.Key},{kvp.Value.ToString("0.0000", CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}
