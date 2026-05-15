namespace CourseProject.Interfaces
{
    /// <summary>
    /// Observer Pattern - subject that manages observers.
    /// </summary>
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }
}
