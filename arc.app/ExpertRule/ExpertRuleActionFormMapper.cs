using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.data.model.Coding;
using Newtonsoft.Json.Linq;

namespace arc.app.ExpertRule;

/// <summary>
/// Maps between expert rule action database rows and record-view add/edit form models.
/// </summary>
public static class ExpertRuleActionFormMapper
{
    /// <summary>
    /// Builds the edit form model from a database row for <c>editexpertruleactionquery</c>.
    /// </summary>
    /// <param name="row">Action row from the database.</param>
    /// <returns>Form model with nullable target ids.</returns>
    public static ExpertRuleActionEditFormModel ToEditFormModel(ExpertRuleActionDataModel row)
    {
        if (row == null || row.Id <= 0)
        {
            return new ExpertRuleActionEditFormModel();
        }

        var normalized = ExpertRuleActionTargetNormalizer.Normalize(row.AntibioticId, row.AntibioticGroupId);

        return new ExpertRuleActionEditFormModel
        {
            Id = row.Id,
            AntibioticId = normalized.AntibioticId,
            AntibioticGroupId = normalized.AntibioticGroupId,
            SusceptibilityId = row.SusceptibilityId,
            DisplayOnReport = row.DisplayOnReport
        };
    }

    /// <summary>
    /// Maps add/edit form payload to a database row. On add, <paramref name="payload"/>.<see cref="ExpertRuleActionEditFormModel.Id"/> is the parent expert rule id.
    /// </summary>
    /// <param name="payload">Deserialized form save payload.</param>
    /// <param name="isAdd">True when inserting a new row; false when updating by action id.</param>
    /// <returns>Row model for insert or update commands.</returns>
    public static ExpertRuleActionDataModel ToDataModel(ExpertRuleActionEditFormModel payload, bool isAdd)
    {
        var normalized = ExpertRuleActionTargetNormalizer.Normalize(payload?.AntibioticId, payload?.AntibioticGroupId);

        var result = new ExpertRuleActionDataModel
        {
            AntibioticId = normalized.AntibioticId,
            AntibioticGroupId = normalized.AntibioticGroupId,
            SusceptibilityId = NullIfZero(payload?.SusceptibilityId),
            DisplayOnReport = NormalizeDisplayOnReport(payload?.DisplayOnReport)
        };

        if (isAdd)
        {
            result.ExpertRuleId = payload?.Id ?? 0;
        }
        else
        {
            result.Id = payload?.Id ?? 0;
        }

        return result;
    }

    /// <summary>
    /// Maps raw JSON form payload to a database row.
    /// </summary>
    /// <param name="dataToSave">JSON form payload from the portal.</param>
    /// <param name="isAdd">True when inserting a new row.</param>
    /// <returns>Row model for insert or update commands.</returns>
    public static ExpertRuleActionDataModel ToDataModelFromJson(string dataToSave, bool isAdd)
    {
        var token = JObject.Parse(dataToSave ?? "{}");
        var payload = new ExpertRuleActionEditFormModel
        {
            Id = token.GetNullableInt32("Id") ?? token.GetNullableInt32("id") ?? 0,
            AntibioticId = token.GetNullableInt32("antibioticid"),
            AntibioticGroupId = token.GetNullableInt32("antibioticgroupid"),
            SusceptibilityId = token.GetNullableInt32("SusceptibilityId") ?? token.GetNullableInt32("susceptibilityid"),
            DisplayOnReport = token["DisplayOnReport"]?.Type == JTokenType.Null
                ? null
                : token["DisplayOnReport"]?.ToString() ?? token["displayonreport"]?.ToString()
        };

        return ToDataModel(payload, isAdd);
    }

    private static int? NullIfZero(int? value) => value is > 0 ? value : null;

    private static string NormalizeDisplayOnReport(string value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}
