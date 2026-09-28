namespace arc.common.Options;
public class AuthenticationOptions
{
    public LocalAuthenticationOptions Local { get; set; }
    public AzureAdAuthenticationOptions AzureAd { get; set; }
}
