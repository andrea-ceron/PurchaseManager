using Microsoft.EntityFrameworkCore;
using PurchaseManager.Repository.Model;
namespace PurchaseManager.Repository;

public class SupplierDbContext(DbContextOptions<SupplierDbContext> options): DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<Supplier>().HasKey(s => s.Id);
		mb.Entity<Supplier>().HasMany(s => s.Orders)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Supplier>().HasMany(s => s.Products)
			.WithOne(o => o.Supplier)
			.HasForeignKey(o => o.SupplierId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Supplier>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<Product>().HasKey(p => p.Id);
		mb.Entity<Product>().HasMany(p => p.ProductOrders)
					.WithOne(po => po.Product)
					.HasForeignKey(po => po.ProductId)
					.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Product>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<Order>().HasKey(o => o.Id);
		mb.Entity<Order>().HasMany(o => o.ProductOrder)
			.WithOne(po => po.Order)
			.HasForeignKey(po => po.OrderId)
			.OnDelete(DeleteBehavior.Restrict);
		mb.Entity<Order>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<ProductOrder>().HasKey(s => s.Id);
		mb.Entity<ProductOrder>()
			.Property(s => s.Id)
			.ValueGeneratedOnAdd();

		mb.Entity<TransactionalOutbox>().HasKey(s => s.Id);

		base.OnModelCreating(mb);
	}
	public DbSet<Supplier> Suppliers { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductOrder> ProductOrders { get; set; }
	public DbSet<TransactionalOutbox> TransactionalOutboxes { get; set; }


}
