namespace LeaveServiceMS.Models
{
    public class ApplyLeaveRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmailId {  get; set; }
    }
}
