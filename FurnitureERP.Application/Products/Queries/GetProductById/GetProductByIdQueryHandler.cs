using AutoMapper;
using FurnitureERP.Application.Materials.Queries.GetMaterials;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IMediator mediator,
        IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            throw new ProductNotFoundException(request.Id);

        var productDto = _mapper.Map<ProductDto>(product);

        var materialBoms = product.MaterialBoms?.Select(mb => _mapper.Map<MaterialBomDto>(mb)).ToList();
        var laborBoms = product.LaborBoms?.Select(lb => _mapper.Map<LaborBomDto>(lb)).ToList();

        if (materialBoms != null && materialBoms.Any())
        {
            var materialsQuery = new GetMaterialsQuery();
            var materials = await _mediator.Send(materialsQuery, cancellationToken);
            var materialDict = materials.ToDictionary(m => m.Id, m => m.Name);

            foreach (var mb in materialBoms)
            {
                mb.MaterialName = materialDict.ContainsKey(mb.MaterialId) ? materialDict[mb.MaterialId] : "Unknown";
            }
        }

        return productDto with
        {
            MaterialBoms = materialBoms,
            LaborBoms = laborBoms
        };
    }
}
