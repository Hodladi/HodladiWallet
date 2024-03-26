using BTCPayServer.Lightning;
using NBitcoin; // Required for accessing the Network class

namespace HodladiWallet.Services;

public interface ILightningInvoiceDecoder
{
	long ParseInvoiceAmount(string lightningInvoice);
}

public class LightningInvoiceDecoder : ILightningInvoiceDecoder
{
	public long ParseInvoiceAmount(string lightningInvoice)
	{
		if (string.IsNullOrWhiteSpace(lightningInvoice))
		{
			return 0;
		}

		try
		{
			var invoice = BOLT11PaymentRequest.Parse(lightningInvoice, Network.Main);
			// Explicitly cast the result to long
			return (long)(invoice.MinimumAmount?.ToUnit(LightMoneyUnit.Satoshi) ?? 0m);
		}
		catch
		{
			// Parsing failed or the invoice does not specify an amount
			return 0;
		}
	}
}
