using HodladiWallet.Models;
using HodladiWallet.Components;
using HodladiWallet.Services;

namespace HodladiWallet;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var phoenixApiConfig = new PhoenixApiConfig();
		builder.Configuration.GetSection("PhoenixApi").Bind(phoenixApiConfig);

		builder.Services.AddSingleton(phoenixApiConfig);
		builder.Services.AddScoped<ILightningInvoiceDecoder, LightningInvoiceDecoder>();

		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddHubOptions(options =>
			{
				options.ClientTimeoutInterval = TimeSpan.FromMinutes(15);
				options.KeepAliveInterval = TimeSpan.FromSeconds(5);
			});

		builder.Services.AddHttpClient<IInvoiceService, InvoiceService>((serviceProvider, client) =>
		{
			var config = serviceProvider.GetRequiredService<PhoenixApiConfig>();
			client.BaseAddress = new Uri(config.BaseUrl);
		});

		var port = builder.Configuration.GetValue<int>("ApplicationSettings:Port");

		builder.WebHost.ConfigureKestrel(serverOptions =>
		{
			serverOptions.ListenAnyIP(port);
		});

		var app = builder.Build();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}
        
		app.UseStaticFiles();
		app.UseAntiforgery();
		app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

		app.Run();
	}
}
