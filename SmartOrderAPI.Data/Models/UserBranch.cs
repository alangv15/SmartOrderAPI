using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class UserBranch
{
    public int UserBranchId { get; set; }

    public int UserId { get; set; }

    public int BranchId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
