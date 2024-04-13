namespace HodladiWallet.Models;

public class Manifest
{
    public string? ShortName { get; set; }
    public string? Name { get; set; }
    public List<Icon> Icons { get; set; } = new List<Icon>();
    public string StartUrl { get; set; }
    public string BackgroundColor { get; set; }
    public string Display { get; set; }
    public string Scope { get; set; }
    public string ThemeColor { get; set; }
}
