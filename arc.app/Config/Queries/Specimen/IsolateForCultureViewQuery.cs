using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Provides a specialized query definition for retrieving isolate-level data
/// within the "CultureView" context.
/// </summary>
/// <remarks>
/// This query targets the <c>Culture</c> table and is marked as <c>Special</c>,
/// indicating non-standard handling or presentation. It supports translation
/// and uses the <c>isolateviewmapper</c> for result mapping.
/// The <c>ListItems</c> field specifies which columns are relevant for display or processing:
/// <c>Type</c>, <c>SpecimenQuantity</c>, <c>Comment1</c>, and <c>Comment2</c>.
/// Tagged with <c>SP</c> to denote special processing or categorization.
/// </remarks>
internal class IsolateForCultureViewQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing metadata for the "IsolateForCultureViewQuery",
    /// including table name, translation flag, result mapping, list items, and tags.
    /// </returns>
    public string Get()
    {
        return @"{
                'Query': 'IsolateForCultureViewQuery',
                'TableName': 'Culture',
                'Type': 'Special',
                'Translate': true,
                'ResultMapping': 'isolateviewmapper',
                'ListItems': 'Type, SpecimenQuantity, Comment1, Comment2',
                'Tags': 'SP'
            }";
    }
}
