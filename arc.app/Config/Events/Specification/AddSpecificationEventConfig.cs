using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for adding a new specification.
/// </summary>
internal class AddSpecificationEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'addspecification',
                        Description: '@SpfAdd@',
                        EventType: 'adddata', 
                        TableName: 'Specification',
                        Topic: 'Specification',
                        ValidationRules: [
                            { field: 'GuidelinesId', rule: 'required', message: '@SpfValB@'},
                            { field: 'DocumentId', rule: 'required', message: '@SpfValC@'}                       
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'specificationexists', message: '@SpfVal@' }
                        ]
                    }";
    }
}
