using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for deleting a publication year list item from the specification list view.
/// </summary>
internal class DeleteYearEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'deleteyear', 
                        Description: '@SpfDelYea@',
                        EventType : 'deletedata', 
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@CodAE@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiD@' },
                            { type: 'NoRecord', query: 'checkwhetheryearinspecification', message: '@SpfYeaInUse@' }
                        ]
                    }";
    }
}
