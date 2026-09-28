using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;

namespace arc.common.Models.Security;

/// <summary>
/// Represents the model used during startup to check system configuration and provide UI text settings.
/// </summary>
public class StartUpCheckModel
{
    /// <summary>
    /// Gets or sets the text indicating that the system is being configured.
    /// </summary>
    public string ConfigureSystem { get; set; }

    /// <summary>
    /// Gets or sets the text label for the username input.
    /// </summary>
    public string UsernameText { get; set; }

    /// <summary>
    /// Gets or sets the text label for the password input.
    /// </summary>
    public string PasswordText { get; set; }

    /// <summary>
    /// Gets or sets the text displayed while establishing the connection or initializing the system.
    /// </summary>
    public string EstablishingText { get; set; }

    /// <summary>
    /// Gets or sets the text displayed during the authentication process.
    /// </summary>
    public string AuthenticatingText { get; set; }

    /// <summary>
    /// Gets or sets the text for the login action, typically a button label.
    /// </summary>
    public string LoginText { get; set; }

    /// <summary>
    /// Represents the version information of the application.
    /// </summary>
    public string VersionText { get; set; }

    /// <summary>
    /// Indicates whether local login authentication is enabled.
    /// </summary>
    public bool LocalLoginEnabled { get; set; }

    /// <summary>
    /// Indicates whether Azure Active Directory login authentication is enabled.
    /// </summary>
    public bool AzureAdLoginEnabled { get; set; }

    /// <summary>
    /// Lines of text to display on the login screen warning a user about the system.
    /// Used for the Evaluation system disclaimer.
    /// </summary>
    public IEnumerable<string> WarningTextLines { get; set; } = [];
}
