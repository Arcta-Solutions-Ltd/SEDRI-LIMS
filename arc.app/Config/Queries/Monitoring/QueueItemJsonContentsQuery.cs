using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// This class defines the query to retrieve queue item JSON contents.
/// Implements the IDefinition interface.
/// </summary>
public class QueueItemJsonContentsQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query details as a JSON string.
    /// </summary>
    /// <returns>A JSON string containing the query details.</returns>
    public string Get()
    {
        return @"{
                    'Query': 'QueueItemJsonContents', 'TableName': 'Queue', 'Type': 'Single',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string'},
                        {'Name': 'Message', 'Type': 'string'}
                    ],
                    'Where' : [
                        {'Field': 'Id', 'Comparison': '=' }
                    ],
                    'Tags': 'PA,SP,MO'
                }";
    }
}
