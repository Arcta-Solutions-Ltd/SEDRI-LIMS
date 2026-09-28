using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Defines a specialized query configuration for retrieving culture-related data
/// intended for the "CultureView" context.
/// </summary>
/// <remarks>
/// This query is marked as <c>Special</c> and supports translation.
/// It maps results using <c>cultureviewmapper</c> and includes specific list items
/// for display or processing: <c>Type</c>, <c>SpecimenQuantity</c>, <c>Comment1</c>, and <c>Comment2</c>.
/// Tagged with <c>SP</c> to indicate special processing or categorization.
/// </remarks>
internal class CultureForCultureViewQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing metadata for the "CultureForCultureView" query,
    /// including table name, translation flag, result mapping, list items, and tags.
    /// </returns>
    public string Get()
    {
        return @"{
                'Query': 'CultureForCultureView',
                'TableName': 'Culture',
                'Type': 'Special',
                'Translate': true,
                'ResultMapping': 'cultureviewmapper',
                'ListItems': 'Type, SpecimenQuantity, Comment1, Comment2',
                'Tags': 'SP'
            }";
    }
}
