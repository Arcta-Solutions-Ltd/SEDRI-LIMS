using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Definition for the expert rule view query. Supplies the query configuration used when loading
/// a single expert rule for view (e.g. on an expert rule record/detail screen).
/// </summary>
/// <remarks>
/// Returns a special query config that uses ExpertRuleViewQuery for execution and
/// expertruleviewmapper for result mapping. Translate is enabled for localized display.
/// </remarks>
internal class ExpertRuleViewQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule view query.
    /// </summary>
    /// <returns>Query definition with Query name, TableName, Type, ResultMapping, and Translate flag.</returns>
    public string Get()
    {
        return @"{
                    'Query': 'ExpertRuleViewQuery',
                    'TableName': 'ExpertRule',
                    'Type': 'Special',
                    'ResultMapping': 'expertruleviewmapper',
                    'Translate': true
                }";
    }
}
