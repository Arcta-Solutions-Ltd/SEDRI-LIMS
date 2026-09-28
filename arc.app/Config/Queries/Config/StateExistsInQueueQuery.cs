using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class StateExistsInQueueQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'stateexistsinqueuequery', 'TableName': 'Queue', 'Type': 'Count',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'StateId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
