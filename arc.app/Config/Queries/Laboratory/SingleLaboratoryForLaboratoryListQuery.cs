using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleLaboratoryForLaboratoryListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleLaboratoryForLaboratoryList',
                        'TableName': 'Laboratory',
                        'Type': 'special'
                    }";
        }
    }
}
