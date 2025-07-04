namespace PurchaseManager.Repository.Model;

    public class SupplierOrder
    {
	public int Id { get; set; }
	public List<RawMaterialSupplierOrder>? RawMaterialSupplierOrder { get; set; }
	public int SupplierId { get; set; }
	public Supplier Supplier { get; set; }
}
 