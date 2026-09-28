using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class AntibioticEntryByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'antibioticentrybyidquery',
                        'TableName': 'AntibioticCoding',
                        'Type': 'Special'
                    }";
        }
    }
}
