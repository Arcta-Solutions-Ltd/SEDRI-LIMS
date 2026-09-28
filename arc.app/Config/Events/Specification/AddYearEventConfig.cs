using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for adding a new publication year list item to the specification list view.
/// </summary>
internal class AddYearEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'addyear', 
                        Description: '@SpfAddYea@',
                        EventType : 'adddata', 
                        Mapping: 'addyearmapper',
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodAD@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'yearlistitemexists', message: '@SpfYeaAlrExi@' }
                        ]
                    }";
    }
}
