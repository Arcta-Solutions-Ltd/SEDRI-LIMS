using arc.app.Common;
using arc.app.Config.Queries.Specimen;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class responsible for creating query definitions based on a string identifier.
/// Implements <see cref="IDefinitionFactory"/> to support dynamic query resolution.
/// </summary>
internal class SpecimenQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a query definition based on the provided <paramref name="definitionName"/>.
    /// </summary>
    /// <param name="definitionName">The name of the query definition to instantiate.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> implementation matching the name, or <c>null</c> if no match is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addspecimentagforminitialquery" => new AddSpecimenTagFormInitialQuery(),
            "managespecimenattachmentsforminitialquery" => new ManageSpecimenAttachmentsFormInitialQuery(),
            "managecultureattachmentsforminitialquery" => new ManageCultureAttachmentsFormInitialQuery(),
            "addculturequery" => new AddCultureQuery(),
            "addculturecommentquery" => new AddCultureCommentQuery(),
            "aliquotforeditquery" => new AliquotForEditQuery(),
            "commentlistbyspecimenid" => new CommentListBySpecimenIdQuery(),
            "culturebyid" => new CultureByIdQuery(),
            "culturebyidforisolatequery" => new CultureByIdForIsolateQuery(),
            "culturecommentformquery" => new CultureCommentFormQuery(),
            "culturecommentlistquery" => new CultureCommentListQuery(),
            "cultureattachmentsforcultureview" => new CultureAttachmentsForCultureViewQuery(),
            "cultureforcultureview" => new CultureForCultureViewQuery(),
            "cultureidlistforspecimenreport" => new CultureIdListForSpecimenReportQuery(),
            "culturelistbyspecimenid" => new CultureListBySpecimenIdQuery(),
            "culturelistforspecimenreport" => new CultureListForSpecimenReportQuery(),
            "cultureviewdetailsquery" => new CultureViewDetailsQuery(),
            "deletecommentquery" => new DeleteCommentQuery(),
            //"deleteculturequery" => new DeleteCultureQuery(),
            "editcommentquery" => new EditCommentQuery(),
            "isolateforcultureviewquery" => new IsolateForCultureViewQuery(),
            "removeculturetestquery" => new RemoveCultureTestQueryConfig(),
            "removedirecttestquery" => new RemoveDirectTestQueryConfig(),
            "singlespecimenforspecimenlabel" => new SingleSpecimenForSpecimenLabelQuery(),
            "singlespecimenforspecimenlist" => new SingleSpecimenForSpecimenListQuery(),
            "specimenarchivelist" => new SpecimenArchiveListQuery(),
            "specimenbatchlist" => new SpecimenBatchListQuery(),
            "specimenbyidforack" => new SpecimenByIdForACKQuery(),
            "specimenbyidforcancelrequest" => new SpecimenByIdForCancelRequestQuery(),
            "specimenbyidforedit" => new SpecimenByIdForEditQuery(),
            "specimendiaryentryquery" => new SpecimenDiaryEntryQuery(),
            "specimenattachmentsforspecimenview" => new SpecimenAttachmentsForSpecimenViewQuery(),
            "specimenforspecimenview" => new SpecimenForSpecimenViewQuery(),
            "specimenlabelavailablefields" => new SpecimenLabelAvailableFieldsQuery(),
            "specimenlist" => new SpecimenListQuery(),
            "specimenlistbypatientid" => new SpecimenListByPatientIdQuery(),
            "specimenlistbyadmissionid" => new SpecimenListByAdmissionIdQuery(),
            "specimenlistbyrequestid" => new SpecimenListByRequestIdQuery(),
            "specimenstatecount" => new SpecimenStateCountQuery(),
            "homedashboardrecentlyused" => new HomeDashboardRecentlyUsedQuery(),
            "homedashboardtatcompliance" => new HomeDashboardTatComplianceQuery(),
            "specimentagcount" => new SpecimenTagCountQuery(),
            "specimentypecount" => new SpecimenTypeCountQuery(),
            _ => null,
        };
    }
}

