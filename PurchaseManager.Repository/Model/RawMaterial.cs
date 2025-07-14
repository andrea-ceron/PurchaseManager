namespace PurchaseManager.Repository.Model;

    public class RawMaterial
    {
	public int Id { get; set; }
	public int SupplierRawMaterialCode { get; set; }
	public decimal Price { get; set; }
	public int MinQuantityForSupplierOrder { get; set; }
	public List<RawMaterialSupplierOrder> RawMaterialSupplierOrders { get; set; } = new();
	public int SupplierId { get; set; }
	public Supplier Supplier { get; set; }
	public string RawMaterialName { get; set; } = string.Empty;

}
