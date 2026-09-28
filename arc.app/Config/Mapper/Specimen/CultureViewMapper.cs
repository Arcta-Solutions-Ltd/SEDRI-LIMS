using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Defines the mapping configuration for the "cultureviewmapper", used to transform
/// culture-related data into structured output for display.
/// </summary>
/// <remarks>
/// This mapper is classified as <c>Standard</c> and includes a set of key-based rules
/// that map source fields (e.g., <c>SpecimenOrganism</c>, <c>Growth</c>, <c>PositiveDate</c>)
/// to their corresponding display tokens. The output is organized into sections and fields,
/// with support for labels, highlights, and nested subsections.
/// </remarks>
internal class CultureViewMapper : IDefinition
{
    /// <summary>
    /// Returns the mapping definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string describing the rules, target sections, and field mappings
    /// for the "cultureviewmapper" configuration.
    /// </returns>
    public string Get()
    {
        return @"{  
            'Name': 'cultureviewmapper',  
            'Type': 'Standard',
            'Rules': [
                { Key: '<:3:>', Type: 'Mapping', Source: 'Growth', Value: 'Growth' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'PositiveDate', Value: 'PositiveDate' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'PositiveTime', Value: 'PositiveTime' },
                { Key: '<:16:>', Type: 'Mapping', Source: 'Type', Value: 'Type' },
                { Key: '<:22:>', Type: 'Mapping', Source: 'CultureBottleWeight', Value: 'CultureBottleWeight' },
                { Key: '<:23:>', Type: 'Mapping', Source: 'CultureBloodAndBottleWeight', Value: 'CultureBloodAndBottleWeight' }
            ],
            'Target': 
                {
                    Sections: [
                        {
                            Id: 'culturedetails',
                            Title: '@SpeCul@',
                            Fields: [
                                { Id: 'type', Label: '@CulTyp@', Highlight: true, Value: '<:16:>' },
                                { Id: 'CultureBottleWeight', Label: '@SpeBot@', Highlight: true, Value: '<:22:>' },
                                { Id: 'CultureBloodAndBottleWeight', Label: '@SpeBlo@', Highlight: true, Value: '<:23:>' },
                                { Id: 'growth', Label: '@SpeGroC@', Highlight: true, Value: '<:3:>' }
                            ],
                            SubSections: [
                                {
                                    Id: 'positive', Title: '',
                                    Fields: [
                                        { Id: 'positivedate', Label: '@SpePosA@', Value: '<:6:>' },
                                        { Id: 'positivetime', Label: '@SpePosB@', Value: '<:7:>' }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }";
    }
}
