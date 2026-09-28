using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class FamilyListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'FamilyList',
                        'TableName': 'Family',
                        'Type': 'Special'
                    }";
        }
    }
}
