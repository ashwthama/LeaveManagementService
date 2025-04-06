namespace LeaveServiceMS.Models
{
    public class LeaveBalance
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int AvailableDays { get; set; } = 20;
    }
}
