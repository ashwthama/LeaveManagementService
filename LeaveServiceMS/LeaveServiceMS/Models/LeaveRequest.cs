namespace LeaveServiceMS.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string EmployeeUsername { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public string? ManagerComment { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string EmailId {  get; set; }
    }
}
