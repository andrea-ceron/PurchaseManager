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

		CreateMap<CreateSupplierOrderDto, SupplierOrder>();
		CreateMap<SupplierOrder, ReadSupplierOrderDto>();
		CreateMap<UpdateSupplierOrderDto, SupplierOrder>();

		CreateMap<CreateRawMaterialSupplierOrderDto, RawMaterialSupplierOrder>();
		CreateMap<CreateRawMaterialSupplierOrderFromSupplierOrderControllerDto, RawMaterialSupplierOrder>();

		CreateMap<RawMaterialSupplierOrder, ReadRawMaterialSupplierOrderDto>();
		CreateMap<UpdateRawMaterialSupplierOrderDto, RawMaterialSupplierOrder>();


		CreateMap<CreateRawMaterialDto, RawMaterial>();
		CreateMap<RawMaterial, ReadRawMaterialDto>();
		CreateMap<UpdateRawMaterialDto, RawMaterial>();
		CreateMap<CreateRawMaterialFromSupplierControllerDto, RawMaterial>();
		CreateMap<RawMaterial, RawMaterialDtoForKafka>();
		CreateMap<UpdateRawMaterialDto, RawMaterialDtoForKafka>();
		CreateMap<ReadRawMaterialDto, RawMaterialDtoForKafka>();



	}
}