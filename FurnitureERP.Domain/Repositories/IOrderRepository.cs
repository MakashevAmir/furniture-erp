using FurnitureERP.Domain.Aggregates.Orders;

namespace FurnitureERP.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    IQueryable<Order> GetAll();

    IQueryable<Order> GetActiveOrders();

    IQueryable<Order> GetOrdersByStatus(OrderStatus status);

    IQueryable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate);

    IQueryable<Order> GetOrdersByCustomer(string customerName);

    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);

    void Update(Order order);

    void Delete(Order order);
}
