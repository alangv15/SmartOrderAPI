using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

namespace SmartOrderAPI.Business.Service;

public sealed class CostingService : ICostingService
{
    private readonly ICostingRepository _repository;

    public CostingService(ICostingRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CostItemDto>> GetCostItemsAsync() => _repository.GetCostItemsAsync();

    public Task<CostItemDto?> GetCostItemAsync(int costItemId) => _repository.GetCostItemAsync(costItemId);

    public async Task<int> SaveCostItemAsync(SaveCostItemRequestDto request)
    {
        if (request.CostItemId < 0 || string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100)
        {
            throw new ArgumentException("El nombre del concepto es obligatorio.");
        }

        if (!CostingCatalog.IsValidCombination(request.CostItemTypeCode, request.UnitCode))
            throw new ArgumentException("Selecciona un tipo y una unidad compatibles.");
        if (request.Description?.Length > 255)
            throw new ArgumentException("La descripcion no debe superar 255 caracteres.");

        if (request.CurrentCost != null)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentCost.PresentationName) || request.CurrentCost.PresentationName.Trim().Length > 100)
                throw new ArgumentException("La presentacion del costo es obligatoria.");

            if (request.CurrentCost.PresentationQuantity <= 0 || request.CurrentCost.PresentationQuantity >= 100000000000000m ||
                request.CurrentCost.PresentationQuantity != decimal.Round(request.CurrentCost.PresentationQuantity, 4))
                throw new ArgumentException("La cantidad debe ser positiva, menor a 100 billones y tener hasta 4 decimales.");

            if (request.CurrentCost.PresentationCost < 0 || request.CurrentCost.PresentationCost >= 1000000000000m ||
                request.CurrentCost.PresentationCost != decimal.Round(request.CurrentCost.PresentationCost, 6))
                throw new ArgumentException("El costo debe ser no negativo, menor a un billon y tener hasta 6 decimales.");
            if (request.CurrentCost.PresentationCost / request.CurrentCost.PresentationQuantity >= 1000000000000m)
                throw new ArgumentException("El costo unitario excede el limite permitido.");
            if (request.CurrentCost.Notes?.Length > 500)
                throw new ArgumentException("Las notas no deben superar 500 caracteres.");
        }

        return await _repository.SaveCostItemAsync(request);
    }

    public Task<IEnumerable<ProductRecipeDto>> GetRecipesAsync() => _repository.GetRecipesAsync();

    public Task<ProductRecipeDto?> GetCurrentRecipeAsync(int productId) => _repository.GetCurrentRecipeAsync(productId);

    public async Task<int> SaveRecipeAsync(SaveProductRecipeRequestDto request)
    {
        if (request.ProductId <= 0)
            throw new ArgumentException("El producto es obligatorio.");

        if (request.Items == null || request.Bases == null)
            throw new ArgumentException("El detalle de la receta es obligatorio.");

        if (request.Items.Count == 0 && request.Bases.Count == 0)
            throw new ArgumentException("Agrega al menos una receta base o un concepto.");

        ValidateRecipeItems(request.Items);
        if (request.Bases.Any(item => item.BaseRecipeId <= 0 || item.QuantityMultiplier <= 0 ||
            item.QuantityMultiplier != decimal.Round(item.QuantityMultiplier, 6)) ||
            request.Bases.Select(item => item.BaseRecipeId).Distinct().Count() != request.Bases.Count)
            throw new ArgumentException("Revisa las bases: factor positivo, hasta 6 decimales y sin duplicados.");
        if (request.Notes?.Length > 500)
            throw new ArgumentException("Las notas no deben superar 500 caracteres.");

        if (request.Items.Any(item => item.CostItemCostId <= 0 || item.Quantity <= 0))
            throw new ArgumentException("Todos los conceptos deben tener un costo registrado y cantidad mayor a cero.");

        return await _repository.SaveRecipeAsync(request);
    }

    public Task<IEnumerable<ProductCostingIssueDto>> GetProductsWithoutCostingAsync() => _repository.GetProductsWithoutCostingAsync();

    public Task<IEnumerable<BaseRecipeDto>> GetBaseRecipesAsync() => _repository.GetBaseRecipesAsync();

    public Task<int> SaveBaseRecipeAsync(SaveBaseRecipeRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100 ||
            string.IsNullOrWhiteSpace(request.Code) || request.Code.Trim().Length > 50)
            throw new ArgumentException("Indica nombre (hasta 100 caracteres) y codigo (hasta 50 caracteres).");
        if (request.Items == null || request.Items.Count == 0)
            throw new ArgumentException("Agrega al menos un concepto a la receta base.");
        if (request.Notes?.Length > 500)
            throw new ArgumentException("Las notas no deben superar 500 caracteres.");
        ValidateRecipeItems(request.Items);
        return _repository.SaveBaseRecipeAsync(request);
    }

    private static void ValidateRecipeItems(List<SaveProductRecipeItemRequestDto> items)
    {
        if (items.Any(item => item.CostItemCostId <= 0 || item.Quantity <= 0 ||
            item.Quantity != decimal.Round(item.Quantity, 4)) ||
            items.Select(item => item.CostItemCostId).Distinct().Count() != items.Count)
            throw new ArgumentException("Revisa los conceptos: cantidades positivas, hasta 4 decimales y sin duplicados.");
    }

}
