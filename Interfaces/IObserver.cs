namespace CourseProject.Interfaces
{
    /// <summary>
    /// Observer Pattern interface - observers get notified when materials become available.
    /// </summary>
    public interface IObserver
    {
        void Update(string message);
    }
}
