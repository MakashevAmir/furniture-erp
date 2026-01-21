using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Commands.CreateMaterial;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialCommandHandler(
        IMaterialRepository materialRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var material = new Material(
            request.Name,
            request.Description,
            request.Category,
            request.Unit,
            request.PricePerUnit,
            request.CurrentStock,
            request.MinimumStock,
            request.Supplier
        );

        await _materialRepository.AddAsync(material, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return material.Id;
    }
}
