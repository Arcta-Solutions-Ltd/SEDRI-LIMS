using arc.common.Models;
using arc.common.Models.Coding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Resolves expert rule test condition list rows from stored ids to display labels for the record view.
/// </summary>
public interface IExpertRuleTestConditionListDisplayEnricher
{
    /// <summary>
    /// Enriches test condition list rows with translated test form titles, field labels, and list-type comp value display text.
    /// </summary>
    /// <param name="rows">Raw list rows with id-based TestName, FieldName, and CompValue values.</param>
    /// <param name="token">Token used for field label translation.</param>
    /// <returns>Rows with display text in TestName, FieldName, and CompValue (when list-backed) properties.</returns>
    Task<List<ExpertRuleTestConditionListModel>> EnrichAsync(
        IReadOnlyList<ExpertRuleTestConditionListModel> rows,
        TokenInfoModel token);
}
