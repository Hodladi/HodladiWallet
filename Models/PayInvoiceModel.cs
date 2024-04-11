namespace HodladiWallet.Models;

public class PayInvoiceModel
{
	public PayInvoiceModel(int recipientAmountSat, int routingFeeSat, string paymentId, string paymentHash, string paymentPreimage)
	{
		RecipientAmountSat = recipientAmountSat;
		RoutingFeeSat = routingFeeSat;
		PaymentId = paymentId;
		PaymentHash = paymentHash;
		PaymentPreimage = paymentPreimage;
	}

	public int RecipientAmountSat { get; set; }
	public int RoutingFeeSat { get; set; }
	public string PaymentId { get; set; }
	public string PaymentHash { get; set; }
	public string PaymentPreimage { get; set; }
}
