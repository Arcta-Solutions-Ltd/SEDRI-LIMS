using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AlertQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "alertapprovalforminitialquery" => new AlertApprovalFormInitialQuery(),
                "alertapprovallistbyalertid" => new AlertApprovalListByAlertIdQuery(),
                "alertcategorylist" => new AlertCategoryListQuery(),
                "alertlist" => new AlertListQuery(),
                "alertviewquery" => new AlertViewQuery(),
                "alertsusceptibilitycriterialistbyalertid" => new AlertSusceptibilityCriteriaListByAlertIdQuery(),
                "alerttestcriterialistbyalertid" => new AlertTestCriteriaListByAlertIdQuery(),
                "alerttypeusedinalert" => new AlertTypeUsedInAlertQuery(),
                "editalert" => new EditAlertQuery(),
                "editalertcategory" => new EditAlertCategoryQuery(),
                "singlealertforalertlist" => new SingleAlertForAlertListQuery(),
                "singleforalertcategorylist" => new SingleForAlertCategoryListQuery(),
                "specimenalertlist" => new SpecimenAlertListQuery(),
                "tagexists" => new TagExistsQuery(),
                _ => null,
            };
        }
    }
}
