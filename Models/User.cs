using CourseProject.Interfaces;

namespace CourseProject.Models
{
    /// <summary>
    /// User model with role-based borrow limits.
    /// </summary>
    public class User : IUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "Guest";
        public List<Guid> BorrowedMaterialIds { get; set; } = new();
        public decimal PenaltyBalance { get; set; }

        public int BorrowLimit => Role switch
        {
            "Admin" => 20,
            "Teacher" => 10,
            "Student" => 5,
            "Guest" => 2,
            _ => 1
        };

        public override string ToString() => $"{Name} ({Role}) - Borrowed: {BorrowedMaterialIds.Count}/{BorrowLimit}, Penalties: ${PenaltyBalance:F2}";
    }
}
