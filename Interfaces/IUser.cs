namespace CourseProject.Interfaces
{
    /// <summary>
    /// Interface for user entities.
    /// </summary>
    public interface IUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Role { get; set; }
        List<Guid> BorrowedMaterialIds { get; set; }
        decimal PenaltyBalance { get; set; }
        int BorrowLimit { get; }
    }
}
