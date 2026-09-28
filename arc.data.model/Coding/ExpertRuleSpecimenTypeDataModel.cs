namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertrulespecimentype table in the database.
/// Stores specimen type include/exclude filters for expert rules.
/// </summary>
public class ExpertRuleSpecimenTypeDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the expertrule table.
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table for specimen type.
    /// </summary>
    public int SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets whether the specimen type is included (true) or excluded (false).
    /// </summary>
    public bool Included { get; set; }
}
