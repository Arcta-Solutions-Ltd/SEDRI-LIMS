using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SerotypeListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SerotypeList',
                        'TableName': 'Serotype',
                        'Type': 'Special'
                    }";
        }
    }
}
