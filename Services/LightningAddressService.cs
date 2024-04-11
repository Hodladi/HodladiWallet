using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HodladiWallet.Services;

public interface ILightningAddressService
{
	Task<string?> FetchLnUrlFromLightningAddress(string? lightningAddress);
	Task<string?> FetchLnUrl(string decodedLnUrl);
	Task<string?> AddAmountAndCommentToLnUrl(string? baseUrl, int amount, string? comment = null);
}

public class LightningAddressService : ILightningAddressService
{
	private readonly HttpClient _httpClient;
	private static readonly Regex InvoiceRegex = new Regex(@"(lnbc[^\""]+)", RegexOptions.Compiled);

	public LightningAddressService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<string?> FetchLnUrlFromLightningAddress(string? lightningAddress)
	{
		if (lightningAddress != null && lightningAddress.StartsWith("lightning:", StringComparison.OrdinalIgnoreCase))
		{
			lightningAddress = lightningAddress.Substring("lightning:".Length);
		}

		string[] parts = lightningAddress!.Split('@');
		if (parts.Length != 2)
		{
			throw new FormatException($"Invalid Lightning Address: {lightningAddress}");
		}

		string url = $"https://{parts[1]}/.well-known/lnurlp/{parts[0]}";

		try
		{
			string response = await _httpClient.GetStringAsync(url);
			var payRequest = JsonConvert.DeserializeObject<CallbackData>(response);
			return payRequest?.Callback;
		}
		catch (HttpRequestException httpEx)
		{
			throw new InvalidOperationException($"Failed to fetch LNURL from Lightning Address: {httpEx.Message}", httpEx);
		}
		catch (JsonException jsonEx)
		{
			throw new InvalidOperationException("Error parsing LNURL response.", jsonEx);
		}
	}


	public async Task<string?> FetchLnUrl(string decodedLnUrl)
	{
        string response = await _httpClient.GetStringAsync(decodedLnUrl);
        var payRequest = JsonConvert.DeserializeObject<CallbackData>(response);
        return payRequest?.Callback;
    }

	public async Task<string?> AddAmountAndCommentToLnUrl(string? baseUrl, int amount, string? comment = null)
	{
        string fullUrl = $"{baseUrl}?amount={amount}" + (comment != null ? $"&comment={Uri.EscapeDataString(comment)}" : string.Empty);

        try
        { 
            HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            string lnAddressUrl = await response.Content.ReadAsStringAsync();
            Match match = InvoiceRegex.Match(lnAddressUrl);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract invoice from LNURL response.");
            }
        }
        catch (HttpRequestException httpEx)
        {
            throw new InvalidOperationException($"Error requesting invoice: {httpEx.Message}", httpEx);
        }
    }

	private class CallbackData
	{
        [JsonProperty("callback")]
        public string? Callback { get; set; } = string.Empty;
    }
}
