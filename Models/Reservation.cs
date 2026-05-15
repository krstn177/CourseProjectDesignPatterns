namespace CourseProject.Models
{
    /// <summary>
    /// Represents a reservation for an unavailable material.
    /// </summary>
    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid MaterialId { get; set; }
        public DateTime ReservedDate { get; set; } = DateTime.Now;
        public bool IsFulfilled { get; set; }
    }
}
