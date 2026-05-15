namespace CourseProject.Interfaces
{
    /// <summary>
    /// State Pattern interface for material states (Available, Borrowed, Reserved, Lost).
    /// </summary>
    public interface IMaterialState
    {
        string StateName { get; }
        bool CanBorrow();
        bool CanReserve();
        bool CanReturn();
    }
}
