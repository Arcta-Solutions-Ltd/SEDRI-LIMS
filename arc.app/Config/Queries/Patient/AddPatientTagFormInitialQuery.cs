using arc.app.Common;

namespace arc.app.Config.Queries.Patient;

/// <summary>
/// Minimal query that returns Id and TagId (comma-separated ListItem IDs) for the add patient tag form.
/// Used to pre-populate the form with the patient's existing tags when opened from the record view.
/// </summary>
internal class AddPatientTagFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the add patient tag form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'AddPatientTagFormInitialQuery',
            'TableName': 'Patient',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'Tags', 'Type': 'patienttagids', 'KnownAs': 'TagId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
