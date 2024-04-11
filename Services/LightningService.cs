using BTCPayServer.Lightning;
using HodladiWallet.Models;
using NBitcoin;

namespace HodladiWallet.Services;

public interface ILightningService
{
	Task<int> ParseInvoiceAmount(string? lightningInvoice);
    Task<string?> HandleInputAsync(string? input);
}

public class LightningService : ILightningService
{
    private readonly Network _network;
    private readonly IBech32Service _bech32Service;
    private readonly ILightningAddressService _lightningAddressService;

    public LightningService(Network network, IBech32Service bech32Service, ILightningAddressService lightningAddressService)
    {
	    _network = network;
	    _bech32Service = bech32Service;
	    _lightningAddressService = lightningAddressService;
    }

    public async Task<string?> HandleInputAsync(string? input)
    {
        Enums inputType = DetermineInputType(input);
        return inputType switch
        {
            Enums.LnInvoice => input,
            Enums.Lnurl => await ProcessLnurl(input),
            Enums.LightningAddress => await FetchInvoiceFromLightningAddress(input),
            _ => throw new ArgumentException("Unsupported input type.")
        };
    }

    private Enums DetermineInputType(string? input)
    {
        if (input != null && input.StartsWith("lnbc", StringComparison.OrdinalIgnoreCase)) return Enums.LnInvoice;
        else if (input != null && input.StartsWith("lnurl", StringComparison.OrdinalIgnoreCase)) return Enums.Lnurl;
        else if (input != null && input.Contains("@")) return Enums.LightningAddress;
        else throw new FormatException("Invalid input format.");
    }

    public Task<int> ParseInvoiceAmount(string? lightningInvoice)
    {
        if (string.IsNullOrWhiteSpace(lightningInvoice)) return Task.FromResult(0);
        try
        {
            var invoice = BOLT11PaymentRequest.Parse(lightningInvoice, _network);
            return Task.FromResult((int)(invoice.MinimumAmount.ToUnit(LightMoneyUnit.Satoshi)));
        }
        catch
        {
            return Task.FromResult(0);
        }
    }

    private async Task<string?> ProcessLnurl(string? lnurl) => await _bech32Service.Decode(lnurl);
    private async Task<string?> FetchInvoiceFromLightningAddress(string? lightningAddress) => await _lightningAddressService.FetchLnUrlFromLightningAddress(lightningAddress);
}

