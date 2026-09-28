using System.Collections.Generic;

namespace arc.common.Models.Laboratory;

/// <summary>
/// Represents the configuration model for culture tests,
/// encapsulating the culture type and associated values.
/// </summary>
public class CultureTestConfigModel
{
    /// <summary>
    /// Gets or sets the identifier for the culture type.
    /// </summary>
    public int CultureType { get; set; }

    /// <summary>
    /// Gets or sets the list of values associated with the culture test configuration.
    /// </summary>
    public List<string> Values { get; set; }
}
