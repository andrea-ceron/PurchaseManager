namespace PurchaseManager.Repository.Model;

    public class Supplier
    {
	public int Id { get; set; }
	required public string Email { get; set; }
	required public string Phone { get; set; }
	required public string CompanyName { get; set; }
	required public string VATNumber { get; set; }
	required public string TaxCode { get; set; }
	required public string CertifiedEmail { get; set; }
	public List<SupplierOrder> SupplierOrders { get; set; } = new();
	public List<RawMaterial> RawMaterials { get; set; } = new();
}
