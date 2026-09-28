using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class CultureIdListForSpecimenReportQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'CultureIdListForSpecimenReport',
                        'TableName': 'Culture',
                        'Type': 'Select',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' }
                        ],
                        'Where' : [
                            {'Field': 'SpecimenId', 'Comparison': '=' }
                        ],
                        'Orderby': 'Id'
                    }";
        }
    }
}

