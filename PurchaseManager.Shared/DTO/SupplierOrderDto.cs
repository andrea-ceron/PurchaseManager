using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Shared.DTO
{
    public class UpdateSupplierOrderDto
    {
        public int Id { get; set; }
		public int SupplierId { get; set; }

    }

	public class ReadSupplierOrderDto
	{
		public int Id { get; set; }
		public List<ReadRawMaterialSupplierOrderDto> RawMaterialSupplierOrder { get; set; }
		public int SupplierId { get; set; }

	}

	public class CreateSupplierOrderDto
	{
		public int SupplierId { get; set; }
		public List<CreateRawMaterialSupplierOrderFromSupplierOrderControllerDto> RawMaterialSupplierOrder { get; set; }
	}
}
