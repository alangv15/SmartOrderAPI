using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories;

public sealed partial class CostingRepository
{
    public async Task<IEnumerable<BaseRecipeDto>> GetBaseRecipesAsync()
    {
        var recipes = await _context.BaseRecipes.AsNoTracking()
            .Include(recipe => recipe.Group)
            .Include(recipe => recipe.Items).ThenInclude(item => item.CostItemCost).ThenInclude(cost => cost.CostItem)
            .OrderBy(recipe => recipe.Group.Name).ThenByDescending(recipe => recipe.VersionNumber)
            .ToListAsync();
        return recipes.Select(recipe => new BaseRecipeDto
        {
            BaseRecipeId = recipe.BaseRecipeId,
            BaseRecipeGroupId = recipe.BaseRecipeGroupId,
            Name = recipe.Group.Name,
            Code = recipe.Group.Code,
            VersionNumber = recipe.VersionNumber,
            EffectiveFrom = recipe.EffectiveFrom,
            EffectiveTo = recipe.EffectiveTo,
            RecipeCost = recipe.RecipeCost,
            IsActive = recipe.IsActive && recipe.Group.IsActive,
            Notes = recipe.Notes,
            Items = recipe.Items.OrderBy(item => item.CostItemCost.CostItem.Name).Select(item => new ProductRecipeItemDto
            {
                CostItemCostId = item.CostItemCostId,
                CostItemId = item.CostItemCost.CostItemId,
                CostItemName = item.CostItemCost.CostItem.Name,
                CostItemTypeCode = item.CostItemCost.CostItem.CostItemTypeCode,
                UnitCode = item.CostItemCost.CostItem.UnitCode,
                PresentationName = item.CostItemCost.PresentationName,
                Quantity = item.Quantity,
                UnitCost = item.CostItemCost.UnitCost ?? 0,
                RecipeItemCost = item.RecipeItemCost
            }).ToList()
        }).ToList();
    }

    public async Task<int> SaveBaseRecipeAsync(SaveBaseRecipeRequestDto request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var code = request.Code.Trim();
        if (await _context.BaseRecipeGroups.AnyAsync(group => group.Code == code && group.BaseRecipeGroupId != request.BaseRecipeGroupId))
            throw new ArgumentException("Ya existe una receta base con ese codigo.");

        var group = request.BaseRecipeGroupId == 0 ? new BaseRecipeGroup() :
            await _context.BaseRecipeGroups.FindAsync(request.BaseRecipeGroupId)
                ?? throw new ArgumentException("Receta base no encontrada.");
        if (request.BaseRecipeGroupId == 0) _context.BaseRecipeGroups.Add(group);
        group.Name = request.Name.Trim();
        group.Code = code;
        group.IsActive = request.IsActive;

        var effectiveFrom = request.EffectiveFrom == default ? DateTime.Today : request.EffectiveFrom.Date;
        var current = await _context.BaseRecipes.Include(recipe => recipe.Items)
            .FirstOrDefaultAsync(recipe => recipe.BaseRecipeGroupId == request.BaseRecipeGroupId && recipe.EffectiveTo == null);
        if (current != null && effectiveFrom < current.EffectiveFrom)
            throw new ArgumentException("La vigencia no puede ser anterior a la receta base actual.");

        var ids = request.Items.Select(item => item.CostItemCostId).ToList();
        var costs = await _context.CostItemCosts.Include(cost => cost.CostItem).Where(cost => ids.Contains(cost.CostItemCostId))
            .ToDictionaryAsync(cost => cost.CostItemCostId);
        ValidateCostSelection(request.Items, costs, effectiveFrom,
            current?.Items.Select(item => item.CostItemCostId).ToList() ?? new List<int>(), true);
        var items = BuildRecipeItems(request.Items, costs).Select(item => new BaseRecipeItem
        {
            CostItemCostId = item.CostItemCostId,
            Quantity = item.Quantity,
            RecipeItemCost = item.RecipeItemCost
        }).ToList();

        BaseRecipe recipe;
        if (current != null && effectiveFrom == current.EffectiveFrom)
        {
            var detailsChanged = current.Items.Count != items.Count || items.Any(item => !current.Items.Any(old =>
                old.CostItemCostId == item.CostItemCostId && old.Quantity == item.Quantity &&
                old.RecipeItemCost == item.RecipeItemCost));
            if (detailsChanged && await _context.ProductRecipeBases.AnyAsync(item => item.BaseRecipeId == current.BaseRecipeId))
                throw new ArgumentException("Esta version ya se utiliza en productos. Selecciona una fecha posterior para guardar otra version.");
            recipe = current;
            if (detailsChanged)
            {
                _context.BaseRecipeItems.RemoveRange(current.Items);
                await _context.SaveChangesAsync();
                current.Items = items;
            }
        }
        else
        {
            var version = (await _context.BaseRecipes.Where(item => item.BaseRecipeGroupId == request.BaseRecipeGroupId)
                .MaxAsync(item => (int?)item.VersionNumber) ?? 0) + 1;
            if (current != null)
            {
                current.EffectiveTo = effectiveFrom.AddDays(-1);
                current.IsActive = false;
                await _context.SaveChangesAsync();
            }
            recipe = new BaseRecipe
            {
                Group = group,
                VersionNumber = version,
                EffectiveFrom = effectiveFrom,
                CreatedAt = DateTime.UtcNow,
                Items = items
            };
            _context.BaseRecipes.Add(recipe);
        }
        recipe.RecipeCost = items.Sum(item => item.RecipeItemCost);
        recipe.IsActive = request.IsActive;
        recipe.Notes = NormalizeOptionalText(request.Notes);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return recipe.BaseRecipeId;
    }

    private async Task<List<ProductRecipeBase>> BuildRecipeBasesAsync(
        List<SaveProductRecipeBaseRequestDto> requests, DateTime effectiveFrom, int? currentRecipeId)
    {
        var ids = requests.Select(item => item.BaseRecipeId).ToList();
        var bases = await _context.BaseRecipes.Include(recipe => recipe.Group)
            .Where(recipe => ids.Contains(recipe.BaseRecipeId)).ToDictionaryAsync(recipe => recipe.BaseRecipeId);
        var existingIds = await _context.ProductRecipeBases.Where(item => item.ProductRecipeId == currentRecipeId)
            .Select(item => item.BaseRecipeId).ToListAsync();
        return requests.Select(item =>
        {
            if (!bases.TryGetValue(item.BaseRecipeId, out var recipe))
                throw new ArgumentException("Una de las recetas base ya no existe.");
            if (!existingIds.Contains(item.BaseRecipeId) && (!recipe.Group.IsActive || !recipe.IsActive ||
                recipe.EffectiveFrom > effectiveFrom || (recipe.EffectiveTo != null && recipe.EffectiveTo < effectiveFrom)))
                throw new ArgumentException("Selecciona una receta base activa y vigente para la fecha indicada.");
            return new ProductRecipeBase
            {
                BaseRecipeId = item.BaseRecipeId,
                QuantityMultiplier = item.QuantityMultiplier,
                BaseRecipeCost = decimal.Round(recipe.RecipeCost * item.QuantityMultiplier, 6)
            };
        }).ToList();
    }
}
