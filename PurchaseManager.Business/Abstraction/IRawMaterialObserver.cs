namespace PurchaseManager.Business.Abstraction;

public interface IRawMaterialObserver
{
	IObserver<int> AddRawMaterialPurchaseToStock { get; }

}
