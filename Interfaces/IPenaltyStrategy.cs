namespace CourseProject.Interfaces
{
    /// <summary>
    /// Strategy Pattern interface for penalty calculation.
    /// Demonstrates: Strategy Pattern - algorithms can be swapped at runtime.
    /// </summary>
    public interface IPenaltyStrategy
    {
        string StrategyName { get; }
        decimal CalculatePenalty(int daysLate, IUser user);
    }
}
