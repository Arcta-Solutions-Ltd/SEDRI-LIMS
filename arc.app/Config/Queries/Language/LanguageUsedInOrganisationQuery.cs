using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LanguageUsedInOrganisationQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'languageusedinorganisation', 'TableName': 'Organisation', 'Type': 'Count',
                        'ParameterMapping': 'languageusedinorganisationmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'LanguageId', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
