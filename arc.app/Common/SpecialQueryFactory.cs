using arc.app.AST;
using arc.app.Coding;
using arc.app.Configuration.Queries;
using arc.app.Instruments;
using arc.app.Laboratory;
using arc.app.List;
using arc.app.Reports;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.app.Images.Queries;
using arc.app.Tests;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace arc.app.Common;

/// <summary>
/// Provides a factory for creating special query instances based on query names.
/// </summary>

public class SpecialQueryFactory : ISpecialQueryFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ImageSpecialQueryFactory _imageSpecialQueryFactory;


    /// <summary>
    /// Initializes a new instance of the <see cref="SpecialQueryFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve query dependencies.</param>

    public SpecialQueryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _imageSpecialQueryFactory = new ImageSpecialQueryFactory(_serviceProvider);
    }


    /// <summary>
    /// Retrieves an instance of <see cref="IQueryRun"/> corresponding to the specified query name.
    /// </summary>
    /// <param name="queryName">The name of the query to create.</param>
    /// <returns>
    /// An instance of <see cref="IQueryRun"/> if the query name is recognized; otherwise, <c>null</c>.
    /// </returns>

    public IQueryRun GetQuery(string queryName)
    {
        var imageQuery = _imageSpecialQueryFactory.GetQuery(queryName);
        if (imageQuery != null)
        {
            return imageQuery;
        }

        return queryName.ToLower() switch
        {
            "activeculturetestbyidfortestlistquery" => new ActiveCultureTestByIdForTestListQuery(_serviceProvider),
            "activeculturetestlistquery" => new ActiveCultureTestListQuery(_serviceProvider),
            "activetestbyidfortestlistquery" => new ActiveTestByIdForTestListQuery(_serviceProvider),
            "activetestlistquery" => new ActiveTestListQuery(_serviceProvider),
            "antibioticentrybyidquery" => new AntibioticEntryByIdQuery(_serviceProvider),
            "antibioticlist" => new AntibioticListQuery(_serviceProvider),
            "approvereportformquery" => new ApproveReportFormQuery(_serviceProvider),
            "approvedreportslistquery" => new ApprovedReportsListQuery(_serviceProvider),
            "alertviewquery" => new arc.app.Alert.AlertViewQuery(_serviceProvider),
            "breakpointviewquery" => new BreakpointViewQuery(_serviceProvider),
            "expertruleviewquery" => new arc.app.ExpertRule.ExpertRuleViewQuery(_serviceProvider),
            "commentlistbyspecimenid" => new CommentListBySpecimenIdQuery(_serviceProvider),
            "culturebyidforisolatequery" => new CultureByIdForIsolateQuery(_serviceProvider),
            "cultureinstrumentresults" => new CultureInstrumentsResultsQuery(_serviceProvider),
            "deletemappingvalidationquery" => new GetDeleteMappingValidationQuery(_serviceProvider),
            "editexpertruletestconditionquery" => new arc.app.ExpertRule.EditExpertRuleTestConditionQuery(_serviceProvider),
            "editexpertruleconditionquery" => new arc.app.ExpertRule.EditExpertRuleConditionQuery(_serviceProvider),
            "editexpertruleactionquery" => new arc.app.ExpertRule.EditExpertRuleActionQuery(_serviceProvider),
            "editinstrumentprofilequery" => new EditInstrumentProfileQuery(_serviceProvider),
            "getculturetestsforculture" => new GetCultureTestsForCultureQuery(_serviceProvider),
            "getorcreateculturetestid" => new arc.app.Tests.GetOrCreateCultureTestIdQuery(_serviceProvider),
            "instrumenterrorjsoncontentsquery" => new ViewInstrumentResultDetailsQuery(_serviceProvider),
            "instrumentrequestquery" => new InstrumentRequestQuery(_serviceProvider),
            "instrumentresultslistquery" => new InstrumentResultsListQuery(_serviceProvider),
            "instrumentprofilelistquery" => new InstrumentListQuery(_serviceProvider),
            "removeculturetestquery" => new RemoveCultureTestQuery(_serviceProvider),
            "requestinstrumenttestforminitialquery" => new RequestInstrumentTestFormInitialQuery(
                _serviceProvider.GetRequiredService<IConfigRepository>(),
                _serviceProvider.GetRequiredService<IInstrumentRepository>(),
                _serviceProvider.GetRequiredService<ITestRepository>()),
            "removedirecttestquery" => new RemoveDirectTestQuery(_serviceProvider),
            "singleinstrumentprofilelistquery" => new SingleInstrumentProfileListQuery(_serviceProvider),
            "specimeninstrumentresultsquery" => new SpecimenInstrumentResultsQuery(_serviceProvider),
            "testinstrumentresultsquery" => new TestInstrumentResultsQuery(_serviceProvider),
            "singletagfortaglist" => new SingleTagForTagListQuery(_serviceProvider),
            "synonymsfororganismquery" => new SynonymsForOrganismQuery(_serviceProvider),
            "taglist" => new TagListQuery(_serviceProvider),
            "testlistforculture" => new TestListForCultureQueryHandler(_serviceProvider),
            "testlistforculturelite" => new TestListForCultureLiteQueryHandler(_serviceProvider),
            "testlistforspecimen" => new TestListForSpecimenQueryHandler(_serviceProvider),
            "testlistforspecimenlite" => new TestListForSpecimenLiteQueryHandler(_serviceProvider),
            "testrecordviewbyid" => new arc.app.Tests.TestRecordViewByIdQuery(_serviceProvider),
            "instrumentresultrecordviewbyid" => new arc.app.Instruments.InstrumentResultRecordViewByIdQuery(_serviceProvider),
            "unapprovedreportslistquery" => new UnapprovedReportsListQuery(_serviceProvider),
            "unapprovedreportslistbyidquery" => new ApprovedReportListByIdQuery(_serviceProvider),
            "updatetablelistquery" => new UpdateTableListQuery(_serviceProvider),
            "workflowlistquery" => new WorkflowListQuery(_serviceProvider),
            "turnaroundtimeforminitialquery" => new TurnAroundTimeFormInitialQueryHandler(_serviceProvider),
            _ => null,

        };
    }
}
