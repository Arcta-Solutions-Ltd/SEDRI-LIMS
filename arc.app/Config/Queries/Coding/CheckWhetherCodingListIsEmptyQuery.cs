using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class CheckWhetherCodingListIsEmptyQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'checkwhethercodinglistisempty', 'TableName': 'organismcoding', 'Type': 'Count',
                         'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=', 'FieldToMatch': 'CodingId' }
                        ]
                    }";
        }
    }
}

