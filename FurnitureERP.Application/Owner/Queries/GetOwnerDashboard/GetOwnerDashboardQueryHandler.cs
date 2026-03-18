using FurnitureERP.Application.Owner.DTOs;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Owner.Queries.GetOwnerDashboard;

public class GetOwnerDashboardQueryHandler : IRequestHandler<GetOwnerDashboardQuery, OwnerDashboardDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public GetOwnerDashboardQueryHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMaterialRepository materialRepository,
        IEmployeeRepository employeeRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _materialRepository = materialRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<OwnerDashboardDto> Handle(GetOwnerDashboardQuery request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1);

        var (completedOrders, allMonthOrders) = await LoadOrdersAsync(startDate, endDate, cancellationToken);

        var allOrderItems = completedOrders.SelectMany(o => o.OrderItems).ToList();

        var productMap = await LoadProductsAsync(allOrderItems, cancellationToken);
        var materialMap = await LoadMaterialsAsync(productMap, cancellationToken);
        var employeeMap = await LoadEmployeesAsync(productMap, cancellationToken);

        var revenue = completedOrders.Sum(o => o.TotalAmount);
        var (materialCosts, laborCosts, topProducts, employeeWork) =
            CalculateCostsAndStats(allOrderItems, productMap, materialMap, employeeMap);

        return new OwnerDashboardDto(
            Revenue: revenue,
            MaterialCosts: materialCosts,
            LaborCosts: laborCosts,
            NetProfit: revenue - materialCosts - laborCosts,
            CompletedOrdersCount: completedOrders.Count,
            TotalOrdersCount: allMonthOrders.Count,
            TopProducts: topProducts,
            EmployeeWork: employeeWork,
            Year: request.Year,
            Month: request.Month);
    }

    private Task<(List<Order> completed, List<Order> all)> LoadOrdersAsync(
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var allOrders = _orderRepository.GetAll()
            .Where(o => (o.OrderDate >= startDate && o.OrderDate < endDate)
                || (o.ActualCompletionDate >= startDate && o.ActualCompletionDate < endDate))
            .ToList();

        var completedOrders = allOrders
            .Where(o => (o.Status == OrderStatus.Completed || o.Status == OrderStatus.Delivered)
                && o.ActualCompletionDate >= startDate
                && o.ActualCompletionDate < endDate)
            .ToList();

        var allMonthOrders = allOrders
            .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
            .ToList();

        return Task.FromResult((completedOrders, allMonthOrders));
    }

    private async Task<Dictionary<int, Domain.Aggregates.Products.Product>> LoadProductsAsync(
        IEnumerable<Domain.Aggregates.Orders.OrderItem> items, CancellationToken cancellationToken)
    {
        var productIds = items.Select(i => i.ProductId).Distinct().ToList();
        var result = new Dictionary<int, Domain.Aggregates.Products.Product>();
        foreach (var id in productIds)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (product != null)
                result[product.Id] = product;
        }
        return result;
    }

    private async Task<Dictionary<int, Domain.Aggregates.Materials.Material>> LoadMaterialsAsync(
        Dictionary<int, Domain.Aggregates.Products.Product> productMap, CancellationToken cancellationToken)
    {
        var materialIds = productMap.Values
            .SelectMany(p => p.MaterialBoms.Select(mb => mb.MaterialId))
            .Distinct().ToList();
        var result = new Dictionary<int, Domain.Aggregates.Materials.Material>();
        foreach (var id in materialIds)
        {
            var material = await _materialRepository.GetByIdAsync(id, cancellationToken);
            if (material != null)
                result[material.Id] = material;
        }
        return result;
    }

    private async Task<Dictionary<int, Domain.Aggregates.Employees.Employee>> LoadEmployeesAsync(
        Dictionary<int, Domain.Aggregates.Products.Product> productMap, CancellationToken cancellationToken)
    {
        var employeeIds = productMap.Values
            .SelectMany(p => p.LaborBoms.Where(lb => lb.EmployeeId.HasValue).Select(lb => lb.EmployeeId!.Value))
            .Distinct().ToList();
        var result = new Dictionary<int, Domain.Aggregates.Employees.Employee>();
        foreach (var id in employeeIds)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee != null)
                result[employee.Id] = employee;
        }
        return result;
    }

    private static (decimal materialCosts, decimal laborCosts, List<TopProductDto> topProducts, List<EmployeeWorkDto> employeeWork)
        CalculateCostsAndStats(
            IEnumerable<Domain.Aggregates.Orders.OrderItem> items,
            Dictionary<int, Domain.Aggregates.Products.Product> productMap,
            Dictionary<int, Domain.Aggregates.Materials.Material> materialMap,
            Dictionary<int, Domain.Aggregates.Employees.Employee> employeeMap)
    {
        var materialCosts = 0m;
        var laborCosts = 0m;
        var topProducts = new Dictionary<int, (string Name, string Article, int Qty, decimal Revenue)>();
        var employeeWork = new Dictionary<string, (string Position, decimal Hours, decimal Cost)>();

        foreach (var item in items)
        {
            if (!productMap.TryGetValue(item.ProductId, out var product))
                continue;

            AccumulateTopProduct(topProducts, item);
            materialCosts += CalculateItemMaterialCost(product, item.Quantity, materialMap);
            laborCosts += CalculateItemLaborCost(product, item.Quantity, employeeMap, employeeWork);
        }

        var topProductsList = topProducts.Values
            .OrderByDescending(tp => tp.Qty)
            .Take(5)
            .Select(tp => new TopProductDto(tp.Name, tp.Article, tp.Qty, tp.Revenue))
            .ToList();

        var employeeWorkList = employeeWork
            .Select(kvp => new EmployeeWorkDto(kvp.Key, kvp.Value.Position, kvp.Value.Hours, kvp.Value.Cost))
            .OrderByDescending(ew => ew.TotalHours)
            .ToList();

        return (materialCosts, laborCosts, topProductsList, employeeWorkList);
    }

    private static void AccumulateTopProduct(
        Dictionary<int, (string Name, string Article, int Qty, decimal Revenue)> topProducts,
        Domain.Aggregates.Orders.OrderItem item)
    {
        if (!topProducts.TryGetValue(item.ProductId, out var existing))
            existing = (item.ProductName, item.ProductArticle, 0, 0m);

        topProducts[item.ProductId] = (existing.Name, existing.Article,
            existing.Qty + item.Quantity, existing.Revenue + item.Subtotal);
    }

    private static decimal CalculateItemMaterialCost(
        Domain.Aggregates.Products.Product product,
        int quantity,
        Dictionary<int, Domain.Aggregates.Materials.Material> materialMap)
    {
        return product.MaterialBoms
            .Where(bom => materialMap.ContainsKey(bom.MaterialId))
            .Sum(bom => bom.QuantityWithWastage * quantity * materialMap[bom.MaterialId].PricePerUnit);
    }

    private static decimal CalculateItemLaborCost(
        Domain.Aggregates.Products.Product product,
        int quantity,
        Dictionary<int, Domain.Aggregates.Employees.Employee> employeeMap,
        Dictionary<string, (string Position, decimal Hours, decimal Cost)> employeeWork)
    {
        var totalLaborCost = 0m;

        foreach (var lb in product.LaborBoms)
        {
            var hours = lb.HoursRequired * quantity;

            if (lb.EmployeeId.HasValue && employeeMap.TryGetValue(lb.EmployeeId.Value, out var emp))
            {
                var cost = hours * emp.HourlyRate;
                totalLaborCost += cost;

                if (!employeeWork.TryGetValue(emp.FullName, out var ew))
                    ew = (emp.Position, 0m, 0m);
                employeeWork[emp.FullName] = (ew.Position, ew.Hours + hours, ew.Cost + cost);
            }
            else
            {
                var key = $"[{lb.Position}]";
                if (!employeeWork.TryGetValue(key, out var ew))
                    ew = (lb.Position, 0m, 0m);
                employeeWork[key] = (ew.Position, ew.Hours + hours, ew.Cost);
            }
        }

        return totalLaborCost;
    }
}
