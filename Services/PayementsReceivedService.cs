using HodladiWallet.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Collections;

namespace HodladiWallet.Services;

public interface IPaymentsReceivedService
{
    Task<List<PaymentInfo>> GetPaymentsReceivedAsync();
}

public class PaymentsReceivedService : IPaymentsReceivedService
{
    private readonly HttpClient _httpClient;
    private readonly PhoenixApiConfig _phoenixApiConfig;

    public PaymentsReceivedService(HttpClient httpClient, PhoenixApiConfig phoenixApiConfig)
    {
        _httpClient = httpClient;
        _phoenixApiConfig = phoenixApiConfig;
    }

    public async Task<List<PaymentInfo>> GetPaymentsReceivedAsync()
    {
        var externalIds = new[] { "hodlwalletId_invoice", "hodlwalletId_lnurl" };
        var paymentsList = new List<PaymentInfo>();

        foreach (var externalId in externalIds)
        {
            var requestUri = $"{_phoenixApiConfig.BaseUrl}/payments/incoming?externalId={externalId}";
            var password = _phoenixApiConfig.Password;
            var byteArray = Encoding.ASCII.GetBytes($":{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var payments = JsonConvert.DeserializeObject<List<PaymentInfo>>(jsonResponse);
                var paidPayments = payments.Where(p => p.IsPaid).ToList();
                paymentsList.AddRange(paidPayments);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Request was canceled. Check timeout settings and network connectivity.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        paymentsList.Sort((x, y) => y.CreatedAt.CompareTo(x.CreatedAt));
        return paymentsList;
    }
}