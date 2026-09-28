using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for loading susceptibility criteria for a given alert.
/// Used by the susceptibility criteria list view section on the alert record view.
/// </summary>
internal class AlertSusceptibilityCriteriaListByAlertIdQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'AlertSusceptibilityCriteriaListByAlertId',
            'TableName': 'alertlines',
            'Type': 'Special',
            'Where': [
                { 'Field': 'AlertId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}
