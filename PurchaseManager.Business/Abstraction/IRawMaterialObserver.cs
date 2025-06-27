using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseManager.Business.Abstraction;

    public interface IRawMaterialObserver
    {
	IObserver<int> AddRawMaterial { get; }

}
