using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleForAlertCategoryListQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'singleforalertcategorylist', 'TableName': 'AlertType', 'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'}
                        ],
                        'ListItems': 'Position, ReportPosition, AlertCategory',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
