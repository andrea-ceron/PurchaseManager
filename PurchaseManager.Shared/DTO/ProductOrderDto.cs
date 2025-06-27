namespace PurchaseManager.Shared.DTO;

    public class CreateProductOrderDto
    {
	public int ProductId { get; set; }
	public int? OrderId { get; set; } = null;
	public int Quantity { get; set; }
	public int Discount { get; set; }
}

public class CreateProductOrderFromOrderControllerDto
{
	public int ProductId { get; set; }
	public int Quantity { get; set; }
	public int Discount { get; set; }
}

public class UpdateProductOrderDto
{
	public int Id { get; set; }
	public UpdateProductDto Product { get; set; }
	public UpdateOrderDto Order { get; set; }
	public int Quantity { get; set; }
	public int Discount { get; set; }
}
public class ReadProductOrderDto
{
	public int Id { get; set; }
	public int Quantity { get; set; }
	public int Discount { get; set; }
	public int OrderId { get; set; } 
	public int ProductId { get; set; }
}
