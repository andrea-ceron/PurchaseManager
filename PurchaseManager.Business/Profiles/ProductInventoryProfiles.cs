using AutoMapper;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using System.Diagnostics.CodeAnalysis;


namespace PurchaseManager.Business.Profiles;

/// <summary>
/// Marker per <see cref="AutoMapper"/>.
/// </summary>
public sealed class AssemblyMarker
{
	AssemblyMarker() { }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class InputFileProfile : Profile
{
	public InputFileProfile()
	{
		CreateMap<CreateSupplierDto, Supplier>();
		CreateMap<Supplier, ReadSupplierDto>();
		CreateMap<UpdateSupplierDto, Supplier>();

		CreateMap<CreateOrderDto, Order>();
		CreateMap<Order, ReadOrderDto>();
		CreateMap<UpdateOrderDto, Order>();

		CreateMap<CreateProductOrderDto, ProductOrder>();
		CreateMap<CreateProductOrderFromOrderControllerDto, ProductOrder>();

		CreateMap<ProductOrder, ReadProductOrderDto>();
		CreateMap<UpdateProductOrderDto, ProductOrder>();


		CreateMap<CreateProductDto, Product>();
		CreateMap<Product, ReadProductDto>();
		CreateMap<UpdateProductDto, Product>();
		CreateMap<CreateProductFromSupplierControllerDto, Product>();
		CreateMap<Product, ProductDtoForKafka>();


	}
}