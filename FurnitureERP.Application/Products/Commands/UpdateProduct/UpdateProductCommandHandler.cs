using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            throw new ProductNotFoundException(request.Id);

        product.Update(
            request.Name,
            request.Description,
            request.Category,
            request.BasePrice,
            request.SalePrice
        );

        product.SetProductionDays(request.ProductionDays);

        if (request.IsActive && !product.IsActive)
            product.Activate();
        else if (!request.IsActive && product.IsActive)
            product.Deactivate();

        var existingMaterialIds = product.MaterialBoms.Select(m => m.MaterialId).ToList();
        foreach (var materialId in existingMaterialIds)
        {
            product.RemoveMaterialBom(materialId);
        }

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

        var existingLaborIds = product.LaborBoms.Select(l => l.Id).ToList();
        foreach (var laborId in existingLaborIds)
        {
            product.RemoveLaborBom(laborId);
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

        _productRepository.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
