using Microsoft.EntityFrameworkCore;
using PurchaseManager.Repository.Model;
namespace PurchaseManager.Repository;

public class PurchaseDbContext(DbContextOptions<PurchaseDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<Supplier>().HasKey(s => s.Id);
		mb.Entity<Supplier>().HasMany(s => s.SupplierOrders)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Supplier>().HasMany(s => s.RawMaterials)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Supplier>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<RawMaterial>().HasKey(p => p.Id);
		mb.Entity<RawMaterial>().HasMany(p => p.RawMaterialSupplierOrders)
					.WithOne(po => po.RawMaterial)
					.HasForeignKey(po => po.RawMaterialId)
					.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<RawMaterial>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<SupplierOrder>().HasKey(o => o.Id);
		mb.Entity<SupplierOrder>().HasMany(o => o.RawMaterialSupplierOrder)
			.WithOne(po => po.SupplierOrder)
			.HasForeignKey(po => po.SupplierOrderId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<SupplierOrder>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<RawMaterialSupplierOrder>().HasKey(s => s.Id);
		mb.Entity<RawMaterialSupplierOrder>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<TransactionalOutbox>().HasKey(s => s.Id);

		base.OnModelCreating(mb);
	}
	public DbSet<Supplier> Suppliers { get; set; }
	public DbSet<SupplierOrder> SupplierOrders { get; set; }
	public DbSet<RawMaterial> RawMaterials { get; set; }
	public DbSet<RawMaterialSupplierOrder> RawMaterialSupplierOrders { get; set; }
	public DbSet<TransactionalOutbox> TransactionalOutboxes { get; set; }


}
