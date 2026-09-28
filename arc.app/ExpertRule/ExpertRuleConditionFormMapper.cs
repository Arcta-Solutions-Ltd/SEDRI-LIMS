using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.data.model.Coding;
using Newtonsoft.Json.Linq;

namespace arc.app.ExpertRule;

/// <summary>
/// Maps between expert rule condition database rows and record-view add/edit form models.
/// </summary>
public static class ExpertRuleConditionFormMapper
{
    /// <summary>
    /// Builds the edit form model from a database row for <c>editexpertruleconditionquery</c>.
    /// </summary>
    /// <param name="row">Condition row from the database.</param>
    /// <returns>Form model with nullable measurement range values.</returns>
    public static ExpertRuleConditionEditFormModel ToEditFormModel(ExpertRuleConditionDataModel row)
    {
        if (row == null || row.Id <= 0)
        {
            return new ExpertRuleConditionEditFormModel();
        }

        return new ExpertRuleConditionEditFormModel
        {
            Id = row.Id,
            AntibioticId = row.AntibioticId,
            AntibioticGroupId = row.AntibioticGroupId,
            SusceptibilityId = row.SusceptibilityId,
            TestMethodId = row.TestMethodId,
            SpecialConsiderationId = row.SpecialConsiderationId,
            StartVal = row.StartVal,
            EndVal = row.EndVal
        };
    }

    /// <summary>
    /// Maps add/edit form payload to a database row. On add, <paramref name="payload"/>.<see cref="ExpertRuleConditionEditFormModel.Id"/> is the parent expert rule id.
    /// </summary>
    /// <param name="payload">Deserialized form save payload.</param>
    /// <param name="isAdd">True when inserting a new row; false when updating by condition id.</param>
    /// <returns>Row model for insert or update commands.</returns>
    public static ExpertRuleConditionDataModel ToDataModel(ExpertRuleConditionEditFormModel payload, bool isAdd)
    {
        var result = new ExpertRuleConditionDataModel
        {
            AntibioticId = NullIfZero(payload?.AntibioticId),
            AntibioticGroupId = NullIfZero(payload?.AntibioticGroupId),
            SusceptibilityId = NullIfZero(payload?.SusceptibilityId),
            TestMethodId = NullIfZero(payload?.TestMethodId),
            SpecialConsiderationId = NormalizeSpecialConsideration(payload?.SpecialConsiderationId),
            StartVal = payload?.StartVal,
            EndVal = payload?.EndVal
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
    /// Maps raw JSON form payload to a database row, parsing optional measurement values from strings or numbers.
    /// </summary>
    /// <param name="dataToSave">JSON form payload from the portal.</param>
    /// <param name="isAdd">True when inserting a new row.</param>
    /// <returns>Row model for insert or update commands.</returns>
    public static ExpertRuleConditionDataModel ToDataModelFromJson(string dataToSave, bool isAdd)
    {
        var token = JObject.Parse(dataToSave ?? "{}");
        var payload = new ExpertRuleConditionEditFormModel
        {
            Id = token.GetNullableInt32("Id") ?? token.GetNullableInt32("id") ?? 0,
            AntibioticId = token.GetNullableInt32("antibioticid"),
            AntibioticGroupId = token.GetNullableInt32("antibioticgroupid"),
            SusceptibilityId = token.GetNullableInt32("SusceptibilityId") ?? token.GetNullableInt32("susceptibilityid"),
            TestMethodId = token.GetNullableInt32("testmethodid"),
            SpecialConsiderationId = token.GetNullableInt32("SpecialConsiderationId") ?? token.GetNullableInt32("specialconsiderationid"),
            StartVal = ParseTokenDecimal(token["startval"]),
            EndVal = ParseTokenDecimal(token["endval"])
        };

        return ToDataModel(payload, isAdd);
    }

    private static decimal? ParseTokenDecimal(JToken token)
    {
        if (token == null || token.Type == JTokenType.Null || token.Type == JTokenType.Undefined)
        {
            return null;
        }

        if (token.Type == JTokenType.String)
        {
            return NullableDecimalExtensions.ParseOptionalMeasurementValue(token.Value<string>());
        }

        if (token.Type == JTokenType.Integer || token.Type == JTokenType.Float)
        {
            return token.ToObject<decimal?>();
        }

        return NullableDecimalExtensions.ParseOptionalMeasurementValue(token.Value<object>());
    }

    private static int? NullIfZero(int? value) => value is > 0 ? value : null;

    private static int? NormalizeSpecialConsideration(int? value)
    {
        if (!value.HasValue || value.Value == 0)
        {
            return 973;
        }

        return value;
    }
}
