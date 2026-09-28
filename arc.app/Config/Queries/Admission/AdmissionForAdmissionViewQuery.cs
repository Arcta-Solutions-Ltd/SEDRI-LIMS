using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Query definition returning a single admission for the admission record view header.
/// </summary>
internal class AdmissionForAdmissionViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                        Query: 'AdmissionForAdmissionView',
                        TableName: 'Admission',
                        Type: 'Single',
                        Translate: true,
                        ResultMapping: 'admissionviewmapper',
                        Fields: [
                            { Name: 'Id', Type: 'int' },
                            { Name: 'PatientId', Type: 'int' },
                            { Name: 'DateOfAdmission', Type: 'date' },
                            { Name: 'TimeOfAdmission', Type: 'string' }
                        ],
                        MultiSelectItems: 'AdmissionDateTimeKnown',
                        Where: [
                            { Field: 'Id', Comparison: '=' }
                        ]
                    }";
    }
}
