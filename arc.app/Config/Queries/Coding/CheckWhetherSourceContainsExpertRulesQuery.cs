using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for validating that a source (guidelines listitem) has no expert rules before deletion.
/// </summary>
internal class CheckWhetherSourceContainsExpertRulesQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition for the checkwhethersourcecontainsexpertrules validation.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'checkwhethersourcecontainsexpertrules', 'TableName': 'ExpertRule', 'Type': 'Special',
            'ParameterMapping': 'idtosourceidmapper'
        }";
    }
}
