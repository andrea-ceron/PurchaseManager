using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Shared.DTO
{
    public class CreateRawMaterialDto
	{
        public int SupplierRawMaterialCode { get; set; }
        public decimal Price { get; set; }
		public int? SupplierId { get; set; }

		public int MinQuantityForSupplierOrder { get; set; }
		public string RawMaterialName { get; set; } = string.Empty;


	}

	public class CreateRawMaterialFromSupplierControllerDto
	{
		public int SupplierRawMaterialCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantityForSupplierOrder { get; set; }
		public string RawMaterialName { get; set; } = string.Empty;


	}

	public class ReadRawMaterialDto
	{
		public int Id { get; set; }
		public int SupplierRawMaterialCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantityForSupplierOrder { get; set; }
		public List<ReadRawMaterialSupplierOrderDto> RawMaterialSupplierOrders { get; set; }
		public int SupplierId { get; set; }
		public string RawMaterialName { get; set; } = string.Empty;

	}


	public class UpdateRawMaterialDto
	{
		public int? Id { get; set; }
		public int? SupplierRawMaterialCode { get; set; }
		public decimal? Price { get; set; }
		public int SupplierId { get; set; }
		public int MinQuantityForSupplierOrder { get; set; }
		public string RawMaterialName { get; set; } = string.Empty;

	}

	public class RawMaterialDtoForKafka
	{
		public int Id { get; set; }
		public int SupplierId { get; set; }
		public string RawMaterialName { get; set; } = string.Empty;
	}

	public class UpdateRawMaterialQuantity
	{
		public int Id { get; set; }
		public int QuantityToAdd { get; set; }

	}


}
