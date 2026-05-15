using CourseProject.Interfaces;
using CourseProject.States;

namespace CourseProject.Models
{
    /// <summary>
    /// Base class for all library materials.
    /// Demonstrates: Liskov Substitution Principle - derived types can substitute base.
    /// </summary>
    public abstract class MaterialBase : IMaterial
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        public int BorrowCount { get; set; }
        public IMaterialState State { get; set; } = new AvailableState();
        public abstract string MaterialType { get; }
        public bool IsAvailable => State.CanBorrow();
        public override string ToString() => $"[{MaterialType}] {Title} by {Author} ({Year}) - {State.StateName}";
    }

    public class Book : MaterialBase
    {
        public override string MaterialType => "Book";
    }

    public class Magazine : MaterialBase
    {
        public override string MaterialType => "Magazine";
    }

    public class Ebook : MaterialBase
    {
        public override string MaterialType => "Ebook";
    }

    public class Audiobook : MaterialBase
    {
        public override string MaterialType => "Audiobook";
    }
}
