namespace arc.common.Models.Laboratory;
public class LaboratoryListGroupingModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the direct test.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the test category associated with the direct test.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the direct test configuration.
    /// </summary>
    public string AssociatedListId { get; set; }

    /// <summary>
    /// Gets or sets the name of the test category associated with the direct test.
    /// </summary>
    public string GroupDescription { get; set; }
}
