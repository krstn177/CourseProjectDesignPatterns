using CourseProject.Interfaces;

namespace CourseProject.Strategies
{
    /// <summary>
    /// Strategy Pattern: Fixed penalty regardless of days late.
    /// </summary>
    public class FixedPenaltyStrategy : IPenaltyStrategy
    {
        public string StrategyName => "Fixed";
        public decimal CalculatePenalty(int daysLate, IUser user) => daysLate > 0 ? 5.00m : 0m;
    }

    /// <summary>
    /// Strategy Pattern: Percentage-based penalty (increases with days late).
    /// </summary>
    public class PercentagePenaltyStrategy : IPenaltyStrategy
    {
        public string StrategyName => "Percentage";
        public decimal CalculatePenalty(int daysLate, IUser user) => daysLate * 0.50m;
    }

    /// <summary>
    /// Strategy Pattern: Role-based penalty (guests pay more, admins pay less).
    /// </summary>
    public class RoleBasedPenaltyStrategy : IPenaltyStrategy
    {
        public string StrategyName => "RoleBased";
        public decimal CalculatePenalty(int daysLate, IUser user)
        {
            if (daysLate <= 0) return 0m;
            decimal multiplier = user.Role switch
            {
                "Guest" => 2.0m,
                "Student" => 1.0m,
                "Teacher" => 0.5m,
                "Admin" => 0.25m,
                _ => 1.0m
            };
            return daysLate * multiplier;
        }
    }
}
