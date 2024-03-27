using HodladiWallet.Hubs;
using HodladiWallet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HodladiWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public PaymentController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentNotification payment)
    {
        await _hubContext.Clients.All.SendAsync("ReceivePayment", payment);
        return Ok();
    }
}