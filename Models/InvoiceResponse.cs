namespace HodladiWallet.Models;

public class InvoiceResponse
{
	public int AmountSat { get; set; }
	public string PaymentHash { get; set; }
	public string Serialized { get; set; }
	public string QrCodeBase64 { get; set; }
}
