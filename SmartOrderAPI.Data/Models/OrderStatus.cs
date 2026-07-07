using System;
using System.Collections.Generic;

namespace SmartOrderAPI.Data.Models;

public partial class OrderStatus
{
    public string OrderStatusCode { get; set; } = null!;

    public string? DisplayName { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
