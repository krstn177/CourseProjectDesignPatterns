namespace CourseProject.Interfaces
{
    /// <summary>
    /// Core abstraction for all library materials.
    /// Demonstrates: Liskov Substitution Principle - all material types work through this interface.
    /// Demonstrates: Interface Segregation Principle - focused on material properties only.
    /// </summary>
    public interface IMaterial
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        string Category { get; set; }
        int Year { get; set; }
        int BorrowCount { get; set; }
        IMaterialState State { get; set; }
        string MaterialType { get; }
        bool IsAvailable { get; }
    }
}
