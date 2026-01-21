using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterial;

public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMaterialCommandHandler(
        IMaterialRepository materialRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var material = await _materialRepository.GetByIdAsync(request.Id, cancellationToken);

        if (material == null)
            throw new DomainException($"Materiál s ID {request.Id} nebyl nalezen");

        material.Update(
            request.Name,
            request.Description,
            request.Category,
            request.Unit,
            request.PricePerUnit,
            request.Supplier
        );

        var stockDifference = request.CurrentStock - material.CurrentStock;
        if (stockDifference != 0)
            material.UpdateStock(stockDifference);

        material.SetMinimumStock(request.MinimumStock);

        if (request.IsActive && !material.IsActive)
        {
            material.Activate();
        }
        else if (!request.IsActive && material.IsActive)
        {
            material.Deactivate();
        }

        _materialRepository.Update(material);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
