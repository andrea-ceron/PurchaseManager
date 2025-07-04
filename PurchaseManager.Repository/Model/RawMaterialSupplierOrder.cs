using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Repository.Model
{
    public class RawMaterialSupplierOrder
    {
        public int Id { get; set; }
        public RawMaterial RawMaterial { get; set; }
        public int RawMaterialId { get; set; }
        public SupplierOrder SupplierOrder { get; set; }
        public int SupplierOrderId { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
    }
}
