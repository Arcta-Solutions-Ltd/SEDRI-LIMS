using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration definition for the batch approve report event.
/// </summary>
internal class BatchApproveReportEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch approve report event configuration as a JSON‐formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string defining:
    /// - EventName: "batchapprovereportevent"
    /// - Description: "@RepAppD@"
    /// - EventType: "special"
    /// - Topic: "Report"
    /// - TableName: "ReportHistory"
    /// - BatchEvent: "approvereportevent"
    /// </returns>
    public string Get()
    {
        return @"{ 
            EventName: 'batchapprovereportevent', 
            Description: '@RepAppD@',
            EventType : 'batch', 
            Topic : 'Report', 
            TableName: 'ReportHistory',
            BatchEvent: 'approvereportevent'
        }";
    }
}
