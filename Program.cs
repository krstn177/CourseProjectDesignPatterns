using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Factories;
using CourseProject.Commands;
using CourseProject.States;
using CourseProject.Strategies;
using CourseProject.Observers;
using CourseProject.Utilities;
using CourseProject.Adapters;

namespace CourseProject
{
    /// <summary>
    /// SmartLibrarySystem - Demonstrates OOP, SOLID Principles, and Design Patterns.
    /// Design Patterns Used: Factory Method, Singleton, Observer, Strategy, Command, State.
    /// </summary>
    internal class Program
    {
        private static readonly LibraryService _library = LibraryService.Instance;
        private static readonly StatisticsService _stats = new(LibraryService.Instance);

        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║        SMART LIBRARY MANAGEMENT SYSTEM       ║");
            Console.WriteLine("║       OOP  SOLID  Design Patterns Demo       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝\n");

            // Load existing data if available
            _library.LoadAll();

            bool running = true;
            while (running)
            {
                DisplayMainMenu();
                int choice = ConsoleHelper.ReadMenuChoice(1, 11);
                Console.WriteLine();

                switch (choice)
                {
                    case 1: ManageMaterials(); break;
                    case 2: ManageUsers(); break;
                    case 3: BorrowMaterial(); break;
                    case 4: ReturnMaterial(); break;
                    case 5: ReserveMaterial(); break;
                    case 6: _stats.DisplayStatistics(); break;
                    case 7: ExportStatistics(); break;
                    case 8: UndoLastOperation(); break;
                    case 9: _library.SaveAll(); break;
                    case 10: _library.LoadAll(); break;
                    case 11: running = false; _library.SaveAll(); break;
                }
            }

            Console.WriteLine("\nGoodbye!");
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\n┌──────────── MAIN MENU ──────────────┐");
            Console.WriteLine("│  1. Manage Materials                │");
            Console.WriteLine("│  2. Manage Users                    │");
            Console.WriteLine("│  3. Borrow Material                 │");
            Console.WriteLine("│  4. Return Material                 │");
            Console.WriteLine("│  5. Reserve Material                │");
            Console.WriteLine("│  6. View Statistics                 │");
            Console.WriteLine("│  7. Export Statistics               │");
            Console.WriteLine("│  8. Undo Last Operation             │");
            Console.WriteLine("│  9. Save Data                       │");
            Console.WriteLine("│  10. Load Data                      │");
            Console.WriteLine("│  11. Exit                           │");
            Console.WriteLine("└─────────────────────────────────────┘");
        }

        // ─── MATERIAL MANAGEMENT ───
        static void ManageMaterials()
        {
            Console.WriteLine("  1. Add Material");
            Console.WriteLine("  2. Edit Material");
            Console.WriteLine("  3. Remove Material");
            Console.WriteLine("  4. Search Material");
            Console.WriteLine("  5. List All Materials");
            int choice = ConsoleHelper.ReadMenuChoice(1, 5);

            switch (choice)
            {
                case 1: AddMaterial(); break;
                case 2: EditMaterial(); break;
                case 3: RemoveMaterial(); break;
                case 4: SearchMaterial(); break;
                case 5: ListMaterials(); break;
            }
        }

        static void AddMaterial()
        {
            Console.WriteLine($"  Available types: {string.Join(", ", MaterialFactoryProvider.AvailableTypes)}");
            string type = ConsoleHelper.ReadNonEmpty("  Type: ");
            string title = ConsoleHelper.ReadNonEmpty("  Title: ");
            string author = ConsoleHelper.ReadNonEmpty("  Author: ");
            string category = ConsoleHelper.ReadNonEmpty("  Category: ");
            int year = ConsoleHelper.ReadInt("  Year: ", 1000, DateTime.Now.Year);

            try
            {
                var factory = MaterialFactoryProvider.GetFactory(type);
                var material = factory.Create(title, author, category, year);
                var command = new AddMaterialCommand(material, _library.Materials);
                _library.CommandHistory.ExecuteCommand(command);
                Console.WriteLine($"  Material added with ID: {material.Id}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Error: {ex.Message}");
            }
        }

        static void EditMaterial()
        {
            ListMaterials();
            var id = ConsoleHelper.ReadGuid("  Enter Material ID to edit");
            if (id == Guid.Empty) return; // User cancelled

            var material = _library.Materials.GetById(id);
            if (material == null) { Console.WriteLine("  Material not found."); return; }

            Console.WriteLine($"  Current Title: {material.Title}");
            string title = ConsoleHelper.ReadNonEmpty("  New Title (or same): ");
            string author = ConsoleHelper.ReadNonEmpty("  New Author (or same): ");
            string category = ConsoleHelper.ReadNonEmpty("  New Category (or same): ");
            int year = ConsoleHelper.ReadInt("  New Year: ", 1000, DateTime.Now.Year);

            material.Title = title;
            material.Author = author;
            material.Category = category;
            material.Year = year;
            _library.Materials.Update(material);
            Console.WriteLine("  Material updated.");
        }

