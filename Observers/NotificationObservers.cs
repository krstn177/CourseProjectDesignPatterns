using CourseProject.Interfaces;

namespace CourseProject.Observers
{
    /// <summary>
    /// Observer Pattern: Represents a user who wants to be notified
    /// when a reserved material becomes available.
    /// </summary>
    public class UserObserver : IObserver
    {
        public string UserName { get; }
        public Guid UserId { get; }
        public List<string> Notifications { get; } = new();

        public UserObserver(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public void Update(string message)
        {
            Notifications.Add(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [NOTIFICATION for {UserName}]: {message}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Observer Pattern: Subject that manages observers for a specific material.
    /// </summary>
    public class MaterialNotificationService : ISubject
    {
        private readonly List<IObserver> _observers = new();
        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);
        public void Notify(string message)
        {
            foreach (var observer in _observers.ToList())
            observer.Update(message);
        }
        public void ClearObservers() => _observers.Clear();
        public int ObserverCount => _observers.Count;
    }
}
