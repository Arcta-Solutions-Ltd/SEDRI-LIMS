namespace arc.common.Options;

public class AzureAdAuthenticationOptions : AuthenticationOptionsBase
{
    public string Instance { get; set; }
    public string Domain { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string Audience { get; set; }
}
