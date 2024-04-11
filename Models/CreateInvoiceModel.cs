namespace HodladiWallet.Models;

public class CreateInvoiceModel
{
	public int AmountSat { get; set; }
	public string? Serialized { get; set; }
	public string? QrCodeBase64 { get; set; }
}
