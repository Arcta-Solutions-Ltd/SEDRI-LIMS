namespace arc.common.Models.Common;

/// <summary>
/// Represents a model that includes both an identifier and a display name.
/// Inherits from <see cref="IdModel"/>.
/// </summary>
public class IdAndNameModel : IdModel
{
    /// <summary>
    /// The display name associated with the identifier.
    /// </summary>
    public string Name { get; set; }
}
