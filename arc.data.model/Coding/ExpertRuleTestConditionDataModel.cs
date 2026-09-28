namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertruletestcondition table in the database.
/// Stores test-based conditions for expert rules (e.g. field comparisons).
/// </summary>
public class ExpertRuleTestConditionDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the expertrule table.
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets the test name to evaluate.
    /// </summary>
    public string? TestName { get; set; }

    /// <summary>
    /// Gets or sets the field name within the test to compare.
    /// </summary>
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the comparison operator (e.g. =, &lt;, &gt;).
    /// </summary>
    public string? Comparison { get; set; }

    /// <summary>
    /// Gets or sets the value to compare against.
    /// </summary>
    public string? CompValue { get; set; }
}
