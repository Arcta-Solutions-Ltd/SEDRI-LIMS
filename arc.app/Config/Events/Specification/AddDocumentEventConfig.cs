using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for adding a new document type list item to the specification list view.
/// </summary>
internal class AddDocumentEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        EventName: 'adddocument', 
                        Description: '@SpfAddDoc@',
                        EventType : 'adddata', 
                        Mapping: 'adddocumentmapper',
                        Topic: 'Specification',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodAD@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'documentlistitemexists', message: '@SpfDocAlrExi@' }
                        ]
                    }";
    }
}
