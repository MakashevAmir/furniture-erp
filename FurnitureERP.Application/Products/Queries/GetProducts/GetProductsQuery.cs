using FurnitureERP.Application.Products.DTOs;
using MediatR;

namespace FurnitureERP.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;
