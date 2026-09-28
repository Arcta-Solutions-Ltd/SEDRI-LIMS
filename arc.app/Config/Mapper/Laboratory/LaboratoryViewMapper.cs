using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Configuration for mapping laboratory view data.
/// </summary>
internal class LaboratoryViewMapper : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for mapping laboratory view data.
    /// </summary>
    /// <remarks>
    /// This configuration includes mapping rules and target sections for transforming 
    /// laboratory-related data into a structured view.
    /// </remarks>
    /// <returns>
    /// A string representation of the mapping configuration, including metadata, rules, and target layout.
    /// </returns>
    public string Get()
    {
        return @"{
                    'Name': 'laboratoryviewmapper',
                    'Type': 'Standard',
                    'Rules': [
                        { Key: '<:1:>', Type: 'Mapping', Source: 'LaboratoryName', Value: 'LaboratoryName' }
                    ],
                    'Target': {
                        Sections: [
                            {
                                Id: 'laboratorydetails',
                                Title: '',
                                Fields: [
                                    { Id: 'laboratoryname', Label: '@LabLabA@', Highlight: true, Value: '<:1:>' }
                                ]
                            }
                        ]
                    }
                }";
    }
}


