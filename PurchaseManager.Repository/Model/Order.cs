namespace PurchaseManager.Repository.Model;

    public class Order
    {
	public int Id { get; set; }
	public DateTime DeliveryDate { get; set; }
	public List<ProductOrder>? ProductOrder { get; set; }
	public int SupplierId { get; set; }
	public Supplier Supplier { get; set; }
}
 