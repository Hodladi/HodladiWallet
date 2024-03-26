using System.ComponentModel.DataAnnotations;

namespace HodladiWallet.Models;

public class PaymentRequest
{
	public string Description { get; set; }
	[Required(ErrorMessage = "Amount in satoshis is required.")]
	public int AmountSat { get; set; }
	public string? ExternalId { get; set; }
}
