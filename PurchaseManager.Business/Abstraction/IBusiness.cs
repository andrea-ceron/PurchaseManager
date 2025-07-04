using PurchaseManager.Repository;
using PurchaseManager.Repository.Model;
using PurchaseManager.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Business.Abstraction
{
    public interface IBusiness
    {
        #region Supplier
        public Task CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default);
        public Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default);
        public Task UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default);
		public Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default);
        #endregion

        #region SupplierOrder 
        public Task<List<UpdateRawMaterialQuantity>> CreateSupplierOrderAsync(CreateSupplierOrderDto createSupplierOrderDto, CancellationToken ct = default);
        public Task<ReadSupplierOrderDto> GetSupplierOrderByIdAsync(int SupplierOrderId, CancellationToken ct = default);
        public Task<List<ReadSupplierOrderDto>?> GetAllSupplierOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default);
        #endregion

        #region RawMaterial
        public Task CreateListOfRawMaterialsAsync(IEnumerable<CreateRawMaterialDto> RawMaterialDto, CancellationToken ct = default);
        public Task<List<ReadRawMaterialDto>> GetRawMaterialListBySupplierId(int SupplierId, CancellationToken ct = default);
        public Task UpdateRawMaterialAsync(UpdateRawMaterialDto RawMaterialDto, CancellationToken ct = default);
		public Task DeleteRawMaterialAsync(int RawMaterialId, CancellationToken ct = default);
        #endregion

    }
}