        static void RemoveMaterial()
        {
            ListMaterials();
            var id = ConsoleHelper.ReadGuid("  Enter Material ID to remove");
            if (id == Guid.Empty) return; // User cancelled

            var material = _library.Materials.GetById(id);
            if (material == null) { Console.WriteLine("  Material not found."); return; }

            var command = new RemoveMaterialCommand(material, _library.Materials);
            _library.CommandHistory.ExecuteCommand(command);
        }

        static void SearchMaterial()
        {
            string query = ConsoleHelper.ReadNonEmpty("  Search (title/author/category): ");
            var results = _library.Materials.GetAll()
                .Where(m => m.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                        || m.Author.Contains(query, StringComparison.OrdinalIgnoreCase)
                        || m.Category.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0) { Console.WriteLine("  No results found."); return; }
            foreach (var m in results)
                Console.WriteLine($"    {m.Id} | {m}");
        }

        static void ListMaterials()
        {
            var all = _library.Materials.GetAll();
            if (all.Count == 0) { Console.WriteLine("  No materials in library."); return; }
            Console.WriteLine($"\n  {"ID",-38} {"Type",-12} {"Title",-25} {"Author",-20} {"State",-10}");
            Console.WriteLine("  " + new string('-', 105));
            foreach (var m in all)
                Console.WriteLine($"  {m.Id,-38} {m.MaterialType,-12} {m.Title,-25} {m.Author,-20} {m.State.StateName,-10}");
        }

        //USER MANAGEMENT
        static void ManageUsers()
        {
            Console.WriteLine("  1. Add User");
            Console.WriteLine("  2. Edit User");
            Console.WriteLine("  3. Remove User");
            Console.WriteLine("  4. View User Info");
            Console.WriteLine("  5. List All Users");
            int choice = ConsoleHelper.ReadMenuChoice(1, 5);

            switch (choice)
            {
                case 1: AddUser(); break;
                case 2: EditUser(); break;
                case 3: RemoveUser(); break;
                case 4: ViewUser(); break;
                case 5: ListUsers(); break;
            }
        }

        static void AddUser()
        {
            string name = ConsoleHelper.ReadNonEmpty("  Name: ");
            Console.WriteLine("  Roles: Student, Teacher, Guest, Admin");
            string role = ConsoleHelper.ReadNonEmpty("  Role: ");
            var user = new User { Name = name, Role = role };
            _library.Users.Add(user);
            Console.WriteLine($"  User added with ID: {user.Id}");
        }

        static void EditUser()
        {
            ListUsers();
            var id = ConsoleHelper.ReadGuid("  Enter User ID to edit");
            if (id == Guid.Empty) return; // User cancelled

            var user = _library.Users.GetById(id);
            if (user == null) { Console.WriteLine("  User not found."); return; }

            string name = ConsoleHelper.ReadNonEmpty("  New Name: ");
            string role = ConsoleHelper.ReadNonEmpty("  New Role: ");
            user.Name = name;
            user.Role = role;
            _library.Users.Update(user);
            Console.WriteLine("  User updated.");
        }

        static void RemoveUser()
        {
            ListUsers();
            var id = ConsoleHelper.ReadGuid("  Enter User ID to remove");
            if (id == Guid.Empty) return; // User cancelled

            _library.Users.Remove(id);
            Console.WriteLine("  User removed.");
        }

        static void ViewUser()
        {
            ListUsers();
            var id = ConsoleHelper.ReadGuid("  Enter User ID");
            if (id == Guid.Empty) return; // User cancelled

            var user = _library.Users.GetById(id);
            if (user == null) { Console.WriteLine("  User not found."); return; }
            Console.WriteLine($"  {user}");
            Console.WriteLine($"  Borrowed IDs: {string.Join(", ", user.BorrowedMaterialIds)}");
        }

        static void ListUsers()
        {
            var all = _library.Users.GetAll();
            if (all.Count == 0) { Console.WriteLine("  No users registered."); return; }
            foreach (var u in all)
                Console.WriteLine($"    {u.Id} | {u}");
        }

