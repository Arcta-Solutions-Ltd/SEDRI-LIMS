using arc.common.Models.Alert;
using System.Collections.Generic;

namespace arc.common.Models.Coding;

/// <summary>
/// Form payload and initial-query result for add/edit expert rule test condition from the record view.
/// </summary>
public class ExpertRuleTestConditionEditFormModel
{
    /// <summary>
    /// Gets or sets the test condition row id (edit) or expert rule id (add).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the expert rule id (populated on edit load for diagnostics; add uses <see cref="Id"/> as parent id).
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets the optional display name when the row has no test grid criteria.
    /// </summary>
    public string TestConditionName { get; set; }

    /// <summary>
    /// Gets or sets the crafted test grid lines (test form id, field id, comparison, value ids).
    /// </summary>
    public List<TestGridModel> TestGrid { get; set; } = [];
}
