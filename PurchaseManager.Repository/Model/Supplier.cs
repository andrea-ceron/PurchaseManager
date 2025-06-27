namespace PurchaseManager.Repository.Model;

    public class Supplier
    {
	public int Id { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public string CompanyName { get; set; }
	public string VATNumber { get; set; }
	public string TaxCode { get; set; }
	public string CertifiedEmail { get; set; }
	public List<Order>? Orders { get; set; }
	public List<Product>? Products { get; set; }
}
