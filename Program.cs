using HodladiWallet.Components;
using HodladiWallet.Models;
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
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient<IInvoiceService, InvoiceService>((serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<PhoenixApiConfig>();
            client.BaseAddress = new Uri(config.BaseUrl);
        });

        // Configure Kestrel to listen on port 5799 for HTTP
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(5799); // Listen for HTTP connections on port 5799
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
