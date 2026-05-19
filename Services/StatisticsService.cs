using CourseProject.Interfaces;
using CourseProject.Models;

namespace CourseProject.Services
{
    /// <summary>
    /// Statistics service - Single Responsibility Principle.
    /// </summary>
    public class StatisticsService
    {
        private readonly LibraryService _library;

        public StatisticsService(LibraryService library)
        {
            _library = library;
        }

        public void DisplayStatistics()
        {
            var materials = _library.Materials.GetAll();
            var users = _library.Users.GetAll();
            var borrows = _library.BorrowRecords.GetAll();

            Console.WriteLine("\n══════════════ LIBRARY STATISTICS ══════════════");

            // Most borrowed materials
            Console.WriteLine("\n  Top 5 Most Borrowed Materials:");
            var topMaterials = materials.OrderByDescending(m => m.BorrowCount).Take(5);
            foreach (var m in topMaterials)
            Console.WriteLine($"    - {m.Title} (Borrowed {m.BorrowCount} times)");

            // Active users
            Console.WriteLine("\n  Most Active Users:");
            var activeUsers = users.OrderByDescending(u => u.BorrowedMaterialIds.Count).Take(5);
            foreach (var u in activeUsers)
            Console.WriteLine($"    - {u.Name} ({u.BorrowedMaterialIds.Count} active borrows)");

            // Availability stats
            int available = materials.Count(m => m.IsAvailable);
            int borrowed = materials.Count - available;
            Console.WriteLine($"\n  Available: {available} | Borrowed: {borrowed} | Total: {materials.Count}");

            // Total penalties
            decimal totalPenalties = users.Sum(u => u.PenaltyBalance);
            Console.WriteLine($"  Total Penalties Collected: ${totalPenalties:F2}");

            Console.WriteLine("═══════════════════════════════════════════════\n");
        }

        /// <summary>
        /// Generates statistics data in a format-agnostic structure for export.
        /// </summary>
        public StatisticsData GenerateStatisticsData()
        {
            var materials = _library.Materials.GetAll();
            var users = _library.Users.GetAll();

            return new StatisticsData
            {
                TotalMaterials = materials.Count,
                AvailableMaterials = materials.Count(m => m.IsAvailable),
                BorrowedMaterials = materials.Count(m => !m.IsAvailable),
                TotalUsers = users.Count,
                TotalPenalties = users.Sum(u => u.PenaltyBalance),

                TopBorrowedMaterials = materials
                    .OrderByDescending(m => m.BorrowCount)
                    .Take(10)
                    .Select(m => new MaterialStatistic
                    {
                        Title = m.Title,
                        Author = m.Author,
                        Type = m.MaterialType,
                        BorrowCount = m.BorrowCount
                    })
                    .ToList(),

                MostActiveUsers = users
                    .OrderByDescending(u => u.BorrowedMaterialIds.Count)
                    .Take(10)
                    .Select(u => new UserStatistic
                    {
                        Name = u.Name,
                        Role = u.Role,
                        ActiveBorrows = u.BorrowedMaterialIds.Count,
                        PenaltyBalance = u.PenaltyBalance
                    })
                    .ToList()
            };
        }
    }
}
