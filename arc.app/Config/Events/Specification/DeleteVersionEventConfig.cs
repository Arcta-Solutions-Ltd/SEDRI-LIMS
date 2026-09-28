using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for deleting a version number list item from the specification list view.
/// </summary>
internal class DeleteVersionEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'deleteversion', 
                        Description: '@SpfDelVer@',
                        EventType : 'deletedata', 
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@CodAE@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiD@' },
                            { type: 'NoRecord', query: 'checkwhetherversioninspecification', message: '@SpfVerInUse@' }
                        ]
                    }";
    }
}
