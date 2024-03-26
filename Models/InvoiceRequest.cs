using System.ComponentModel.DataAnnotations;

namespace HodladiWallet.Models;

public class InvoiceRequest
{
	[Required(ErrorMessage = "Invoice is required.")]
	public string Invoice { get; set; }
	public string Description { get; set; }
}
