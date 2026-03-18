using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Commands.ReceiveMaterialStock;

public class ReceiveMaterialStockCommandHandler : IRequestHandler<ReceiveMaterialStockCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReceiveMaterialStockCommandHandler(
        IMaterialRepository materialRepository,
        IStockTransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(ReceiveMaterialStockCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Množství příjmu musí být kladné");

        var material = await _materialRepository.GetByIdAsync(request.MaterialId, cancellationToken);
        if (material == null)
            throw new NotFoundException($"Materiál s ID {request.MaterialId} nebyl nalezen");

        var transaction = new StockTransaction(
            material.Id,
            material.Name,
            material.Unit,
            request.Quantity,
            material.CurrentStock,
            StockTransactionType.Purchase,
            notes: request.Notes ?? string.Empty);

        material.UpdateStock(request.Quantity);

        _materialRepository.Update(material);
        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
