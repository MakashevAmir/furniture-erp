using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterialStock;

public class UpdateMaterialStockCommandHandler : IRequestHandler<UpdateMaterialStockCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMaterialStockCommandHandler(
        IMaterialRepository materialRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(UpdateMaterialStockCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var material = await _materialRepository.GetByIdAsync(request.Id, cancellationToken);

        if (material == null)
            throw new DomainException($"Materiál s ID {request.Id} nebyl nalezen");

        material.UpdateStock(request.Quantity);

        _materialRepository.Update(material);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
