using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for loading test criteria for a given alert.
/// Used by the test criteria list view section on the alert record view.
/// </summary>
internal class AlertTestCriteriaListByAlertIdQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'AlertTestCriteriaListByAlertId',
            'TableName': 'alerttestlines',
            'Type': 'Special',
            'Where': [
                { 'Field': 'AlertId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}
