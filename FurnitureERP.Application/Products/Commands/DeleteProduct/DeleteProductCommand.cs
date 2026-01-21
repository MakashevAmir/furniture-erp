using MediatR;

namespace FurnitureERP.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest;
