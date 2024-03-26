namespace HodladiWallet.Models;

public class PaymentResponse
{
	public int RecipientAmountSat { get; set; }
	public int RoutingFeeSat { get; set; }
	public string PaymentId { get; set; }
	public string PaymentHash { get; set; }
	public string PaymentPreimage { get; set; }
}
