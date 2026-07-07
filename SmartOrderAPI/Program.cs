using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Business.Orders.Service;
using SmartOrderAPI.Business.Reports.Service;
using SmartOrderAPI.Business.Service;
using SmartOrderAPI.Data;
using SmartOrderAPI.Data.Configuration;
using SmartOrderAPI.Data.Repositories;
using SmartOrderAPI.Data.Reports.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BusinessTimeOptions>(builder.Configuration.GetSection("BusinessTime"));

builder.Services.AddDbContext<SmartOrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartOrderConnection")));

builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDiscountRuleRepository, DiscountRuleRepository>();
builder.Services.AddScoped<IDiscountRuleService, DiscountRuleService>();
builder.Services.AddScoped<IDiscountRuleTargetRepository, DiscountRuleTargetRepository>();
builder.Services.AddScoped<IDiscountRuleTargetService, DiscountRuleTargetService>();
builder.Services.AddScoped<IDiscountLimitRuleRepository, DiscountLimitRuleRepository>();
builder.Services.AddScoped<IDiscountLimitRuleService, DiscountLimitRuleService>();
builder.Services.AddScoped<IDiscountTargetRepository, DiscountTargetRepository>();
builder.Services.AddScoped<IDiscountTargetService, DiscountTargetService>();
builder.Services.AddScoped<IDiscountTypeRepository, DiscountTypeRepository>();
builder.Services.AddScoped<IDiscountTypeService, DiscountTypeService>();
builder.Services.AddScoped<IOrderDiscountRepository, OrderDiscountRepository>();
builder.Services.AddScoped<IOrderDiscountService, OrderDiscountService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
builder.Services.AddScoped<IPaymentStatusRepository, PaymentStatusRepository>();
builder.Services.AddScoped<IPaymentStatusService, PaymentStatusService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISecurityAccessRepository, SecurityAccessRepository>();
builder.Services.AddScoped<ISecurityAccessService, SecurityAccessService>();
builder.Services.AddScoped<IUserBranchRepository, UserBranchRepository>();
builder.Services.AddScoped<IUserBranchService, UserBranchService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
