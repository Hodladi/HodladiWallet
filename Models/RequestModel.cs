using System.ComponentModel.DataAnnotations;

namespace HodladiWallet.Models;

public class RequestModel(string description)
{
	public string Description { get; set; } = description;

	[Required(ErrorMessage = "Amount in satoshis is required.")]
	public int AmountSat { get; set; }
	public string ExternalId { get; set; }
}
