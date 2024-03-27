using Microsoft.AspNetCore.SignalR;

namespace HodladiWallet.Hubs;

public class NotificationHub : Hub
{
    public async Task SendPaymentNotification(string message)
    {
        await Clients.All.SendAsync("ReceivePayment", message);
    }
}
