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
		public List<CreateProductFromSupplierControllerDto> Products { get; set; }

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
		public List<ReadOrderDto> Orders { get; set; }
		public List<ReadProductDto> Products { get; set; }


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
