namespace PurchaseManager.Repository.Model;

    public class Product
    {
	public int Id { get; set; }
	public int SupplierProductCode { get; set; }
	public decimal Price { get; set; }
	public int MinQuantityForOrder { get; set; }
	public int AvailableQuantity { get; set; }
	public List<ProductOrder>? ProductOrders { get; set; }
	public int SupplierId { get; set; }
	public Supplier Supplier { get; set; }
	public string ProductName { get; set; } = string.Empty;

}
