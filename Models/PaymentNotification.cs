namespace HodladiWallet.Models;

public class PaymentNotification
{
    public int AmountSat { get; set; }
    public string PaymentHash { get; set; }
    public string ExternalId { get; set; }
}
