using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the configuration for the ApproveReport special event.
/// </summary>
internal class ApproveReportEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the ApproveReport event configuration.
    /// </summary>
    /// <returns>A JSON string defining the ApproveReport event.</returns>
    public string Get()
    {
        return @"{ 
            EventName: 'approvereportevent', 
            Description: '@RepAppA@',
            EventType : 'special', 
            Topic : 'Report', 
            TableName: 'ReportHistory'
        }";
    }
}
