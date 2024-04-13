namespace HodladiWallet.Models;

public class PaymentInfo
{
    public long CreatedAt { get; set; }
    public int ReceivedSat { get; set; }
    public bool IsPaid { get; set; }
}
