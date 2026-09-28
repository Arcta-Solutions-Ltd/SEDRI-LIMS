using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleTestPatternForTestPatternListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleTestPatternForTestPatternList',
                        'TableName': 'TestPattern',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'FirstName', 'Type': 'string' },
                            {'Name': 'Surname', 'Type': 'string' },
                            {'Name': 'Age', 'Type': 'string' },
                            {'Name': 'PatientRef', 'Type': 'string' }
                        ],
                        'ListItems': 'Province, District, SubDistrict, Gender',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
