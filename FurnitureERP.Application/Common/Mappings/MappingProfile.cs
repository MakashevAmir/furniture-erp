using AutoMapper;
using FurnitureERP.Application.Employees.DTOs;
using FurnitureERP.Application.Materials.DTOs;
using FurnitureERP.Application.Orders.DTOs;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Aggregates.Employees;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Aggregates.Products;

namespace FurnitureERP.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForCtorParam("MaterialBoms", opt => opt.MapFrom(_ => (List<MaterialBomDto>?)null))
            .ForCtorParam("LaborBoms", opt => opt.MapFrom(_ => (List<LaborBomDto>?)null));

        CreateMap<MaterialBom, MaterialBomDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.QuantityRequired))
            .ForMember(dest => dest.MaterialName, opt => opt.Ignore());

        CreateMap<LaborBom, LaborBomDto>()
            .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.HoursRequired));

        CreateMap<Material, MaterialDto>();
        CreateMap<Employee, EmployeeDto>();

        CreateMap<Order, OrderDto>()
            .ForCtorParam("TotalAmount", opt => opt.MapFrom(src =>
                src.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)));

        CreateMap<OrderItem, OrderItemDto>();
    }
}
