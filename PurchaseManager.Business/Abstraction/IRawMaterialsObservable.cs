namespace PurchaseManager.Business.Abstraction;

public interface IRawMaterialsObservable
{
	IObservable<int> AddRawMaterialPurchaseToStock { get; }

}
