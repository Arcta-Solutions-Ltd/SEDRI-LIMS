using arc.data.model.Configuration;

namespace arc.common.Models.Laboratory;
public class LaboratoryConfigsModel : LaboratoryConfigsDataModel
{
    /// <summary>
    /// Represents the identifier for the laboratory group.
    /// Used to categorize or group related configurations.
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// Descriptive text associated with the laboratory group.
    /// Provides a human-readable representation of the group.
    /// </summary>
    public string GroupText { get; set; }

    /// <summary>
    /// Represents the identifier for an associated list.
    /// Used to reference related data or configurations.
    /// </summary>
    public string AssociatedListId { get; set; }

    /// <summary>
    /// Categories related to the laboratory configurations.
    /// Initialized with an empty string by default.
    /// </summary>
    public string Categories { get; set; } = "";
}

