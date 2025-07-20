using PurchaseManager.Business.Abstraction;
using System.Reactive.Subjects;
namespace PurchaseManager.Business;

public class Subject : ISubject
{
	private Subject<int> AddRawMaterial { get; } = new Subject<int>();

	public IObserver<int> AddRawMaterialPurchaseToStock => AddRawMaterial;

	IObservable<int> IRawMaterialsObservable.AddRawMaterialPurchaseToStock => AddRawMaterial;
}
