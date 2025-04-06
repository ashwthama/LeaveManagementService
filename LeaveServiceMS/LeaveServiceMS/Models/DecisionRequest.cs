namespace LeaveServiceMS.Models
{
    public class DecisionRequest
    {
        public int RequestId { get; set; }
        public bool IsApproved { get; set; }
        public string? ManagerComment { get; set; }
    }
}
