using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'AntibioticList',
                        'TableName': 'antibiotic',
                        'Type': 'Special'
                    }";
        }
    }
}


