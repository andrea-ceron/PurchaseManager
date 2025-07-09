namespace PurchaseManager.Shared.DTO
{
	public class CreateSupplierDto
	{
		public required string Email { get; set; }
		public required string Phone { get; set; }
		public required string CompanyName { get; set; }
		public required string VATNumber { get; set; }
		public required string TaxCode { get; set; }
		public required string CertifiedEmail { get; set; }

	}

	public class ReadSupplierDto
	{
		public required int Id { get; set; }
		public required string Email { get; set; }
		public required string Phone { get; set; }
		public required string CompanyName { get; set; }
		public required string VATNumber { get; set; }
		public required string TaxCode { get; set; }
		public required string CertifiedEmail { get; set; }
		public List<ReadSupplierOrderDto> SupplierOrders { get; set; }
		public List<ReadRawMaterialDto> RawMaterials { get; set; }


	}

	public class UpdateSupplierDto
	{
		public int Id { get; set; }
		public required string Email { get; set; }
		public required string Phone { get; set; }
		public required string CompanyName { get; set; }
		public required string VATNumber { get; set; }
		public required string TaxCode { get; set; }
		public required string CertifiedEmail { get; set; }


	}

}
