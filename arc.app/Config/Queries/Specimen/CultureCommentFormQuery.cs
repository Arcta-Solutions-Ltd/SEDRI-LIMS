using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CultureCommentFormQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CultureCommentFormQuery', 
                        'TableName': 'Culture', 
                        'Type': 'Single',
                        'ResultMapping': 'culturecommentformquerymapper',
                        'Fields': [
                            { 'Name': 'SpecimenId', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '='} //Wrong 'FieldToMatch': 'AntibioticId'
                        ]
                    }";
        }
    }
}
