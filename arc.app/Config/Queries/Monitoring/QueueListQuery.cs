using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// This class defines the query to retrieve the queue list.
/// Implements the IDefinition interface.
/// </summary>
internal class QueueListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query details as a JSON string.
    /// </summary>
    /// <returns>A JSON string containing the query details.</returns>
    public string Get()
    {
        return @"{ 
                        'Query': 'QueueList', 
                        'TableName': 'Queue', 
                        'Type': 'special', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'Username', 'Type': 'string'},
                            { 'Name': 'Added', 'Type': 'jdate'}
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Type': 'Left', 'Fields': [{'Name': 'PatientRef'}] },
                            { 'Table': 'Specimen', 'Type': 'Left', 'Fields': [{'Name': 'AccessionNumber'}] }
                        ],
                        'ListItems': 'Topic, Event, EventStatus',
                        'Where' : [
                            {'Field': 'TopicId', 'Comparison': 'oneof' },
                            {'Field': 'EventId', 'Comparison': 'oneof' },
                            {'Field': 'EventStatusId', 'Comparison': 'oneof' },
                            {'Field': 'Username', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'AccessionNumber', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'PatientRef', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'Topic', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'Event', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'EventStatus', 'Comparison': 'contains', orGroup: 'search' }
                        ],
                        'Orderby': 'Added',
                        'Descending': true,
                        'Tags': 'MO'
                    }";
    }
}

