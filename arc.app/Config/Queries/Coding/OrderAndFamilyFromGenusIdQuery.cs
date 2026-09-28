using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class OrderAndFamilyFromGenusIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrderAndFamilyFromGenusId',
                        'TableName': 'Genus',
                        'Type': 'Special'
                    }";
        }
    }
}
