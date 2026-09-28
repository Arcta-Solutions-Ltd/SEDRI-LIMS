using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration definition for the batch reject report event.
/// </summary>
internal class BatchRejectReportEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch reject report event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string defining:
    /// - EventName: "batchrejectreportevent"
    /// - Description: "@RepBatD@"
    /// - EventType: "special"
    /// - Topic: "Report"
    /// - TableName: "ReportHistory"
    /// - BatchEvent: "approverejectevent"
    /// </returns>
    public string Get()
    {
        return @"{ 
            EventName: 'batchrejectreportevent', 
            Description: '@RepBatD@',
            EventType : 'batch', 
            Topic : 'Report', 
            TableName: 'ReportHistory',
            BatchEvent: 'unapprovereportevent'
        }";
    }
}
