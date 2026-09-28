using arc.app.Common;
using arc.common.Models.Coding;
using arc.common.Utils;
using System.Collections.Generic;

namespace arc.app.Coding;

/// <summary>
/// Validates the breakpoint configuration by deserializing a JSON message into
/// a <see cref="BreakpointValidatorModel"/> and enforcing rules on its content.
/// </summary>
internal class BreakpointValidator : ISpecialValidator
{
    private readonly string _message;

    /// <summary>
    /// Initializes a new instance of <see cref="BreakpointValidator"/> with
    /// the raw JSON payload to validate.
    /// </summary>
    /// <param name="message">
    /// A JSON string representing a <see cref="BreakpointValidatorModel"/>.
    /// </param>
    public BreakpointValidator(string message)
    {
        _message = message;
    }

    /// <summary>
    /// Performs validation in three phases:
    ///   1. Ensures <see cref="BreakpointValidatorModel.BreakpointGrid"/> is neither null nor empty.
    ///   2. Ensures <see cref="BreakpointValidatorModel.SpecificationId"/> is non-zero.
    ///   3. Ensures no two <see cref="BreakpointLineModel"/> entries overlap in their StartVal/EndVal ranges.
    /// </summary>
    /// <returns>
    /// An error code if a rule is violated:
    ///   "@BreNod@" when the grid is missing or empty,
    ///   "@BreSouA@" when SpecificationId is zero,
    ///   "@BreOve@" when any breakpoint lines overlap;
    ///   otherwise an empty string.
    /// </returns>
    public string ValidateMessage()
    {
        var breakpoint = ArcJson.Deserialize<BreakpointValidatorModel>(_message);

        if (breakpoint.BreakpointGrid == null || breakpoint.BreakpointGrid.Count == 0)
        {
            return "@BreNod@";
        }

        if (breakpoint.SpecificationId == 0)
        {
            return "@BreSouA@";
        }

        var runningList = new List<BreakpointLineModel>();
        foreach (var breakpointLine in breakpoint.BreakpointGrid)
        {
            foreach (var existing in runningList)
            {
                // Check if the current line's start or end falls within any previously processed line
                if ((breakpointLine.StartVal >= existing.StartVal && breakpointLine.StartVal <= existing.EndVal) ||
                    (breakpointLine.EndVal >= existing.StartVal && breakpointLine.EndVal <= existing.EndVal))
                {
                    return "@BreOve@";
                }
            }

            runningList.Add(breakpointLine);
        }

        return "";
    }
}
