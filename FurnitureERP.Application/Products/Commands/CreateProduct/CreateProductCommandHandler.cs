using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var product = new Product(
            request.Name,
            request.Description,
            request.Category,
            request.Article,
            request.BasePrice,
            request.SalePrice
        );

        await _productRepository.AddAsync(product, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (request.Materials != null && request.Materials.Any())
        {
            foreach (var materialDto in request.Materials)
            {
                var materialBom = new MaterialBom(
                    product.Id,
                    materialDto.MaterialId,
                    materialDto.Quantity,
                    materialDto.WastagePercentage,
                    materialDto.Notes
                );

                product.AddMaterialBom(materialBom);
            }
        }

        if (request.LaborOperations != null && request.LaborOperations.Any())
        {
            foreach (var laborDto in request.LaborOperations)
            {
                var laborBom = new LaborBom(
                    product.Id,
                    laborDto.Position,
                    laborDto.Hours,
                    laborDto.Description
                );

                product.AddLaborBom(laborBom);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
