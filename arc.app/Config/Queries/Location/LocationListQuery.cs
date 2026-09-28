using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Defines the LocationList query that returns location list data including ParentLocationId for hierarchy views.
    /// </summary>
    internal class LocationListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'LocationList', 'TableName': 'Location', 'Type': 'Select', 
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'},
                            {'Name': 'Code','Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string' },
                            {'Name': 'Enabled', 'Type': 'string' },
                            {'Name': 'ParentLocationId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'Location', 'Type': 'Left','On': 'ParentLocationId', 'From': 'Id', 'Fields': [{'Name': 'Name', 'KnownAs': 'ParentLocation'}] }
                        ],                        
                        'Where' : [
                            {'Field': 'Name', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'Code', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'FullyQualifiedName', 'Comparison': 'contains', orGroup: 'search' }
                        ],
                        'OrderBy' : 'Name'
                    }";
        }
    }
}
