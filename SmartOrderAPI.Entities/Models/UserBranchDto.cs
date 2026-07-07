namespace SmartOrderAPI.Entities.Models
{
    public class UserBranchDto
    {
        public int UserBranchId { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
