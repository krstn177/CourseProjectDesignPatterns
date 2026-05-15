using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.Observers;
using CourseProject.Repositories;
using CourseProject.Strategies;
using CourseProject.Commands;

namespace CourseProject.Services
{
    /// <summary>
    /// Singleton Pattern: Central library service providing access to all subsystems.
    /// Demonstrates: Single Responsibility - this class only coordinates services.
    /// </summary>
    public class LibraryService
    {
    private static LibraryService? _instance;
        private static readonly object _lock = new();
        public IRepository<IMaterial> Materials { get; }
        public IRepository<User> Users { get; }
        public IRepository<BorrowRecord> BorrowRecords { get; }
        public IRepository<Reservation> Reservations { get; }
        public CommandHistory CommandHistory { get; } = new();
        public IPenaltyStrategy PenaltyStrategy { get; set; }
        public MaterialNotificationService NotificationService { get; } = new();

        private LibraryService()
        {
            Materials = new MaterialRepository("data/materials.json");
            Users = new JsonRepository<User>("data/users.json", u => u.Id);
            BorrowRecords = new JsonRepository<BorrowRecord>("data/borrows.json", b => b.Id);
            Reservations = new JsonRepository<Reservation>("data/reservations.json", r => r.Id);
            PenaltyStrategy = new FixedPenaltyStrategy();
        }

        /// <summary>
        /// Singleton accessor.
        /// </summary>
        public static LibraryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new LibraryService();
                    }
                }
                return _instance;
            }
        }

        public void SaveAll()
        {
            Materials.SaveToFile();
            Users.SaveToFile();
            BorrowRecords.SaveToFile();
            Reservations.SaveToFile();
            Console.WriteLine("  All data saved successfully.");
        }

        public void LoadAll()
        {
            Materials.LoadFromFile();
            Users.LoadFromFile();
            BorrowRecords.LoadFromFile();
            Reservations.LoadFromFile();
            Console.WriteLine("  All data loaded successfully.");
        }
    }
}
