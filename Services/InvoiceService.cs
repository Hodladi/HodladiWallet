using Net.Codecrete.QrCodeGenerator;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using HodladiWallet.Models;

namespace HodladiWallet.Services;

public interface IInvoiceService
{
	Task<InvoiceResponse?> CreateInvoiceAsync(PaymentRequest request);
	Task<PaymentResponse?> ProcessPaymentAsync(InvoiceRequest request);
	Task<BalanceResponse?> GetBalanceAsync();
}
public class InvoiceService : IInvoiceService
{
	private readonly HttpClient _httpClient;
	private readonly PhoenixApiConfig _phoenixApiConfig;

	public InvoiceService(HttpClient httpClient, PhoenixApiConfig phoenixApiConfig)
	{
		_httpClient = httpClient;
		_phoenixApiConfig = phoenixApiConfig;
	}

	public async Task<InvoiceResponse?> CreateInvoiceAsync(PaymentRequest request)
	{
		var requestUri = $"{_phoenixApiConfig.BaseUrl}/createinvoice";
		var requestContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("description", request.Description),
			new KeyValuePair<string, string>("amountSat", request.AmountSat.ToString()),
		});

		var password = _phoenixApiConfig.Password;
		var byteArray = Encoding.ASCII.GetBytes($":{password}");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		var response = await _httpClient.PostAsync(requestUri, requestContent);
		response.EnsureSuccessStatusCode();
		var jsonResponse = await response.Content.ReadAsStringAsync();
		var invoiceResponse = JsonConvert.DeserializeObject<InvoiceResponse>(jsonResponse);

		if (invoiceResponse != null)
		{
			invoiceResponse.QrCodeBase64 = GenerateQrCodeBase64(invoiceResponse.Serialized);
		}

		return invoiceResponse;
	}

	private string GenerateQrCodeBase64(string data)
	{
		var qr = QrCode.EncodeText(data, QrCode.Ecc.Low);
		byte[] svgBytes = Encoding.UTF8.GetBytes(qr.ToSvgString(1));
		return Convert.ToBase64String(svgBytes);
	}

	public async Task<PaymentResponse?> ProcessPaymentAsync(InvoiceRequest request)
	{
		var requestUri = $"{_phoenixApiConfig.BaseUrl}/payinvoice";
		var requestContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string?>("invoice", request.Invoice)
		});

		var password = _phoenixApiConfig.Password;
		var byteArray = Encoding.ASCII.GetBytes($":{password}");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		var response = await _httpClient.PostAsync(requestUri, requestContent);
		response.EnsureSuccessStatusCode();

		var jsonResponse = await response.Content.ReadAsStringAsync();
		var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(jsonResponse);

		return paymentResponse;
	}

	public async Task<BalanceResponse?> GetBalanceAsync()
	{
		string url = $"{_phoenixApiConfig.BaseUrl}/getbalance";

		using (var httpClient = new HttpClient())
		{
			var password = _phoenixApiConfig.Password;
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{password}")));

			var response = await httpClient.GetAsync(url);

			response.EnsureSuccessStatusCode();

			string jsonResponse = await response.Content.ReadAsStringAsync();
			var balanceResponse = JsonConvert.DeserializeObject<BalanceResponse>(jsonResponse);

			return balanceResponse;
		}
	}

}
