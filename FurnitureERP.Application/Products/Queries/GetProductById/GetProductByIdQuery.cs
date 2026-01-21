using FurnitureERP.Application.Products.DTOs;
using MediatR;

namespace FurnitureERP.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;
