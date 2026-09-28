using arc.app.Common;
using arc.app.Config.Queries.Admission;
using arc.app.Config.Queries.Asset;
using arc.app.Config.Queries.Billing;
using arc.app.Config.Queries.ExpertRules;
using arc.app.Config.Queries.Images;
using arc.app.Config.Queries.Specification;
using arc.domain.Configuration.QueryConfig;
using System.Collections.Generic;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class responsible for creating and retrieving query configurations.
/// </summary>
public class QueryFactory : IQueryFactory
{
    /// <summary>
    /// Retrieves the query configuration by name.
    /// </summary>
    /// <param name="name">The name of the query configuration to retrieve.</param>
    /// <returns>A <see cref="QueryConfig"/> object representing the specified query configuration.</returns>
    public QueryConfig GetQuery(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new AdmissionQueryFactory(),
            new AlertQueryFactory(),
            new AssetQueryFactory(),
            new BillingQueryFactory(),
            new ASTQueryFactory(),
            new CodingQueryFactory(),
            new ConfigQueryFactory(),
            new ExpertRuleQueryFactory(),
            new ExportQueryFactory(),
            new ImageQueryFactory(),
            new InstrumentsQueryFactory(),
            new LaboratoryQueryFactory(),
            new LanguageQueryFactory(),
            new ListQueryFactory(),
            new LocationQueryFactory(),
            new OrganisationQueryFactory(),
            new PatientQueryFactory(),
            new QueueQueryFactory(),
            new QualityQueryFactory(),
            new ReportQueryFactory(),
            new RoleQueryFactory(),
            new SettingsQueryFactory(),
            new SpecificationQueryFactory(),
            new SpecimenQueryFactory(),
            new TestsQueryFactory(),
            new UserQueryFactory()
        }.GetConfigByName<QueryConfig>(name);
    }
}
