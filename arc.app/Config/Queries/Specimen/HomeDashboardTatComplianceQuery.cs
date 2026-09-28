using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for home dashboard TAT Compliance KPI (<see cref="SpecialFactory"/> / <c>query/filteredget</c>).
/// </summary>
internal class HomeDashboardTatComplianceQuery : IDefinition
{
    public string Get()
    {
        return @"{ 
                        'Query': 'homedashboardtatcompliance',
                        'TableName': 'Specimen',
                        'Type': 'Special'
                    }";
    }
}
