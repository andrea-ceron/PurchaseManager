using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseManager.ClientHttp.Abstraction;

namespace PurchaseManager.ClientHttp.DI;

public static class PurchaseManagerClientExtension
{
	public static IServiceCollection AddPurchaseManagerClientHttp(this IServiceCollection services, IConfiguration config)
	{
		IConfigurationSection section = config.GetSection(PurchaseManagerClientOption.SectionName);
		PurchaseManagerClientOption options = section.Get<PurchaseManagerClientOption>() ?? new();

		services.AddHttpClient<IPurchaseManagerClientHttp, PurchaseManagerClientHttp>(o => {
			o.BaseAddress = new Uri(options.BaseAddress);
		}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
		});
		return services;
	}
}
