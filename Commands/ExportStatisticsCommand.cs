using CourseProject.Interfaces;
using CourseProject.Models;

namespace CourseProject.Commands
{
    /// <summary>
    /// Command Pattern: Encapsulates statistics export operation with undo support.
    /// Demonstrates: Command Pattern + Adapter Pattern working together.
    /// </summary>
    public class ExportStatisticsCommand : ICommand
    {
        private readonly StatisticsData _data;
        private readonly IDataExporter _exporter;
        private readonly string _filePath;
        private bool _wasExecuted;

        public string Description => $"Export statistics to {_exporter.FormatName}";

        public ExportStatisticsCommand(StatisticsData data, IDataExporter exporter, string filePath)
        {
            _data = data;
            _exporter = exporter;
            _filePath = filePath;
        }

        public void Execute()
        {
            _exporter.ExportToFile(_filePath, _data);
            _wasExecuted = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Statistics exported successfully to: {_filePath}");
            Console.ResetColor();
        }

        public void Undo()
        {
            if (_wasExecuted && File.Exists(_filePath))
            {
                File.Delete(_filePath);
                Console.WriteLine($"  Export file deleted: {_filePath}");
            }
        }
    }
}
