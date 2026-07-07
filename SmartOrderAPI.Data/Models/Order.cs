namespace SmartOrderAPI.Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int BranchId { get; set; }

    public int UserId { get; set; }

    public int? CustomerId { get; set; }

    public int Pieces { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? ProductionStartDate { get; set; }

    public DateTime? ProductionEndDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string OrderStatusCode { get; set; } = null!;

    public string PaymentStatusCode { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? Comments { get; set; }

    public string? CustomerGender { get; set; }

    public string? CustomerAgeRange { get; set; }

    public string? CustomerType { get; set; }

    public string? AcquisitionChannel { get; set; }

    public string SalesChannel { get; set; } = null!;

    public bool IsDirectSale { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; } = new List<OrderDiscount>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus OrderStatusCodeNavigation { get; set; } = null!;

    public virtual PaymentStatus PaymentStatusCodeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
