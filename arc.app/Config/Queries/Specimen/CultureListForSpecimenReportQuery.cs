using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class CultureListForSpecimenReportQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'CultureListForSpecimenReport',
                        'TableName': 'Culture',
                        'Type': 'Select',
                        'ResultMapping': 'culturereportmapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' },
                            { 'Name': 'SpecimenId', 'Type': 'int' }
                        ],
                        'ListItems': 'SpecimenOrganism, SpecimenQuantity, ESBL',
                        'Where' : [
                            {'Field': 'SpecimenId', 'Comparison': '=' }
                        ],
                        'Orderby': 'Id'
                    }";
        }
    }
}
