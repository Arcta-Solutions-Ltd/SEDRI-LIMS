using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for deleting a document type list item from the specification list view.
/// </summary>
internal class DeleteDocumentEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'deletedocument', 
                        Description: '@SpfDelDoc@',
                        EventType : 'deletedata', 
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@CodAE@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiD@' },
                            { type: 'NoRecord', query: 'checkwhetherdocumentinspecification', message: '@SpfDocInUse@' }
                        ]
                    }";
    }
}
