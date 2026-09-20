using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Data.Repositories;

public sealed partial class CostingRepository : ICostingRepository
{
    private readonly SmartOrderContext _context;

    public CostingRepository(SmartOrderContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CostItemDto>> GetCostItemsAsync()
    {
        var costItems = await _context.CostItems
            .Include(costItem => costItem.CostItemCosts)
            .AsNoTracking()
            .OrderByDescending(costItem => costItem.IsActive)
            .ThenBy(costItem => costItem.Name)
            .ToListAsync();

        return costItems.Select(MapCostItem).ToList();
    }

    public async Task<CostItemDto?> GetCostItemAsync(int costItemId)
    {
        var costItem = await _context.CostItems
            .Include(item => item.CostItemCosts)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.CostItemId == costItemId);

        return costItem == null ? null : MapCostItem(costItem);
    }

    public async Task<int> SaveCostItemAsync(SaveCostItemRequestDto request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var now = DateTime.UtcNow;
        CostItem costItem;

        if (request.CostItemId <= 0)
        {
            costItem = new CostItem
            {
                Name = request.Name.Trim(),
                Description = NormalizeOptionalText(request.Description),
                CostItemTypeCode = request.CostItemTypeCode,
                UnitCode = request.UnitCode,
                IsActive = request.IsActive,
                CreatedAt = now
            };
            _context.CostItems.Add(costItem);
            await _context.SaveChangesAsync();
        }
        else
        {
            costItem = await _context.CostItems
                .FirstOrDefaultAsync(item => item.CostItemId == request.CostItemId)
                ?? throw new ArgumentException("Concepto no encontrado.");

            if ((costItem.UnitCode != request.UnitCode || costItem.CostItemTypeCode != request.CostItemTypeCode) &&
                await _context.CostItemCosts.AnyAsync(cost => cost.CostItemId == costItem.CostItemId))
                throw new ArgumentException("No se puede cambiar el tipo o la unidad de un concepto con historial de costos. Crea otro concepto.");

            costItem.Name = request.Name.Trim();
            costItem.CostItemTypeCode = request.CostItemTypeCode;
            costItem.UnitCode = request.UnitCode;
            costItem.Description = NormalizeOptionalText(request.Description);
            costItem.IsActive = request.IsActive;
            costItem.UpdatedAt = now;
        }

        if (request.CurrentCost != null)
        {
            await SaveCostItemCostAsync(costItem.CostItemId, request.CurrentCost, now);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return costItem.CostItemId;
    }

    public async Task<IEnumerable<ProductRecipeDto>> GetRecipesAsync()
    {
        var recipes = await _context.ProductRecipes
            .Include(recipe => recipe.Bases).ThenInclude(item => item.BaseRecipe).ThenInclude(recipe => recipe.Group)
            .AsSplitQuery()
            .Include(recipe => recipe.Product)
            .Include(recipe => recipe.ProductRecipeItems)
                .ThenInclude(item => item.CostItemCost)
                    .ThenInclude(cost => cost.CostItem)
            .AsNoTracking()
            .OrderBy(recipe => recipe.Product.Name)
            .ThenByDescending(recipe => recipe.VersionNumber)
            .ToListAsync();

        return recipes.Select(MapRecipe).ToList();
    }

    public async Task<ProductRecipeDto?> GetCurrentRecipeAsync(int productId)
    {
        var recipe = await _context.ProductRecipes
            .Include(recipe => recipe.Bases).ThenInclude(item => item.BaseRecipe).ThenInclude(recipe => recipe.Group)
            .AsSplitQuery()
            .Include(item => item.Product)
            .Include(item => item.ProductRecipeItems)
                .ThenInclude(item => item.CostItemCost)
                    .ThenInclude(cost => cost.CostItem)
            .AsNoTracking()
            .Where(item => item.ProductId == productId)
            .Where(item => item.EffectiveTo == null)
            .OrderByDescending(item => item.VersionNumber)
            .FirstOrDefaultAsync();

        return recipe == null ? null : MapRecipe(recipe);
    }

    public async Task<int> SaveRecipeAsync(SaveProductRecipeRequestDto request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var product = await _context.Products
            .FirstOrDefaultAsync(item => item.ProductId == request.ProductId)
            ?? throw new InvalidOperationException("Producto no encontrado.");

        var effectiveFrom = request.EffectiveFrom.Date == default ? DateTime.UtcNow.Date : request.EffectiveFrom.Date;
        var current = await _context.ProductRecipes
            .Where(recipe => recipe.ProductId == request.ProductId)
            .Where(recipe => recipe.EffectiveTo == null)
            .OrderByDescending(recipe => recipe.VersionNumber)
            .FirstOrDefaultAsync();

        var costItemCostIds = request.Items.Select(item => item.CostItemCostId).Distinct().ToList();
        var costs = await _context.CostItemCosts
            .Include(cost => cost.CostItem)
            .Where(cost => costItemCostIds.Contains(cost.CostItemCostId))
            .ToDictionaryAsync(cost => cost.CostItemCostId);

        var existingCostIds = await _context.ProductRecipeItems.Where(item => item.ProductRecipeId == (current == null ? 0 : current.ProductRecipeId))
            .Select(item => item.CostItemCostId).ToListAsync();
        ValidateCostSelection(request.Items, costs, effectiveFrom, existingCostIds, false);
        var recipeItems = BuildRecipeItems(request.Items, costs).ToList();
        var recipeBases = await BuildRecipeBasesAsync(request.Bases, effectiveFrom, current?.ProductRecipeId);
        var recipeCost = recipeItems.Sum(item => item.RecipeItemCost) + recipeBases.Sum(item => item.BaseRecipeCost);
        if (current != null && effectiveFrom < current.EffectiveFrom)
            throw new ArgumentException("La vigencia no puede ser anterior a la receta actual.");

        if (current != null && current.EffectiveFrom.Date == effectiveFrom)
        {
            var existingItems = await _context.ProductRecipeItems
                .Where(item => item.ProductRecipeId == current.ProductRecipeId)
                .ToListAsync();

            var existingBases = await _context.ProductRecipeBases.Where(item => item.ProductRecipeId == current.ProductRecipeId).ToListAsync();
            var detailsChanged = existingItems.Count != recipeItems.Count || recipeItems.Any(item => !existingItems.Any(old =>
                old.CostItemCostId == item.CostItemCostId && old.Quantity == item.Quantity && old.RecipeItemCost == item.RecipeItemCost)) ||
                existingBases.Count != recipeBases.Count || recipeBases.Any(item => !existingBases.Any(old =>
                old.BaseRecipeId == item.BaseRecipeId && old.QuantityMultiplier == item.QuantityMultiplier && old.BaseRecipeCost == item.BaseRecipeCost));
            if (detailsChanged && await _context.OrderItems.AnyAsync(item => item.ProductRecipeId == current.ProductRecipeId))
                throw new ArgumentException("Esta receta ya se utilizo en ventas o pedidos. Usa una fecha posterior para crear otra version.");

            _context.ProductRecipeItems.RemoveRange(existingItems);
            _context.ProductRecipeBases.RemoveRange(await _context.ProductRecipeBases
                .Where(item => item.ProductRecipeId == current.ProductRecipeId).ToListAsync());
            await _context.SaveChangesAsync();
            current.RecipeCost = recipeCost;
            current.Notes = NormalizeOptionalText(request.Notes);
            current.IsActive = true;

            foreach (var item in recipeItems)
            {
                item.ProductRecipeId = current.ProductRecipeId;
                _context.ProductRecipeItems.Add(item);
            }

            foreach (var item in recipeBases)
            {
                item.ProductRecipeId = current.ProductRecipeId;
                _context.ProductRecipeBases.Add(item);
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return current.ProductRecipeId;
        }

        var nextVersion = (await _context.ProductRecipes
            .Where(recipe => recipe.ProductId == request.ProductId)
            .Select(recipe => (int?)recipe.VersionNumber)
            .MaxAsync() ?? 0) + 1;

        if (current != null)
        {
            current.EffectiveTo = effectiveFrom.AddDays(-1);
            current.IsActive = false;
            await _context.SaveChangesAsync();
        }

        var recipe = new ProductRecipe
        {
            ProductId = product.ProductId,
            VersionNumber = nextVersion,
            EffectiveFrom = effectiveFrom,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Notes = NormalizeOptionalText(request.Notes),
            RecipeCost = recipeCost
        };

        foreach (var item in recipeItems)
        {
            recipe.ProductRecipeItems.Add(item);
        }

        _context.ProductRecipes.Add(recipe);
        foreach (var item in recipeBases) recipe.Bases.Add(item);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return recipe.ProductRecipeId;
    }

    public async Task<IEnumerable<ProductCostingIssueDto>> GetProductsWithoutCostingAsync()
    {
        var effectiveDate = DateTime.UtcNow.Date;
        var products = await _context.Products
            .Include(product => product.Category)
            .AsNoTracking()
            .Where(product => product.IsActive)
            .OrderBy(product => product.Category.Name)
            .ThenBy(product => product.Name)
            .Select(product => new
            {
                product.ProductId,
                ProductName = product.Name,
                CategoryName = product.Category.Name,
                HasPrice = _context.ProductPrices.Any(price =>
                    price.ProductId == product.ProductId &&
                    price.EffectiveFrom <= effectiveDate &&
                    (price.EffectiveTo == null || price.EffectiveTo >= effectiveDate)),
                HasRecipe = _context.ProductRecipes.Any(recipe =>
                    recipe.ProductId == product.ProductId &&
                    recipe.IsActive &&
                    recipe.RecipeCost > 0 &&
                    recipe.EffectiveFrom <= effectiveDate &&
                    (recipe.EffectiveTo == null || recipe.EffectiveTo >= effectiveDate))
            })
            .ToListAsync();

        return products
            .Where(product => !product.HasPrice || !product.HasRecipe)
            .Select(product => new ProductCostingIssueDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName.Trim(),
                CategoryName = product.CategoryName.Trim(),
                Issue = !product.HasPrice && !product.HasRecipe
                    ? "Sin precio vigente y sin receta vigente"
                    : !product.HasPrice
                    ? "Sin precio vigente"
                    : "Sin receta/costo vigente"
            })
            .ToList();
    }

    private async Task SaveCostItemCostAsync(int costItemId, SaveCostItemCostRequestDto request, DateTime now)
    {
        var effectiveFrom = request.EffectiveFrom.Date == default ? DateTime.UtcNow.Date : request.EffectiveFrom.Date;
        var current = await _context.CostItemCosts
            .Where(cost => cost.CostItemId == costItemId)
            .Where(cost => cost.EffectiveTo == null)
            .FirstOrDefaultAsync();

        if (current != null && effectiveFrom < current.EffectiveFrom)
            throw new ArgumentException("La vigencia no puede ser anterior al costo actual.");
        if (await _context.CostItemCosts.AnyAsync(cost => cost.CostItemId == costItemId &&
            (current == null || cost.CostItemCostId != current.CostItemCostId) &&
            (cost.EffectiveTo == null || cost.EffectiveTo >= effectiveFrom)))
            throw new ArgumentException("La vigencia se superpone con otro costo registrado.");

        var normalizedPresentation = request.PresentationName.Trim();
        var currentMatches = current != null &&
            current.PresentationName == normalizedPresentation &&
            current.PresentationQuantity == request.PresentationQuantity &&
            current.PresentationCost == request.PresentationCost;

        if (currentMatches)
        {
            current!.Notes = NormalizeOptionalText(request.Notes);
            return;
        }

        if (current != null && current.EffectiveFrom.Date == effectiveFrom)
        {
            if (await _context.ProductRecipeItems.AnyAsync(item => item.CostItemCostId == current.CostItemCostId) ||
                await _context.BaseRecipeItems.AnyAsync(item => item.CostItemCostId == current.CostItemCostId))
                throw new ArgumentException("Este costo ya se utiliza en recetas. Usa una fecha posterior para conservar su historial.");
            current.PresentationName = normalizedPresentation;
            current.PresentationQuantity = request.PresentationQuantity;
            current.PresentationCost = request.PresentationCost;
            current.Notes = NormalizeOptionalText(request.Notes);
            return;
        }

        if (current != null)
        {
            current.EffectiveTo = effectiveFrom.AddDays(-1);
            await _context.SaveChangesAsync();
        }

        _context.CostItemCosts.Add(new CostItemCost
        {
            CostItemId = costItemId,
            PresentationName = normalizedPresentation,
            PresentationQuantity = request.PresentationQuantity,
            PresentationCost = request.PresentationCost,
            EffectiveFrom = effectiveFrom,
            CreatedAt = now,
            Notes = NormalizeOptionalText(request.Notes)
        });
    }

    private static CostItemDto MapCostItem(CostItem costItem)
    {
        var costs = costItem.CostItemCosts
            .OrderByDescending(cost => cost.EffectiveFrom)
            .Select(MapCostItemCost)
            .ToList();

        return new CostItemDto
        {
            CostItemId = costItem.CostItemId,
            CostItemTypeCode = costItem.CostItemTypeCode,
            UnitCode = costItem.UnitCode,
            IsUnitLocked = costs.Count > 0,
            IsTypeLocked = costs.Count > 0,
            Name = costItem.Name.Trim(),
            Description = costItem.Description,
            IsActive = costItem.IsActive,
            CreatedAt = costItem.CreatedAt,
            UpdatedAt = costItem.UpdatedAt,
            CurrentCost = costs.FirstOrDefault(cost => cost.EffectiveTo == null),
            CostHistory = costs
        };
    }

    private static CostItemCostDto MapCostItemCost(CostItemCost cost)
    {
        return new CostItemCostDto
        {
            CostItemCostId = cost.CostItemCostId,
            CostItemId = cost.CostItemId,
            PresentationName = cost.PresentationName.Trim(),
            PresentationQuantity = cost.PresentationQuantity,
            PresentationCost = cost.PresentationCost,
            UnitCost = cost.UnitCost ?? 0,
            EffectiveFrom = cost.EffectiveFrom,
            EffectiveTo = cost.EffectiveTo,
            Notes = cost.Notes
        };
    }

    private static ProductRecipeDto MapRecipe(ProductRecipe recipe)
    {
        return new ProductRecipeDto
        {
            ProductRecipeId = recipe.ProductRecipeId,
            ProductId = recipe.ProductId,
            ProductName = recipe.Product.Name.Trim(),
            VersionNumber = recipe.VersionNumber,
            EffectiveFrom = recipe.EffectiveFrom,
            EffectiveTo = recipe.EffectiveTo,
            RecipeCost = recipe.RecipeCost,
            IsActive = recipe.IsActive,
            Notes = recipe.Notes,
            Bases = recipe.Bases.Select(item => new ProductRecipeBaseDto
            {
                BaseRecipeId = item.BaseRecipeId,
                Name = item.BaseRecipe.Group.Name,
                VersionNumber = item.BaseRecipe.VersionNumber,
                QuantityMultiplier = item.QuantityMultiplier,
                UnitCost = item.BaseRecipe.RecipeCost,
                BaseRecipeCost = item.BaseRecipeCost
            }).ToList(),
            Items = recipe.ProductRecipeItems
                .OrderBy(item => item.CostItemCost.CostItem.Name)
                .Select(item => new ProductRecipeItemDto
                {
                    ProductRecipeItemId = item.ProductRecipeItemId,
                    ProductRecipeId = item.ProductRecipeId,
                    CostItemCostId = item.CostItemCostId,
                    CostItemId = item.CostItemCost.CostItemId,
                    CostItemName = item.CostItemCost.CostItem.Name.Trim(),
                    CostItemTypeCode = item.CostItemCost.CostItem.CostItemTypeCode,
                    UnitCode = item.CostItemCost.CostItem.UnitCode,
                    PresentationName = item.CostItemCost.PresentationName.Trim(),
                    Quantity = item.Quantity,
                    UnitCost = item.CostItemCost.UnitCost ?? 0,
                    RecipeItemCost = item.RecipeItemCost
                })
                .ToList()
        };
    }


    private static IEnumerable<ProductRecipeItem> BuildRecipeItems(
        IEnumerable<SaveProductRecipeItemRequestDto> requestItems,
        IReadOnlyDictionary<int, CostItemCost> costs)
    {
        foreach (var item in requestItems)
        {
            if (!costs.TryGetValue(item.CostItemCostId, out var cost))
            {
                throw new ArgumentException("Uno de los costos de concepto no existe.");
            }

            yield return new ProductRecipeItem
            {
                CostItemCostId = item.CostItemCostId,
                Quantity = item.Quantity,
                RecipeItemCost = decimal.Round(item.Quantity * (cost.UnitCost ?? 0), 6)
            };
        }
    }

    private static void ValidateCostSelection(IEnumerable<SaveProductRecipeItemRequestDto> items,
        IReadOnlyDictionary<int, CostItemCost> costs, DateTime effectiveDate, ICollection<int> existingIds, bool isBaseRecipe)
    {
        var conceptIds = new HashSet<int>();
        foreach (var item in items)
        {
            if (!costs.TryGetValue(item.CostItemCostId, out var cost))
                throw new ArgumentException("Uno de los costos seleccionados no existe.");
            if (!conceptIds.Add(cost.CostItemId))
                throw new ArgumentException("No agregues dos versiones de costo del mismo concepto.");
            if (isBaseRecipe && !CostingCatalog.IsAllowedInBaseRecipe(cost.CostItem.CostItemTypeCode))
                throw new ArgumentException("Las recetas base admiten materias primas, empaques y servicios. Los productos de reventa se registran directamente en la receta del producto.");
            if (!existingIds.Contains(item.CostItemCostId) && (!cost.CostItem.IsActive || cost.EffectiveFrom > effectiveDate ||
                (cost.EffectiveTo != null && cost.EffectiveTo < effectiveDate)))
                throw new ArgumentException("Selecciona conceptos activos con costo vigente en la fecha de la receta.");
            if (item.Quantity <= 0 || item.Quantity >= 100000000000000m ||
                item.Quantity != decimal.Round(item.Quantity, 4) ||
                cost.UnitCost == null || item.Quantity * cost.UnitCost >= 1000000000000m)
                throw new ArgumentException("Revisa la cantidad y el importe del concepto.");
        }
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
