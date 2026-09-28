namespace arc.common.Options;

public class LocalAuthenticationOptions : AuthenticationOptionsBase
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
