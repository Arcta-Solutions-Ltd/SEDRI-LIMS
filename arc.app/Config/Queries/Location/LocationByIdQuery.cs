using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LocationByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'LocationById', 'TableName': 'Location', 'Type': 'Single', 
                        'Fields': [
                            {'Name': 'Name', 'Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string'},
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'ParentLocationId', 'Type': 'int'},
                            {'Name': 'Latitude', 'Type': 'int'},
                            {'Name': 'Longitude', 'Type': 'int'},
                            {'Name': 'Enabled', 'Type': 'string'},
                            {'Name': 'Code', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
