namespace CourseProject.Interfaces
{
    /// <summary>
    /// Repository interface for data access abstraction.
    /// Demonstrates: Dependency Inversion Principle.
    /// </summary>
    public interface IRepository<T>
    {
        List<T> GetAll();
        T? GetById(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Remove(Guid id);
        void SaveToFile();
        void LoadFromFile();
    }
}
