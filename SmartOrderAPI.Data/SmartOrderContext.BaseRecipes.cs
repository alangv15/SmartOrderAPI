using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;

namespace SmartOrderAPI.Data;

public partial class SmartOrderContext
{
    public DbSet<BaseRecipeGroup> BaseRecipeGroups => Set<BaseRecipeGroup>();
    public DbSet<BaseRecipe> BaseRecipes => Set<BaseRecipe>();
    public DbSet<BaseRecipeItem> BaseRecipeItems => Set<BaseRecipeItem>();
    public DbSet<ProductRecipeBase> ProductRecipeBases => Set<ProductRecipeBase>();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseRecipeGroup>(entity =>
        {
            entity.ToTable("BaseRecipeGroups", "Catalog");
            entity.HasKey(e => e.BaseRecipeGroupId);
            entity.Property(e => e.BaseRecipeGroupId).HasColumnName("BaseRecipeGroupID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
        });
        modelBuilder.Entity<BaseRecipe>(entity =>
        {
            entity.ToTable("BaseRecipes", "Catalog");
            entity.HasKey(e => e.BaseRecipeId);
            entity.Property(e => e.BaseRecipeId).HasColumnName("BaseRecipeID");
            entity.Property(e => e.BaseRecipeGroupId).HasColumnName("BaseRecipeGroupID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EffectiveFrom).HasColumnType("date");
            entity.Property(e => e.EffectiveTo).HasColumnType("date");
            entity.Property(e => e.RecipeCost).HasPrecision(18, 6);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.HasIndex(e => e.BaseRecipeGroupId).IsUnique().HasFilter("[EffectiveTo] IS NULL");
            entity.HasIndex(e => new { e.BaseRecipeGroupId, e.VersionNumber }).IsUnique();
            entity.HasIndex(e => new { e.BaseRecipeGroupId, e.EffectiveFrom }).IsUnique();
            entity.HasOne(e => e.Group).WithMany().HasForeignKey(e => e.BaseRecipeGroupId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Items).WithOne().HasForeignKey(e => e.BaseRecipeId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<BaseRecipeItem>(entity =>
        {
            entity.ToTable("BaseRecipeItems", "Catalog");
            entity.HasKey(e => e.BaseRecipeItemId);
            entity.Property(e => e.BaseRecipeItemId).HasColumnName("BaseRecipeItemID");
            entity.Property(e => e.BaseRecipeId).HasColumnName("BaseRecipeID");
            entity.Property(e => e.CostItemCostId).HasColumnName("CostItemCostID");
            entity.Property(e => e.Quantity).HasPrecision(18, 4);
            entity.Property(e => e.RecipeItemCost).HasPrecision(18, 6);
            entity.HasIndex(e => new { e.BaseRecipeId, e.CostItemCostId }).IsUnique();
            entity.HasOne(e => e.CostItemCost).WithMany().HasForeignKey(e => e.CostItemCostId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<ProductRecipeBase>(entity =>
        {
            entity.ToTable("ProductRecipeBases", "Catalog");
            entity.HasKey(e => e.ProductRecipeBaseId);
            entity.Property(e => e.ProductRecipeBaseId).HasColumnName("ProductRecipeBaseID");
            entity.Property(e => e.ProductRecipeId).HasColumnName("ProductRecipeID");
            entity.Property(e => e.BaseRecipeId).HasColumnName("BaseRecipeID");
            entity.Property(e => e.QuantityMultiplier).HasPrecision(18, 6);
            entity.Property(e => e.BaseRecipeCost).HasPrecision(18, 6);
            entity.HasIndex(e => new { e.ProductRecipeId, e.BaseRecipeId }).IsUnique();
            entity.HasOne(e => e.BaseRecipe).WithMany().HasForeignKey(e => e.BaseRecipeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<ProductRecipe>().WithMany(e => e.Bases).HasForeignKey(e => e.ProductRecipeId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
