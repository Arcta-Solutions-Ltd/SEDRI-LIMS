using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Maps admission query results to the admission record view header sections.
/// </summary>
internal class AdmissionViewMapper : IDefinition
{
    /// <summary>
    /// Returns the JSON mapping definition for the admission record view.
    /// </summary>
    /// <returns>Mapper definition with field mappings and section layout.</returns>
    public string Get()
    {
        return @"{
            Name: 'admissionviewmapper',
            Type: 'Standard',
            Rules: [
                { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'AdmissionDateTimeKnown', Value: 'AdmissionDateTimeKnown' },
                { Key: '<:3:>', Type: 'Mapping', Source: 'DateOfAdmission', Value: 'DateOfAdmission' },
                { Key: '<:4:>', Type: 'Mapping', Source: 'TimeOfAdmission', Value: 'TimeOfAdmission' }
            ],
            Target: {
                Sections: [
                    {
                        Id: 'admissiondetails',
                        Title: '@NeoAdm@',
                        Fields: [
                            { Id: 'AdmissionDateTimeKnown', Label: '@NeoAdmKno@', Value: '<:2:>' },
                            { Id: 'DateOfAdmission', Label: '@NeoAdmDat@', Value: '<:3:>' },
                            { Id: 'TimeOfAdmission', Label: '@NeoAdmTim@', Value: '<:4:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
