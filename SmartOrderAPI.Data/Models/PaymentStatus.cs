using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class PaymentStatus
{
    public string PaymentStatusCode { get; set; } = null!;

    public string? DisplayName { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
