using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class GenusListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'GenusList',
                        'TableName': 'Genus',
                        'Type': 'Special'
                    }";
        }
    }
}
