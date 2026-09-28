using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AlertCategoryListQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'alertcategorylist', 'TableName': 'AlertType', 'Type': 'Select',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'}
                        ],
                        'ListItems': 'Position, ReportPosition, AlertCategory',
                        'Where' : [
                            {'Field': 'AlertCategoryId', 'Comparison': 'oneof' },
                            {'Field': 'PositionId', 'Comparison': 'oneof' },
                            {'Field': 'ReportPositionId', 'Comparison': 'oneof' },
                            {'Field': 'Name', 'Comparison': 'contains', orGroup: 'search'},                            
                            {'Field': 'Position', 'Comparison': 'contains', orGroup: 'search'},
                            {'Field': 'ReportPosition', 'Comparison': 'contains', orGroup: 'search'},
                            {'Field': 'AlertCategory', 'Comparison': 'contains', orGroup: 'search'}
                        ]
                    }";
        }
    }
}
