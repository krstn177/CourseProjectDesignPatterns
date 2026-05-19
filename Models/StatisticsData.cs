namespace CourseProject.Models
{
    /// <summary>
    /// Data transfer object for statistics export.
    /// Encapsulates all statistical information in a format-agnostic structure.
    /// </summary>
    public class StatisticsData
    {
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public int TotalMaterials { get; set; }
        public int AvailableMaterials { get; set; }
        public int BorrowedMaterials { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalPenalties { get; set; }

        public List<MaterialStatistic> TopBorrowedMaterials { get; set; } = new();
        public List<UserStatistic> MostActiveUsers { get; set; } = new();
    }

    public class MaterialStatistic
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class UserStatistic
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int ActiveBorrows { get; set; }
        public decimal PenaltyBalance { get; set; }
    }
}
