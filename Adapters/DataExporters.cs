using System.Text;
using CourseProject.Interfaces;
using CourseProject.Models;

namespace CourseProject.Adapters
{
    /// <summary>
    /// Adapter Pattern: Adapts statistics data to CSV file format.
    /// Demonstrates: Adapter Pattern - converts internal data structure to external CSV format.
    /// </summary>
    public class CsvDataExporter : IDataExporter
    {
        public string FileExtension => ".csv";
        public string FormatName => "CSV (Comma-Separated Values)";

        public void ExportToFile(string filePath, object data)
        {
            if (data is not StatisticsData stats)
                throw new ArgumentException("Data must be of type StatisticsData", nameof(data));

            var csv = new StringBuilder();

            // Header section
            csv.AppendLine("SMART LIBRARY SYSTEM - STATISTICS REPORT");
            csv.AppendLine($"Generated At,{stats.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            csv.AppendLine();

            // Summary section
            csv.AppendLine("SUMMARY");
            csv.AppendLine("Metric,Value");
            csv.AppendLine($"Total Materials,{stats.TotalMaterials}");
            csv.AppendLine($"Available Materials,{stats.AvailableMaterials}");
            csv.AppendLine($"Borrowed Materials,{stats.BorrowedMaterials}");
            csv.AppendLine($"Total Users,{stats.TotalUsers}");
            csv.AppendLine($"Total Penalties Collected,${stats.TotalPenalties:F2}");
            csv.AppendLine();

            // Top borrowed materials section
            csv.AppendLine("TOP BORROWED MATERIALS");
            csv.AppendLine("Title,Author,Type,Borrow Count");
            foreach (var material in stats.TopBorrowedMaterials)
            {
                csv.AppendLine($"\"{EscapeCsv(material.Title)}\",\"{EscapeCsv(material.Author)}\",{material.Type},{material.BorrowCount}");
            }
            csv.AppendLine();

            // Most active users section
            csv.AppendLine("MOST ACTIVE USERS");
            csv.AppendLine("Name,Role,Active Borrows,Penalty Balance");
            foreach (var user in stats.MostActiveUsers)
            {
                csv.AppendLine($"\"{EscapeCsv(user.Name)}\",{user.Role},{user.ActiveBorrows},${user.PenaltyBalance:F2}");
            }

            // Ensure directory exists and write file
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Escape double quotes by doubling them
            return value.Replace("\"", "\"\"");
        }
    }

    /// <summary>
    /// Adapter Pattern: Adapts statistics data to JSON file format.
    /// </summary>
    public class JsonDataExporter : IDataExporter
    {
        public string FileExtension => ".json";
        public string FormatName => "JSON (JavaScript Object Notation)";

        public void ExportToFile(string filePath, object data)
        {
            if (data is not StatisticsData stats)
                throw new ArgumentException("Data must be of type StatisticsData", nameof(data));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(stats, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
    }
}
