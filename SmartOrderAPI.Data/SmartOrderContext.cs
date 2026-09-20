using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;

namespace SmartOrderAPI.Data;

public partial class SmartOrderContext : DbContext
{
    public SmartOrderContext()
    {
    }

    public SmartOrderContext(DbContextOptions<SmartOrderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountRule> DiscountRules { get; set; }

    public virtual DbSet<DiscountRuleTarget> DiscountRuleTargets { get; set; }

    public virtual DbSet<DiscountLimitRule> DiscountLimitRules { get; set; }

    public virtual DbSet<DiscountLimitTarget> DiscountLimitTargets { get; set; }

    public virtual DbSet<DiscountTarget> DiscountTargets { get; set; }

    public virtual DbSet<DiscountType> DiscountTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDiscount> OrderDiscounts { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
    public virtual DbSet<CostItem> CostItems { get; set; }
    public virtual DbSet<CostItemCost> CostItemCosts { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductPrice> ProductPrices { get; set; }
    public virtual DbSet<ProductRecipe> ProductRecipes { get; set; }
    public virtual DbSet<ProductRecipeItem> ProductRecipeItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBranch> UserBranches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FA55BD37F47");

            entity.ToTable("Branches", "Catalog");

            entity.HasIndex(e => e.Code, "UQ__Branches__A25C5AA75B2D4578").IsUnique();

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.State).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B2EFC17B9");

            entity.ToTable("Categories", "Catalog");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDirectSale).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B842647BA4");

            entity.ToTable("Customers", "Catalog");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Neighborhood).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.State).HasMaxLength(100);
        });

        modelBuilder.Entity<DiscountLimitRule>(entity =>
        {
            entity.HasKey(e => e.DiscountLimitRuleId);

            entity.ToTable("DiscountLimitRules", "Discount");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MaxDiscountPerUnit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AdjustmentTypeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AdjustmentValue).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<DiscountLimitTarget>(entity =>
        {
            entity.HasKey(e => e.DiscountLimitTargetId);

            entity.ToTable("DiscountLimitTargets", "Discount");

            entity.Property(e => e.TargetType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.DiscountLimitRule).WithMany(p => p.DiscountLimitTargets)
                .HasForeignKey(d => d.DiscountLimitRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountLimitTargets_DiscountLimitRules");
        });

        modelBuilder.Entity<DiscountRule>(entity =>
        {
            entity.HasKey(e => e.DiscountRuleId).HasName("PK__Discount__07F121DF32218044");

            entity.ToTable("DiscountRules", "Discount");

            entity.Property(e => e.ConditionValue).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DiscountTargetCode).HasMaxLength(50);
            entity.Property(e => e.DiscountTypeCode).HasMaxLength(50);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinTotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.DiscountTargetCodeNavigation).WithMany(p => p.DiscountRules)
                .HasForeignKey(d => d.DiscountTargetCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountRules_DiscountTargets");

            entity.HasOne(d => d.DiscountTypeCodeNavigation).WithMany(p => p.DiscountRules)
                .HasForeignKey(d => d.DiscountTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountRules_DiscountTypes");
        });

        modelBuilder.Entity<DiscountRuleTarget>(entity =>
        {
            entity.HasKey(e => e.DiscountRuleTargetId).HasName("PK__Discount__01B7F421071BE9F8");

            entity.ToTable("DiscountRuleTargets", "Discount");

            entity.Property(e => e.TargetType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.DiscountRule).WithMany(p => p.DiscountRuleTargets)
                .HasForeignKey(d => d.DiscountRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountRuleTarget_DiscountRules");
        });

        modelBuilder.Entity<DiscountTarget>(entity =>
        {
            entity.HasKey(e => e.DiscountTargetCode).HasName("PK__Discount__F44B2652B8B9000C");

            entity.ToTable("DiscountTargets", "Catalog");

            entity.Property(e => e.DiscountTargetCode).HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.HasKey(e => e.DiscountTypeCode).HasName("PK__Discount__63A86E6AE4080C95");

            entity.ToTable("DiscountTypes", "Catalog");

            entity.Property(e => e.DiscountTypeCode).HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF8B0CBBCD");

            entity.ToTable("Orders", "Sale");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.AcquisitionChannel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerAgeRange)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CustomerGender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CashReceivedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CashChangeAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrderStatusCode)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.PaymentStatusCode)
                .HasMaxLength(50)
                .HasDefaultValue("Unpaid");
            entity.Property(e => e.ProductionEndDate).HasColumnType("datetime");
            entity.Property(e => e.ProductionStartDate).HasColumnType("datetime");
            entity.Property(e => e.SalesChannel)
                .HasMaxLength(50)
                .HasDefaultValue("In-Store");
            entity.Property(e => e.IsInternalProduction).HasDefaultValue(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Branch).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.OrderStatusCodeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_OrderStatuses");

            entity.HasOne(d => d.PaymentStatusCodeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentStatusCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PaymentStatuses");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });


        modelBuilder.Entity<OrderDiscount>(entity =>
        {
            entity.HasKey(e => e.OrderDiscountId).HasName("PK__OrderDis__5EF1877EB904713E");

            entity.ToTable("OrderDiscounts", "Sale");

            entity.Property(e => e.AppliedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AppliedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.DiscountRule).WithMany(p => p.OrderDiscounts)
                .HasForeignKey(d => d.DiscountRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDiscount_DiscountRules");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDiscounts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDiscount_Orders");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06A17DC007A7");

            entity.ToTable("OrderItems", "Sale");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DiscountPerUnit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([Quantity]*[UnitPrice]-isnull([DiscountAmount],(0)))", true)
                .HasColumnType("decimal(22, 2)");
            entity.Property(e => e.CostAmount)
                .HasComputedColumnSql("([Quantity]*[UnitCost])", true)
                .HasColumnType("decimal(22, 4)");
            entity.Property(e => e.GrossProfit)
                .HasComputedColumnSql("(([Quantity]*[UnitPrice]-isnull([DiscountAmount],(0)))-([Quantity]*[UnitCost]))", true)
                .HasColumnType("decimal(23, 4)");
            entity.Property(e => e.CostCalculatedAt).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId)
                .HasColumnName("ProductID")
                .ValueGeneratedNever();
            entity.Property(e => e.ProductPriceId).HasColumnName("ProductPriceID");
            entity.Property(e => e.ProductRecipeId).HasColumnName("ProductRecipeID");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products");

            entity.HasOne(d => d.ProductPrice).WithMany()
                .HasForeignKey(d => d.ProductPriceId)
                .HasConstraintName("FK_OrderItems_ProductPrices");

            entity.HasOne(d => d.ProductRecipe).WithMany()
                .HasForeignKey(d => d.ProductRecipeId)
                .HasConstraintName("FK_OrderItems_ProductRecipes");
        });

        modelBuilder.Entity<CostItem>(entity =>
        {
            entity.HasKey(e => e.CostItemId);
            entity.ToTable("CostItems", "Catalog");
            entity.Property(e => e.CostItemId).HasColumnName("CostItemID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.CostItemTypeCode).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.UnitCode).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CostItemCost>(entity =>
        {
            entity.HasKey(e => e.CostItemCostId);
            entity.ToTable("CostItemCosts", "Catalog");
            entity.HasIndex(e => e.CostItemId, "UX_CostItemCosts_Current")
                .IsUnique()
                .HasFilter("[EffectiveTo] IS NULL");
            entity.HasIndex(e => new { e.CostItemId, e.EffectiveFrom }, "UX_CostItemCosts_CostItem_EffectiveFrom")
                .IsUnique();
            entity.Property(e => e.CostItemCostId).HasColumnName("CostItemCostID");
            entity.Property(e => e.CostItemId).HasColumnName("CostItemID");
            entity.Property(e => e.PresentationName).HasMaxLength(100);
            entity.Property(e => e.PresentationQuantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.PresentationCost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UnitCost)
                .HasComputedColumnSql("(CONVERT([decimal](18,6),[PresentationCost]/NULLIF([PresentationQuantity],(0))))", true)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("date");
            entity.Property(e => e.EffectiveTo).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.HasOne(d => d.CostItem).WithMany(p => p.CostItemCosts)
                .HasForeignKey(d => d.CostItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CostItemCosts_CostItems");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusCode).HasName("PK__OrderSta__1ACAE7CFE955F4AE");

            entity.ToTable("OrderStatuses", "Catalog");

            entity.Property(e => e.OrderStatusCode).HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusCode).HasName("PK__PaymentS__501089CE47BA1FEB");

            entity.ToTable("PaymentStatuses", "Catalog");

            entity.Property(e => e.PaymentStatusCode).HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);

            entity.ToTable("Permissions", "Catalog");

            entity.HasIndex(e => e.Code, "UQ_Permissions_Code").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnType("datetime2");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDBB7B7E28");

            entity.ToTable("Products", "Catalog");

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0D71E64E64").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDirectSale).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.ProductPriceId);
            entity.ToTable("ProductPrices", "Catalog");

            entity.HasIndex(e => e.ProductId, "UX_ProductPrices_Current")
                .IsUnique()
                .HasFilter("[EffectiveTo] IS NULL");

            entity.HasIndex(e => new { e.ProductId, e.EffectiveFrom }, "UX_ProductPrices_Product_EffectiveFrom")
                .IsUnique();

            entity.Property(e => e.ProductPriceId).HasColumnName("ProductPriceID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("date");
            entity.Property(e => e.EffectiveTo).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPrices_Products");
        });

        modelBuilder.Entity<ProductRecipe>(entity =>
        {
            entity.HasKey(e => e.ProductRecipeId);
            entity.ToTable("ProductRecipes", "Catalog");

            entity.HasIndex(e => e.ProductId, "UX_ProductRecipes_Current")
                .IsUnique()
                .HasFilter("[EffectiveTo] IS NULL");

            entity.HasIndex(e => new { e.ProductId, e.EffectiveFrom }, "UX_ProductRecipes_Product_EffectiveFrom")
                .IsUnique();

            entity.HasIndex(e => new { e.ProductId, e.VersionNumber }, "UQ_ProductRecipes_Product_Version")
                .IsUnique();

            entity.Property(e => e.ProductRecipeId).HasColumnName("ProductRecipeID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.EffectiveFrom).HasColumnType("date");
            entity.Property(e => e.EffectiveTo).HasColumnType("date");
            entity.Property(e => e.RecipeCost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRecipes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductRecipes_Products");
        });

        modelBuilder.Entity<ProductRecipeItem>(entity =>
        {
            entity.HasKey(e => e.ProductRecipeItemId);
            entity.ToTable("ProductRecipeItems", "Catalog");
            entity.HasIndex(e => new { e.ProductRecipeId, e.CostItemCostId }, "UQ_ProductRecipeItems_Recipe_CostItemCost")
                .IsUnique();
            entity.Property(e => e.ProductRecipeItemId).HasColumnName("ProductRecipeItemID");
            entity.Property(e => e.ProductRecipeId).HasColumnName("ProductRecipeID");
            entity.Property(e => e.CostItemCostId).HasColumnName("CostItemCostID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.RecipeItemCost).HasColumnType("decimal(18, 6)");
            entity.HasOne(d => d.ProductRecipe).WithMany(p => p.ProductRecipeItems)
                .HasForeignKey(d => d.ProductRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductRecipeItems_ProductRecipes");
            entity.HasOne(d => d.CostItemCost).WithMany()
                .HasForeignKey(d => d.CostItemCostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductRecipeItems_CostItemCosts");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("Roles", "Catalog");

            entity.HasIndex(e => e.Code, "UQ_Roles_Code").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnType("datetime2");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId);

            entity.ToTable("RolePermissions", "Catalog");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_RolePermissions_Role_Permission").IsUnique();

            entity.Property(e => e.RolePermissionId).HasColumnName("RolePermissionID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnType("datetime2");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE8993EA5");

            entity.ToTable("Users", "Catalog");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342C7D97AD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserBranch>(entity =>
        {
            entity.HasKey(e => e.UserBranchId).HasName("PK__UserBran__101E3831BA571678");

            entity.ToTable("UserBranches", "Catalog");

            entity.HasIndex(e => new { e.UserId, e.BranchId }, "UQ_UserBranches_User_Branch").IsUnique();

            entity.Property(e => e.UserBranchId).HasColumnName("UserBranchID");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Branch).WithMany(p => p.UserBranches)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserBranches_Branches");

            entity.HasOne(d => d.User).WithMany(p => p.UserBranches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserBranches_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
