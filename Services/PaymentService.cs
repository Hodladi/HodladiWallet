using Net.Codecrete.QrCodeGenerator;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using HodladiWallet.Models;

namespace HodladiWallet.Services;

public interface IPaymentService
{
	Task<CreateInvoiceModel?> CreateInvoiceAsync(RequestModel createInvoiceModel);
	Task<PayInvoiceModel?> PayIncomingInvoice(SendModel incomingInvoiceModel);
	Task<BalanceModel?> GetBalanceAsync();
	Task<string?> GenerateQrCodeBase64(string? data);
}
public class PaymentService : IPaymentService
{
	private readonly HttpClient _httpClient;
	private readonly PhoenixApiConfig _phoenixApiConfig;

	public PaymentService(HttpClient httpClient, PhoenixApiConfig phoenixApiConfig)
	{
		_httpClient = httpClient;
		_phoenixApiConfig = phoenixApiConfig;
	}

	public async Task<CreateInvoiceModel?> CreateInvoiceAsync(RequestModel createInvoiceModel)
	{
		var requestUri = $"{_phoenixApiConfig.BaseUrl}/createinvoice";
		var requestContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("description", createInvoiceModel.Description),
			new KeyValuePair<string, string>("amountSat", createInvoiceModel.AmountSat.ToString()),
		});

		var password = _phoenixApiConfig.Password;
		var byteArray = Encoding.ASCII.GetBytes($":{password}");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		var response = await _httpClient.PostAsync(requestUri, requestContent);
		response.EnsureSuccessStatusCode();
		var jsonResponse = await response.Content.ReadAsStringAsync();
		var invoiceResponse = JsonConvert.DeserializeObject<CreateInvoiceModel>(jsonResponse);

		if (invoiceResponse != null)
		{
			invoiceResponse.QrCodeBase64 = await GenerateQrCodeBase64(invoiceResponse.Serialized);
		}

		return invoiceResponse;
	}

	public Task<string?> GenerateQrCodeBase64(string? data)
	{
		var qr = QrCode.EncodeText(data, QrCode.Ecc.Low);
		byte[] svgBytes = Encoding.UTF8.GetBytes(qr.ToSvgString(1));
		return Task.FromResult(Convert.ToBase64String(svgBytes));
	}

	public async Task<PayInvoiceModel?> PayIncomingInvoice(SendModel incomingInvoiceModel)
	{
		var requestUri = $"{_phoenixApiConfig.BaseUrl}/payinvoice";
		var requestContent = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string?>("invoice", incomingInvoiceModel.Invoice)
		});

		var password = _phoenixApiConfig.Password;
		var byteArray = Encoding.ASCII.GetBytes($":{password}");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		var response = await _httpClient.PostAsync(requestUri, requestContent);
		response.EnsureSuccessStatusCode();

		var jsonResponse = await response.Content.ReadAsStringAsync();
		var paymentResponse = JsonConvert.DeserializeObject<PayInvoiceModel>(jsonResponse);

		return paymentResponse;
	}

	public async Task<BalanceModel?> GetBalanceAsync()
	{
		string url = $"{_phoenixApiConfig.BaseUrl}/getbalance";

		using (var httpClient = new HttpClient())
		{
			var password = _phoenixApiConfig.Password;
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{password}")));

			var response = await httpClient.GetAsync(url);

			response.EnsureSuccessStatusCode();

			string jsonResponse = await response.Content.ReadAsStringAsync();
			var balanceResponse = JsonConvert.DeserializeObject<BalanceModel>(jsonResponse);

			return balanceResponse;
		}
	}

}
