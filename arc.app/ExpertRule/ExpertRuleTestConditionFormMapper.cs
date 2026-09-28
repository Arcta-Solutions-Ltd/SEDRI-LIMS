using arc.common.Models.Alert;
using arc.common.Models.Coding;
using arc.data.model.Coding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.ExpertRule;

/// <summary>
/// Maps between expert rule test condition database rows and record-view add/edit form models.
/// Uses stable ids (test form name, field id, list item id) — not translated display text.
/// </summary>
public static class ExpertRuleTestConditionFormMapper
{
    /// <summary>
    /// Builds the edit form model from a database row for <c>editexpertruletestconditionquery</c>.
    /// </summary>
    /// <param name="row">Test condition row from the database.</param>
    /// <returns>Form model with <see cref="ExpertRuleTestConditionEditFormModel.TestGrid"/> populated when criteria exist.</returns>
    public static ExpertRuleTestConditionEditFormModel ToEditFormModel(ExpertRuleTestConditionDataModel row)
    {
        if (row == null || row.Id <= 0)
        {
            return new ExpertRuleTestConditionEditFormModel();
        }

        var hasCriteria = !string.IsNullOrWhiteSpace(row.FieldName);

        return new ExpertRuleTestConditionEditFormModel
        {
            Id = row.Id,
            ExpertRuleId = row.ExpertRuleId,
            TestGrid = hasCriteria ? [ToTestGridLine(row)] : []
        };
    }

    /// <summary>
    /// Maps add/edit form payload to a database row. On add, <paramref name="payload"/>.<see cref="ExpertRuleTestConditionEditFormModel.Id"/> is the parent expert rule id.
    /// </summary>
    /// <param name="payload">Deserialized form save payload.</param>
    /// <param name="isAdd">True when inserting a new row; false when updating by test condition id.</param>
    /// <returns>Row model for insert or update commands.</returns>
    /// <exception cref="InvalidOperationException">When no test grid criteria line is present.</exception>
    public static ExpertRuleTestConditionDataModel ToDataModel(ExpertRuleTestConditionEditFormModel payload, bool isAdd)
    {
        var gridLine = GetFirstCriteriaLine(payload?.TestGrid);
        if (gridLine == null)
        {
            throw new InvalidOperationException("Expert rule test condition requires test, field, and comparison criteria.");
        }

        var result = new ExpertRuleTestConditionDataModel();

        if (isAdd)
        {
            result.ExpertRuleId = payload?.Id ?? 0;
        }
        else
        {
            result.Id = payload?.Id ?? 0;
        }

        result.TestName = gridLine.Test;
        result.FieldName = gridLine.Field;
        result.Comparison = gridLine.Comparison;
        result.CompValue = ResolveCompValue(gridLine);

        return result;
    }

    private static TestGridModel GetFirstCriteriaLine(IReadOnlyList<TestGridModel> testGrid)
    {
        if (testGrid == null || testGrid.Count == 0)
        {
            return null;
        }

        return testGrid.FirstOrDefault(line =>
            !string.IsNullOrWhiteSpace(line?.Field) &&
            !string.IsNullOrWhiteSpace(line?.Test));
    }

    private static TestGridModel ToTestGridLine(ExpertRuleTestConditionDataModel row)
    {
        var compValue = row.CompValue ?? string.Empty;
        return new TestGridModel
        {
            Test = row.TestName,
            Field = row.FieldName,
            Comparison = row.Comparison,
            ListValue = compValue,
            StringValue = compValue,
            NumberValue = compValue
        };
    }

    private static string ResolveCompValue(TestGridModel line)
    {
        if (!string.IsNullOrWhiteSpace(line.ListValue))
        {
            return line.ListValue;
        }

        if (!string.IsNullOrWhiteSpace(line.NumberValue))
        {
            return line.NumberValue;
        }

        return line.StringValue;
    }
}
