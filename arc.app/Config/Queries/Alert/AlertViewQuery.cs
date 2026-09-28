using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Definition for the alert view query. Supplies the query configuration used when loading
/// a single alert for view (e.g. on an alert record/detail screen).
/// </summary>
internal class AlertViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'AlertViewQuery',
            'TableName': 'Alert',
            'Type': 'Special',
            'ResultMapping': 'alertviewmapper',
            'Translate': true
        }";
    }
}
