using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for loading an expert rule condition for delete confirmation.
/// </summary>
internal class DeleteExpertRuleConditionQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON query configuration for the delete expert rule condition form.
    /// </summary>
    /// <returns>JSON string defining the single-record query and result mapping.</returns>
    public string Get()
    {
        return @"{ 
                        'Query': 'deleteexpertruleconditionquery', 
                        'TableName': 'ExpertRuleCondition', 
                        'Type': 'Single',
                        'ResultMapping': 'deleteexpertruleconditionquerymapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' },
                            { 'Name': 'AntibioticId', 'Type': 'int' },
                            { 'Name': 'AntibioticGroupId', 'Type': 'int' },
                            { 'Name': 'SusceptibilityId', 'Type': 'int' }
                        ],
                        'Joins': [
                            { 'Table': 'Antibiotic', 'Type': 'Left', 'On': 'AntibioticId', 'From': 'Id', 'Fields': [{ 'Name': 'AntibioticName' }] },
                            { 'Table': 'ListItem', 'Type': 'Left', 'On': 'SusceptibilityId', 'From': 'Id', 'Fields': [{ 'Name': 'Value', 'As': 'SusceptibilityName' }] }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
    }
}
