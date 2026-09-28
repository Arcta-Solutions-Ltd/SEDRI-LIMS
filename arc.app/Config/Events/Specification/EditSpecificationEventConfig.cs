using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for editing an existing specification.
/// </summary>
internal class EditSpecificationEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'editspecification',
                        Description: '@SpfEdi@',
                        EventType: 'special', 
                        TableName: 'Specification',
                        Topic: 'Specification',
                        ValidationRules: [
                            { field: 'GuidelinesId', rule: 'required', message: '@SpfValB@'},
                            { field: 'DocumentId', rule: 'required', message: '@SpfValC@' } 
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'specificationexists', message: '@SpfVal@' }                          
                        ]
                    }";
    }
}
