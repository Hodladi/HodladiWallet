using HodladiWallet.Components;
using HodladiWallet.Models;
using HodladiWallet.Services;
using NBitcoin;

namespace HodladiWallet;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var phoenixApiConfig = new PhoenixApiConfig();
        builder.Configuration.GetSection("PhoenixApi").Bind(phoenixApiConfig);
        builder.Services.AddSingleton(phoenixApiConfig);

        builder.Services.AddSingleton(Network.Main);

		builder.Services.AddScoped<IBech32Service, Bech32Service>();
		builder.Services.AddScoped<ILightningAddressService, LightningAddressService>();
        builder.Services.AddScoped<ILightningService, LightningService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<HttpClient>();

        builder.Services.AddRazorComponents()
	        .AddInteractiveServerComponents()
	        .AddHubOptions(options =>
	        {
		        options.ClientTimeoutInterval = TimeSpan.FromMinutes(15);
		        options.KeepAliveInterval = TimeSpan.FromSeconds(5);
	        });

        builder.Services.AddHttpClient<IPaymentService, PaymentService>((serviceProvider, client) =>
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