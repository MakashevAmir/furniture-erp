using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var trackedOrder = _context.Orders.Local.FirstOrDefault(o => o.Id == id);
        if (trackedOrder != null)
        {
            _context.Entry(trackedOrder).State = EntityState.Detached;
        }

        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            return null;

        var existingOrder = _context.Orders.Local.FirstOrDefault(o => o.OrderNumber == orderNumber.Trim());
        if (existingOrder != null)
        {
            _context.Entry(existingOrder).State = EntityState.Detached;
        }

        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber.Trim(), cancellationToken);
    }

    public IQueryable<Order> GetAll()
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking();
    }

    public IQueryable<Order> GetActiveOrders()
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InProduction)
            .AsNoTracking();
    }

    public IQueryable<Order> GetOrdersByStatus(OrderStatus status)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Status == status)
            .AsNoTracking();
    }

    public IQueryable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .AsNoTracking();
    }

    public IQueryable<Order> GetOrdersByCustomer(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            return Enumerable.Empty<Order>().AsQueryable();

        var searchTerm = customerName.Trim().ToLower();
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerName.ToLower().Contains(searchTerm))
            .AsNoTracking();
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        await _context.Orders.AddAsync(order, cancellationToken);
        return order;
    }

    public void Update(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        _context.Orders.Update(order);
    }

    public void Delete(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        _context.Orders.Remove(order);
    }
}
