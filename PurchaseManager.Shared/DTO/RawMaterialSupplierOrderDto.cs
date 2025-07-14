namespace PurchaseManager.Shared.DTO;
public class CreateRawMaterialSupplierOrderDto
{
	public int RawMaterialId { get; set; }
	public int? SupplierOrderId { get; set; } = null;
	public int Quantity { get; set; }
}

public class CreateRawMaterialSupplierOrderFromSupplierOrderControllerDto
{
	public int RawMaterialId { get; set; }
	public int Quantity { get; set; }
}

public class UpdateRawMaterialSupplierOrderDto
{
	public int Id { get; set; }
	public UpdateRawMaterialDto RawMaterial { get; set; }
	public UpdateSupplierOrderDto SupplierOrder { get; set; }
	public int Quantity { get; set; }
}

public class ReadRawMaterialSupplierOrderDto
{
	public int Id { get; set; }
	public int Quantity { get; set; }
	public int SupplierOrderId { get; set; } 
	public int RawMaterialId { get; set; }
}
