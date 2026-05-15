using CourseProject.Interfaces;

namespace CourseProject.States
{
    /// <summary>
    /// State Pattern: Material is available for borrowing.
    /// </summary>
    public class AvailableState : IMaterialState
    {
        public string StateName => "Available";
        public bool CanBorrow() => true;
        public bool CanReserve() => false;
        public bool CanReturn() => false;
    }

    /// <summary>
    /// State Pattern: Material is currently borrowed.
    /// </summary>
    public class BorrowedState : IMaterialState
    {
        public string StateName => "Borrowed";
        public bool CanBorrow() => false;
        public bool CanReserve() => true;
        public bool CanReturn() => true;
    }

    /// <summary>
    /// State Pattern: Material is reserved by a user.
    /// </summary>
    public class ReservedState : IMaterialState
    {
        public string StateName => "Reserved";
        public bool CanBorrow() => false;
        public bool CanReserve() => false;
        public bool CanReturn() => true;
    }

    /// <summary>
    /// State Pattern: Material is lost.
    /// </summary>
    public class LostState : IMaterialState
    {
        public string StateName => "Lost";
        public bool CanBorrow() => false;
        public bool CanReserve() => false;
        public bool CanReturn() => false;
    }
}
