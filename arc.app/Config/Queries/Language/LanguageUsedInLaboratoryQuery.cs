using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LanguageUsedInLaboratoryQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'languageusedinlaboratory', 'TableName': 'Laboratory', 'Type': 'Count',
                        'ParameterMapping': 'languageusedinlaboratorymapper',
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
