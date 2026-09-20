using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Data;
using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Entities.Models;

// These checks deliberately never open a database connection.
var passed = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    passed++;
    Console.WriteLine("PASS: " + name);
}
async Task Reject(Func<Task> action, string name)
{
    try { await action(); }
    catch (ArgumentException) { Check(true, name); return; }
    throw new Exception("Expected validation failure: " + name);
}

var repository = new RecordingRepository();
var service = new CostingService(repository);
SaveCostItemRequestDto Request(string type = "RawMaterial", string unit = "Gram") => new()
{
    Name = "Test item", CostItemTypeCode = type, UnitCode = unit,
    CurrentCost = new() { PresentationName = "Test presentation", PresentationQuantity = 1,
        PresentationCost = 1.257862m, EffectiveFrom = DateTime.Today }
};
foreach (var type in CostingCatalog.Types)
foreach (var unit in CostingCatalog.Units)
{
    if (CostingCatalog.IsValidCombination(type.Code, unit.Code))
        Check(await service.SaveCostItemAsync(Request(type.Code, unit.Code)) == 1, $"Valid unit {type.Code}/{unit.Code}");
    else
        await Reject(() => service.SaveCostItemAsync(Request(type.Code, unit.Code)), $"Invalid unit {type.Code}/{unit.Code}");
}
Check(repository.Last!.CurrentCost!.PresentationCost == 1.257862m, "Six decimal service tariff preserved");
var blank = Request(); blank.Name = " ";
await Reject(() => service.SaveCostItemAsync(blank), "Blank name");
var invalidType = Request("Unknown");
await Reject(() => service.SaveCostItemAsync(invalidType), "Unknown type");
var partial = Request(); partial.CurrentCost = new();
await Reject(() => service.SaveCostItemAsync(partial), "Partial cost is not silently ignored");
foreach (var quantity in new[] { 0m, -1m, .00001m, 100000000000000m })
{
    var request = Request(); request.CurrentCost!.PresentationQuantity = quantity;
    await Reject(() => service.SaveCostItemAsync(request), $"Invalid presentation quantity {quantity}");
}
foreach (var value in new[] { -1m, .0000001m, 1000000000000m })
{
    var request = Request(); request.CurrentCost!.PresentationCost = value;
    await Reject(() => service.SaveCostItemAsync(request), $"Invalid presentation cost {value}");
}
var zero = Request(); zero.CurrentCost!.PresentationCost = 0;
Check(await service.SaveCostItemAsync(zero) == 1, "Zero cost remains valid");
var noCost = Request(); noCost.CurrentCost = null;
Check(await service.SaveCostItemAsync(noCost) == 1, "Concept can be created without cost");

await Reject(() => service.SaveRecipeAsync(new() { ProductId = 1 }), "Empty product recipe");
await Reject(() => service.SaveRecipeAsync(new() { ProductId = 1, Items = new()
    { new() { CostItemCostId = 1, Quantity = 1 }, new() { CostItemCostId = 1, Quantity = 2 } } }), "Duplicate cost version");
await Reject(() => service.SaveRecipeAsync(new() { ProductId = 1, Items = new()
    { new() { CostItemCostId = 1, Quantity = .00001m } } }), "Recipe quantity precision");
Check(await service.SaveRecipeAsync(new() { ProductId = 1,
    Bases = new() { new() { BaseRecipeId = 1, QuantityMultiplier = .5m } } }) == 1, "Base-only product supported");

var today = DateTime.Today;
var costs = new Dictionary<int, CostItemCost>
{
    [1] = new() { CostItemCostId = 1, CostItemId = 1, UnitCost = .0162m, EffectiveFrom = today,
        CostItem = new() { CostItemId = 1, IsActive = true, CostItemTypeCode = "RawMaterial", UnitCode = "Gram" } },
    [2] = new() { CostItemCostId = 2, CostItemId = 2, UnitCost = .5619m, EffectiveFrom = today,
        CostItem = new() { CostItemId = 2, IsActive = true, CostItemTypeCode = "Packaging", UnitCode = "Piece" } },
    [3] = new() { CostItemCostId = 3, CostItemId = 3, UnitCost = 1.257862m, EffectiveFrom = today,
        CostItem = new() { CostItemId = 3, IsActive = true, CostItemTypeCode = "Service", UnitCode = "Application" } },
    [4] = new() { CostItemCostId = 4, CostItemId = 4, UnitCost = 105m, EffectiveFrom = today,
        CostItem = new() { CostItemId = 4, IsActive = true, CostItemTypeCode = "Resale", UnitCode = "Piece" } }
};
var items = new List<SaveProductRecipeItemRequestDto>
{
    new() { CostItemCostId = 1, Quantity = 400 }, new() { CostItemCostId = 2, Quantity = 1 },
    new() { CostItemCostId = 3, Quantity = 1 }
};
object? Invoke(string method, params object[] args)
{
    try { return typeof(CostingRepository).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, args); }
    catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException argument) { throw argument; }
}
Task Validate(List<SaveProductRecipeItemRequestDto> selected, bool isBaseRecipe = false, int[]? existing = null)
{
    Invoke("ValidateCostSelection", selected, costs, today, existing ?? Array.Empty<int>(), isBaseRecipe);
    return Task.CompletedTask;
}
await Validate(items);
var calculated = ((IEnumerable<ProductRecipeItem>)Invoke("BuildRecipeItems", items, costs)!).ToList();
Check(calculated.Sum(item => item.RecipeItemCost) == 8.299762m, "Gram, piece and service costs add correctly");
await Validate(items, true);
Check(true, "Base accepts raw materials, packaging and services");
await Reject(() => Validate(new() { new() { CostItemCostId = 4, Quantity = 1 } }, true), "Resale excluded from base recipes");
await Validate(new() { new() { CostItemCostId = 4, Quantity = 1 } });
Check(((IEnumerable<ProductRecipeItem>)Invoke("BuildRecipeItems",
    new List<SaveProductRecipeItemRequestDto> { new() { CostItemCostId = 4, Quantity = 1 } }, costs)!).Single().RecipeItemCost == 105m,
    "Resale jar costs 105 per piece, not per gram");
