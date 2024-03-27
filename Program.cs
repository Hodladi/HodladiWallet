using HodladiWallet.Components;
using HodladiWallet.Models;
using HodladiWallet.Services;
using HodladiWallet.Hubs;

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

        builder.Services.AddControllers();
        builder.Services.AddSignalR();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient<IInvoiceService, InvoiceService>((serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<PhoenixApiConfig>();
            client.BaseAddress = new Uri(config.BaseUrl);
        });

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(5799);
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAntiforgery();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/notificationhub");
            endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        });

        app.Run();
    }
}