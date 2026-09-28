using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration used for viewing alert details.
/// </summary>
internal class ViewAlertUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'viewalertuievent',
            description: 'View alert details',
            type: 'view-record',
            action: 'alerts'
        }";
    }
}
