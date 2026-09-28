using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Minimal query that returns Id and TagId (comma-separated ListItem IDs) for the add specimen tag form.
/// Used to pre-populate the form with the specimen's existing tags when opened from the record view.
/// </summary>
internal class AddSpecimenTagFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the add specimen tag form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'AddSpecimenTagFormInitialQuery',
            'TableName': 'Specimen',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'Tags', 'Type': 'specimentagids', 'KnownAs': 'TagId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
