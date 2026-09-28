namespace arc.common.Models.Laboratory;

/// <summary>
/// Laboratory configuration keyed on a form name rather than a list item. Used by the form specimen type
/// options, where the group is the request form the restriction applies to.
/// </summary>
public class LaboratoryFormGroupingModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the configuration record.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the form the configuration applies to.
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// Gets or sets the comma separated list item ids the form is restricted to.
    /// </summary>
    public string AssociatedListId { get; set; }

    /// <summary>
    /// Gets or sets the display title of the form, resolved from the form configuration.
    /// </summary>
    public string GroupDescription { get; set; }
}