await Reject(() => Validate(new() { new() { CostItemCostId = 99, Quantity = 1 } }), "Missing cost rejected");
costs[5] = new() { CostItemCostId = 5, CostItemId = 1, UnitCost = .02m, EffectiveFrom = today, CostItem = costs[1].CostItem };
await Reject(() => Validate(new() { items[0], new() { CostItemCostId = 5, Quantity = 1 } }), "Two versions of same concept rejected");
costs[1].EffectiveTo = today.AddDays(-1);
await Reject(() => Validate(new() { items[0] }), "Expired cost cannot be newly selected");
await Validate(new() { items[0] }, existing: new[] { 1 });
Check(true, "Existing historical cost reference preserved");
costs[1].EffectiveTo = null; costs[1].EffectiveFrom = today.AddDays(1);
await Reject(() => Validate(new() { items[0] }), "Future cost cannot be selected early");
costs[1].EffectiveFrom = today; costs[1].CostItem.IsActive = false;
await Reject(() => Validate(new() { items[0] }), "Inactive concept cannot be newly selected");

using var db = new SmartOrderContext(new DbContextOptionsBuilder<SmartOrderContext>()
    .UseSqlServer("Server=unused;Database=unused;Integrated Security=true").Options);
Check(db.Model.FindEntityType(typeof(CostItem))!.GetTableName() == "CostItems", "CostItems SQL mapping");
var mapping = db.Model.FindEntityType(typeof(CostItemCost))!;
Check(mapping.GetTableName() == "CostItemCosts", "CostItemCosts SQL mapping");
var costColumnType = mapping.FindProperty(nameof(CostItemCost.PresentationCost))!.GetColumnType();
Check(costColumnType?.Replace(" ", "") == "decimal(18,6)", $"Presentation cost precision mapping ({costColumnType})");
Check(mapping.FindProperty(nameof(CostItemCost.UnitCost))!.GetComputedColumnSql()!.Contains("[PresentationQuantity]"), "Computed unit cost uses generic quantity");
var sql = db.ProductRecipeItems.Include(item => item.CostItemCost).ThenInclude(cost => cost.CostItem).ToQueryString();
Check(sql.Contains("[CostItemCosts]") && sql.Contains("[UnitCode]") && !sql.Contains("Ingredient"), "Recipe query uses new schema");
var json = JsonSerializer.Serialize(Request("Service", "Application"), new JsonSerializerOptions(JsonSerializerDefaults.Web));
Check(json.Contains("costItemTypeCode") && json.Contains("presentationQuantity") && !json.Contains("Grams"), "Generic API contract");
Console.WriteLine($"Completed {passed} checks without database access.");

sealed class RecordingRepository : ICostingRepository
{
    public SaveCostItemRequestDto? Last { get; private set; }
    public Task<int> SaveCostItemAsync(SaveCostItemRequestDto request) { Last = request; return Task.FromResult(1); }
    public Task<int> SaveRecipeAsync(SaveProductRecipeRequestDto request) => Task.FromResult(1);
    public Task<int> SaveBaseRecipeAsync(SaveBaseRecipeRequestDto request) => Task.FromResult(1);
    public Task<IEnumerable<CostItemDto>> GetCostItemsAsync() => throw new NotSupportedException();
    public Task<CostItemDto?> GetCostItemAsync(int id) => throw new NotSupportedException();
    public Task<IEnumerable<ProductRecipeDto>> GetRecipesAsync() => throw new NotSupportedException();
    public Task<IEnumerable<BaseRecipeDto>> GetBaseRecipesAsync() => throw new NotSupportedException();
    public Task<ProductRecipeDto?> GetCurrentRecipeAsync(int id) => throw new NotSupportedException();
    public Task<IEnumerable<ProductCostingIssueDto>> GetProductsWithoutCostingAsync() => throw new NotSupportedException();
}
