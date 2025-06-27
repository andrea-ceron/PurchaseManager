using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Shared.DTO
{
    public class CreateProductDto
	{
        public int SupplierProductCode { get; set; }
        public decimal Price { get; set; }
		public int? SupplierId { get; set; }

		public int MinQuantityForOrder { get; set; }
		public int AvailableQuantity { get; set; }
		public string ProductName { get; set; } = string.Empty;


	}

	public class CreateProductFromSupplierControllerDto
	{
		public int SupplierProductCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantityForOrder { get; set; }
		public int AvailableQuantity { get; set; }
		public string ProductName { get; set; } = string.Empty;


	}

	public class ReadProductDto
	{
		public int Id { get; set; }
		public int SupplierProductCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantityForOrder { get; set; }
		public List<ReadProductOrderDto> ProductOrders { get; set; }
		public int SupplierId { get; set; }
		public int AvailableQuantity { get; set; }
		public string ProductName { get; set; } = string.Empty;

	}


	public class UpdateProductDto
	{
		public int? Id { get; set; }
		public int? SupplierProductCode { get; set; }
		public decimal? Price { get; set; }
		public int SupplierId { get; set; }
		public int MinQuantityForOrder { get; set; }
		public int AvailableQuantity { get; set; }

	}

	public class ProductDtoForKafka
	{
		public int Id { get; set; }
		public int AvailableQuantity { get; set; }
		public int SupplierId { get; set; }
		public string ProductName { get; set; } = string.Empty;
	}


}
