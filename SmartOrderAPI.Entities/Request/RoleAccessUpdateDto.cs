namespace SmartOrderAPI.Entities.Request
{
    public class RoleAccessUpdateDto
    {
        public List<int> PermissionIds { get; set; } = new();
    }
}
