namespace PurchaseManager.Shared.DTO
{
	public class CreateSupplierDto
	{
		public string Email { get; set; }
		public string Phone { get; set; }
		public string CompanyName { get; set; }
		public string VATNumber { get; set; }
		public string TaxCode { get; set; }
		public string CertifiedEmail { get; set; }
		public List<CreateRawMaterialFromSupplierControllerDto> RawMaterials { get; set; }

	}

	public class ReadSupplierDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string CompanyName { get; set; }
		public string VATNumber { get; set; }
		public string TaxCode { get; set; }
		public string CertifiedEmail { get; set; }
		public List<ReadSupplierOrderDto> SupplierOrders { get; set; }
		public List<ReadRawMaterialDto> RawMaterials { get; set; }


	}

	public class UpdateSupplierDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string CompanyName { get; set; }
		public string VATNumber { get; set; }
		public string TaxCode { get; set; }
		public string CertifiedEmail { get; set; }


	}

}
