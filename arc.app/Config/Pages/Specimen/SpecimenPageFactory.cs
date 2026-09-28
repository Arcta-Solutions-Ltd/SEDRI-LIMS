using arc.app.Common;
using arc.app.Config.Pages.Specimen;
using System;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory class responsible for creating page configuration definitions related to specimen workflows.
/// </summary>
internal class SpecimenPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a page configuration definition based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">
    /// The case-insensitive name of the page definition to create (e.g. <c>"specimentimings"</c>,
    /// <c>"specimentimingsreceived"</c>, <c>"advancespecimendetailspage"</c>).
    /// </param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name,
    /// or <c>null</c> if the name does not match any known configuration.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "ackreceiptpage" => new ACKReceiptPageConfig(),
            "ackreceiptpageforedit" => new AckReceiptForEditPageConfig(),
            "addspecimentagpage" => new AddSpecimenTagPageConfig(),
            "batchaddspecimentagpage" => new BatchAddSpecimenTagPageConfig(),
            "managespecimenattachmentspage" => new ManageSpecimenAttachmentsPageConfig(),
            "managecultureattachmentspage" => new ManageCultureAttachmentsPageConfig(),
            "advancespecimendetailspage" => new AdvanceSpecimenDetailsPageConfig(),
            "admissionselectionpage" => new AdmissionSelectionPageConfig(),
            "ast" => new ASTPageConfig(),
            "batchspecimenapprovalpage" => new BatchSpecimenApprovalPageConfig(),
            "batchspecimenapprovaltwopage" => new BatchSpecimenApprovalTwoPageConfig(),
            "culturecommentpage" => new CultureCommentPageConfig(),
            "cultureorganismpage" => new CultureOrganismPageConfig(),
            "culturetypeselectionpage" => new CultureTypeSelectionPageConfig(),
            "deletecommentpage" => new DeleteCommentPageConfig(),
            "deleteculturepage" => new DeleteCulturePageConfig(),
            "deleteisolatepage" => new DeleteIsolatePageConfig(),
            "editcommentpage" => new EditCommentPageConfig(),
            "editcommentforselectorpage" => new EditCommentForSelectorPageConfig(),
			"editculturepage" => new EditCulturePageConfig(),
            "neoshieldadmissionpage" => new NeoshieldAdmissionPageConfig(),
            "neoshieldbirthdetailspage" => new NeoshieldBirthDetailsPageConfig(),
            "neoshieldbottlepage" => new NeoshieldBottlePageConfig(),
            "neoshieldclinicalstatepage" => new NeoshieldClinicalStatePageConfig(),
            "neoshieldpatientidentificationpage" => new NeoshieldPatientIdentificationPageConfig(),
            "neoshieldrequestheaderpage" => new NeoshieldRequestHeaderPageConfig(),
            "neoshieldspecimenpage" => new NeoshieldSpecimenPageConfig(),
            "ordercommentspage" => new OrderCommentsPageConfig(),
            "patientcollectiondetails" => new PatientCollectionDetailsPageConfig(),
            "rejectspecimen" => new RejectSpecimenPageConfig(),
            "requestselectionpage" => new RequestSelectionPageConfig(),
            "requestselectionforadmissionpage" => new RequestSelectionForAdmissionPageConfig(),
            "restartspecimenpage" => new RestartSpecimenPageConfig(),
            "specimenaction" => new SpecimenActionPageConfig(),
            "specimenadditionalguidance" => new SpecimenAdditionalGuidancePageConfig(),
            "specimenapproval" => new SpecimenApprovalPageConfig(),
            "specimenapprovaltwopage" => new SpecimenApprovalTwoPageConfig(),
            "specimenattributes" => new SpecimenAttributesPageConfig(),
            "specimenattributeswhenreceived" => new SpecimenAttributesWhenReceivedPageConfig(),
            "specimencancelrequest" => new SpecimenCancelRequestPageConfig(),
            "specimencomment" => new SpecimenCommentPageConfig(),
            "specimengrowthdetails" => new SpecimenGrowthDetailsPageConfig(),
            "specimengrowthdetailsforisolatepage" => new SpecimenGrowthDetailsForIsolatePageConfig(),
            "specimenotherinformationpage" => new SpecimenOtherInformationPageConfig(),
            "specimenpatientdetails" => new SpecimenPatientDetailsPageConfig(),
            "specimentimings" => new SpecimenTimingsPageConfig(),
            "specimentimingsreceived" => new SpecimenTimingsForReceivedPageConfig(),
            "specimentimingswhenreceived" => new SpecimenTimingsWhenReceivedPageConfig(),
            "submitconfirmationpage" => new SubmitConfirmationPageConfig(),
            "day0benchread" => new Day0BenchReadPageConfig(),
            "day1benchread" => new Day1BenchReadPageConfig(),
            "editaliquotpage" => new EditAliquotPageConfig(),
            _ => null,
        }; ;
    }
}
