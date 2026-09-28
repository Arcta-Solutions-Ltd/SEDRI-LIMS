using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Event configuration for deleting a specification.
/// </summary>
internal class DeleteSpecificationEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'deletespecification', 
                        Description: '@SpfDel@',
                        EventType : 'deletedata', 
                        Topic: 'Specification',
                        TableName: 'Specification',
                        DataRules: [
                            { type: 'NoRecord', query: 'specificationinuse', message: '@SpfValA@' }                  
                        ]
                    }";
    }
}
