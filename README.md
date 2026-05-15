# SmartLibrarySystem

A C# Console Application demonstrating **Object-Oriented Programming**, **SOLID Principles**, and **Design Patterns**.

## Design Patterns Used

| Pattern | Location | Purpose |
|---------|----------|---------|
| **Factory Method** | `Factories/MaterialFactories.cs` | Create different material types (Book, Magazine, Ebook, Audiobook) without modifying existing code |
| **Singleton** | `Services/LibraryService.cs` | Single shared library service instance across the application |
| **Observer** | `Observers/NotificationObservers.cs` | Notify users when reserved materials become available |
| **Strategy** | `Strategies/PenaltyStrategies.cs` | Interchangeable penalty calculation algorithms (Fixed, Percentage, RoleBased) |
| **Command** | `Commands/LibraryCommands.cs` | Encapsulate operations (Borrow, Return, Add, Remove) with undo support |
| **State** | `States/MaterialStates.cs` | Represent material lifecycle (Available, Borrowed, Reserved, Lost) |

## SOLID Principles

| Principle | Implementation |
|-----------|---------------|
| **S** - Single Responsibility | Each service handles one concern (LibraryService, StatisticsService, ConsoleHelper) |
| **O** - Open/Closed | New material types added via new factory classes without modifying existing code |
| **L** - Liskov Substitution | All materials work through `IMaterial` interface interchangeably |
| **I** - Interface Segregation | Small focused interfaces (IMaterial, ICommand, IPenaltyStrategy, IObserver) |
| **D** - Dependency Inversion | Services depend on `IRepository<T>` abstraction, not concrete implementations |

## Project Structure

```
CourseProject/
??? Interfaces/          - All abstractions
??? Models/   - Domain entities (Material types, User, BorrowRecord, Reservation)
??? States/              - State pattern implementations
??? Factories/     - Factory method pattern implementations
??? Commands/  - Command pattern with undo support
??? Strategies/          - Penalty calculation strategies
??? Observers/        - Observer pattern for notifications
??? Repositories/        - JSON-based data persistence
??? Services/   - Business logic (Singleton LibraryService, StatisticsService)
??? Utilities/  - Console input helpers
??? Program.cs   - Main menu and application flow
```

## Features

- Material CRUD (Book, Magazine, Ebook, Audiobook)
- User management with roles (Student, Teacher, Guest, Admin)
- Borrowing with role-based limits
- Reservation system with notifications
- Penalty system with swappable strategies
- Undo functionality via command history
- Statistics display
- JSON file persistence

## How to Run

```bash
dotnet run
```

Data is saved to/loaded from the `data/` folder as JSON files.
