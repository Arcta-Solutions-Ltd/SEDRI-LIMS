using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrderListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrderList',
                        'TableName': 'OrderCat',
                        'Type': 'Special'
                    }";
        }
    }
}