        //BORROW
        static void BorrowMaterial()
        {
            ListUsers();
            var userId = ConsoleHelper.ReadGuid("  Enter User ID");
            if (userId == Guid.Empty) return; // User cancelled

            var user = _library.Users.GetById(userId);
            if (user == null) { Console.WriteLine("  User not found."); return; }

            if (user.BorrowedMaterialIds.Count >= user.BorrowLimit)
            {
                Console.WriteLine($"  User has reached borrow limit ({user.BorrowLimit}).");
                return;
            }

            ListMaterials();
            var materialId = ConsoleHelper.ReadGuid("  Enter Material ID to borrow");
            if (materialId == Guid.Empty) return; // User cancelled

            var material = _library.Materials.GetById(materialId);
            if (material == null) { Console.WriteLine("  Material not found."); return; }

            if (!material.IsAvailable)
            {
                Console.WriteLine("  Material is not available. Use Reserve option instead.");
                return;
            }

            var command = new BorrowCommand(material, user, _library.BorrowRecords);
            _library.CommandHistory.ExecuteCommand(command);
        }

        //RETURN
        static void ReturnMaterial()
        {
            ListUsers();
            var userId = ConsoleHelper.ReadGuid("  Enter User ID");
            if (userId == Guid.Empty) return; // User cancelled

            var user = _library.Users.GetById(userId);
            if (user == null) { Console.WriteLine("  User not found."); return; }

            if (user.BorrowedMaterialIds.Count == 0)
            {
                Console.WriteLine("  User has no borrowed materials.");
                return;
            }

            Console.WriteLine("  Borrowed materials:");
            foreach (var mid in user.BorrowedMaterialIds)
            {
                var m = _library.Materials.GetById(mid);
                if (m != null) Console.WriteLine($"    {m.Id} | {m.Title}");
            }

            var materialId = ConsoleHelper.ReadGuid("  Enter Material ID to return");
            if (materialId == Guid.Empty) return; // User cancelled

            var material = _library.Materials.GetById(materialId);
            if (material == null) { Console.WriteLine("  Material not found."); return; }

            // Check for late return penalty
            var record = _library.BorrowRecords.GetAll()
                .FirstOrDefault(r => r.MaterialId == materialId && r.UserId == userId && !r.IsReturned);
            if (record != null && DateTime.Now > record.DueDate)
            {
                int daysLate = (DateTime.Now - record.DueDate).Days;
                decimal penalty = _library.PenaltyStrategy.CalculatePenalty(daysLate, user);
                user.PenaltyBalance += penalty;
                Console.WriteLine($"  Late by {daysLate} days! Penalty: ${penalty:F2} (Strategy: {_library.PenaltyStrategy.StrategyName})");
            }

            var command = new ReturnCommand(material, user, _library.BorrowRecords, _library.NotificationService);
            _library.CommandHistory.ExecuteCommand(command);
        }

        //RESERVE
        static void ReserveMaterial()
        {
            ListUsers();
            var userId = ConsoleHelper.ReadGuid("  Enter User ID");
            if (userId == Guid.Empty) return; // User cancelled

            var user = _library.Users.GetById(userId);
            if (user == null) { Console.WriteLine("  User not found."); return; }

            ListMaterials();
            var materialId = ConsoleHelper.ReadGuid("  Enter Material ID to reserve");
            if (materialId == Guid.Empty) return; // User cancelled

            var material = _library.Materials.GetById(materialId);
            if (material == null) { Console.WriteLine("  Material not found."); return; }

            if (material.IsAvailable)
            {
                Console.WriteLine("  Material is available - you can borrow it directly.");
                return;
            }

            // Add reservation
            var reservation = new Reservation { UserId = userId, MaterialId = materialId };
            _library.Reservations.Add(reservation);

            // Register observer for notification
            var observer = new UserObserver(user.Id, user.Name);
            _library.NotificationService.Attach(observer);

            Console.WriteLine($"  Reservation created. You will be notified when '{material.Title}' is available.");
        }

        // ─── EXPORT STATISTICS ───
        static void ExportStatistics()
        {
            Console.WriteLine("  Select export format:");
            Console.WriteLine("  1. CSV (Comma-Separated Values)");
            Console.WriteLine("  2. JSON (JavaScript Object Notation)");
            int formatChoice = ConsoleHelper.ReadMenuChoice(1, 2);

            IDataExporter exporter = formatChoice switch
            {
                1 => new CsvDataExporter(),
                2 => new JsonDataExporter(),
                _ => new CsvDataExporter()
            };

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"library_statistics_{timestamp}{exporter.FileExtension}";
            string filePath = Path.Combine("exports", fileName);

            // Generate statistics data
            var statsData = _stats.GenerateStatisticsData();

            // Create and execute export command
            var command = new ExportStatisticsCommand(statsData, exporter, filePath);
            _library.CommandHistory.ExecuteCommand(command);
        }

        // ─── UNDO ───
        static void UndoLastOperation()
        {
            if (!_library.CommandHistory.Undo())
                Console.WriteLine("  No operations to undo.");
        }
    }
}
