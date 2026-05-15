namespace CourseProject.Interfaces
{
    /// <summary>
    /// Factory Method pattern interface for creating materials.
    /// Demonstrates: Open/Closed Principle - new factories can be added without modifying existing code.
    /// </summary>
    public interface IMaterialFactory
    {
        IMaterial Create(string title, string author, string category, int year);
    }
}
