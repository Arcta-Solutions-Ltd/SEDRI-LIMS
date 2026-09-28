using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditAlertCategoryQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'editalertcategory', 'TableName': 'AlertType', 'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'},
                            {'Name': 'Colour', 'Type': 'string'},
                            {'Name': 'PositionId', 'Type': 'string'},
                            {'Name': 'AlertCategoryId', 'Type': 'string'},
                            {'Name': 'ReportPositionId', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
