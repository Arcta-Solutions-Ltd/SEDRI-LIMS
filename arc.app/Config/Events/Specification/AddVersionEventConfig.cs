using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for adding a new version number list item to the specification list view.
/// </summary>
internal class AddVersionEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'addversion', 
                        Description: '@SpfAddVer@',
                        EventType : 'adddata', 
                        Mapping: 'addversionmapper',
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodAD@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'versionlistitemexists', message: '@SpfVerAlrExi@' }
                        ]
                    }";
    }
}
