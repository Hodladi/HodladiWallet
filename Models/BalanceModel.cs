using Newtonsoft.Json;

namespace HodladiWallet.Models;

public class BalanceModel
{
	[JsonProperty("balanceSat")]
	public long BalanceSat { get; set; }

	[JsonProperty("feeCreditSat")]
	public int FeeCreditSat { get; set; }
}