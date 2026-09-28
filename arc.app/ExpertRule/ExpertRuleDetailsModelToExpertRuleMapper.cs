using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.domain.Coding;
using System.Linq;

namespace arc.app.ExpertRule;

/// <summary>
/// Maps the ExpertRuleDetailsModel type onto the ExpertRule type.
/// </summary>
public class ExpertRuleDetailsModelToExpertRuleMapper : IMapType<ExpertRuleDetailsModel, arc.domain.Coding.ExpertRule>
{
    /// <summary>
    /// Maps a <see cref="ExpertRuleDetailsModel"/> object to a <see cref="arc.domain.Coding.ExpertRule"/> object.
    /// </summary>
    /// <param name="source">The <see cref="ExpertRuleDetailsModel"/> object to map.</param>
    /// <returns>The mapped <see cref="arc.domain.Coding.ExpertRule"/> object.</returns>
    public arc.domain.Coding.ExpertRule Map(ExpertRuleDetailsModel source)
    {
        var result = source.Map<arc.domain.Coding.ExpertRule>();

        // Transform RuleTestConditionGrid from crafted TestGrid format if needed
        if (source.RuleTestConditionGrid != null && source.RuleTestConditionGrid.Any())
        {
            result.RuleTestConditionGrid = source.RuleTestConditionGrid.Select(t => TransformTestCondition(t)).ToList();
        }

        return result;
    }

    /// <summary>
    /// Transforms a RuleTestConditionGridModel from crafted TestGrid format to the required format.
    /// If the item has Test/Field properties (crafted format), transforms them to TestName/FieldName/CompValue.
    /// If already in correct format, preserves the existing values.
    /// </summary>
    /// <param name="source">The source RuleTestConditionGridModel to transform.</param>
    /// <returns>The transformed RuleTestConditionGridModel.</returns>
    private static RuleTestConditionGridModel TransformTestCondition(RuleTestConditionGridModel source)
    {
        var result = new RuleTestConditionGridModel
        {
            TestConditionId = source.TestConditionId,
            ExpertRuleId = source.ExpertRuleId,

            Comparison = source.Comparison
        };

        // Check if data is in crafted format (has Test or Field properties)
        // These are the key indicators that data came from the crafted TestGrid component
        bool isCraftedFormat = !string.IsNullOrWhiteSpace(source.Test) || !string.IsNullOrWhiteSpace(source.Field);

        if (isCraftedFormat)
        {
            // Transform from crafted format
            // Test and Field may contain IDs or display names - use them as-is for now
            result.TestName = source.Test ?? source.TestName;
            result.FieldName = source.Field ?? source.FieldName;

            // CompValue should be the first non-null value from: ListValue, NumberValue, StringValue
            // Or fall back to existing CompValue if none are set
            result.CompValue = source.ListValue ?? source.NumberValue ?? source.StringValue ?? source.CompValue;
        }
        else
        {
            // Already in correct format (from database), preserve existing values
            result.TestName = source.TestName;
            result.FieldName = source.FieldName;
            result.CompValue = source.CompValue;
        }

        return result;
    }
}
