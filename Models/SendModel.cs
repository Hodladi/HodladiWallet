using System.ComponentModel.DataAnnotations;

namespace HodladiWallet.Models;

public class SendModel
{
	[Required(ErrorMessage = "Invoice is required.")]
	public string? Invoice { get; set; }
}
