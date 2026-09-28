using arc.app.Common;

namespace arc.app.Config.Queries.Location
{
    internal class SingleLocationForLocationListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleLocationForLocationList', 'TableName': 'Location', 'Type': 'Single', 
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'},
                            {'Name': 'Code','Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string' },
                            {'Name': 'Enabled', 'Type': 'string' }
                        ],
                        'Joins': [
                            { 'Table': 'Location', 'Type': 'Left','On': 'ParentLocationId', 'From': 'Id', 'Fields': [{'Name': 'Name', 'KnownAs': 'ParentLocation'}] }
                        ],                        
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
