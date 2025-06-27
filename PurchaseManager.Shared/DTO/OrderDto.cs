using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Shared.DTO
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
		public int SupplierId { get; set; }

    }

	public class ReadOrderDto
	{
		public int Id { get; set; }
		public DateTime DeliveryDate { get; set; }
		public List<ReadProductOrderDto> ProductOrder { get; set; }
		public int SupplierId { get; set; }

	}

	public class CreateOrderDto
	{
		public DateTime Delivery { get; set; }
		public int SupplierId { get; set; }
		public List<CreateProductOrderFromOrderControllerDto> ProductOrder { get; set; }
	}
}
