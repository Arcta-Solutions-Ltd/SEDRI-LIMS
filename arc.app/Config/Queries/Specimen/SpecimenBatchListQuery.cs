using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class SpecimenBatchListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenBatchList', 
                        'TableName': 'Specimen', 
                        'Type': 'Special',
                        'Tags': 'BA'
                    }";
        }
    }
}
