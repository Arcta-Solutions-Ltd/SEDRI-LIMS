using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the configuration for the UnapproveReport special event.
/// </summary>
internal class UnapproveReportEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the UnapproveReport event configuration.
    /// </summary>
    /// <returns>A JSON string defining the UnapproveReport event.</returns>
    public string Get()
    {
        return @"{ 
            EventName: 'unapprovereportevent', 
            Description: '@RepUna@',
            EventType : 'special', 
            Topic : 'Report', 
            TableName: 'ReportHistory'
        }";
    }
}
