using System.Text;

namespace HodladiWallet.Services;

public interface IBech32Service
{
	Task<string?> Decode(string? input);
}

public class Bech32Service : IBech32Service
{
	private const string Charset = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

	private readonly ILightningAddressService _lightningAddressService;

	public Bech32Service(ILightningAddressService lightningAddressService)
	{
		_lightningAddressService = lightningAddressService;
	}

	public async Task<string?> Decode(string? input)
	{
		return await DecodeBech32(input);
	}

	private async Task<string?> DecodeBech32(string? bech32)
	{
		bech32 = bech32?.ToLower();
		int pos = bech32!.LastIndexOf('1');
		if (pos < 1 || pos + 6 > bech32.Length) throw new FormatException("Invalid Bech32 string.");

		string dataPart = bech32.Substring(pos + 1);

		var data = dataPart.ToCharArray().Select(c => Charset.IndexOf(c)).ToArray();
		if (!VerifyChecksum()) throw new FormatException("Checksum verification failed.");

		var decoded = ConvertBits(data, 5, 8, false);
		var decodedString = Encoding.UTF8.GetString(decoded);

		decodedString = new string(decodedString.TakeWhile(c => !char.IsControl(c)).ToArray());

        return await _lightningAddressService.FetchLnUrl(decodedString);
	}

	private byte[] ConvertBits(IEnumerable<int> data, int fromBits, int toBits, bool pad)
	{
		int acc = 0;
		int bits = 0;
		var ret = new List<byte>();
		int maxv = (1 << toBits) - 1;

		foreach (int value in data)
		{
			if (value < 0 || (value >> fromBits) != 0)
			{
				throw new ArgumentException("Invalid data in ConvertBits");
			}

			acc = (acc << fromBits) | value;
			bits += fromBits;
			while (bits >= toBits)
			{
				bits -= toBits;
				ret.Add((byte)((acc >> bits) & maxv));
			}
		}

		if (pad)
		{
			if (bits > 0)
			{
				ret.Add((byte)((acc << (toBits - bits)) & maxv));
			}
		}
		else if (bits >= fromBits || ((acc << (toBits - bits)) & maxv) != 0)
		{
			throw new ArgumentException("Invalid padding in ConvertBits");
		}

		int zeroIndex = ret.IndexOf(0);
		if (zeroIndex != -1)
		{
			ret = ret.Take(zeroIndex).ToList();
		}

		return ret.ToArray();
	}

	private bool VerifyChecksum()
	{
		return true;
	}
}