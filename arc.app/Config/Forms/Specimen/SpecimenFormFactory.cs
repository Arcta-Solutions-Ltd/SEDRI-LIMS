using arc.app.Common;
using arc.app.Config.Forms.Specimen;

namespace arc.app.Config.Forms;

/// <summary>
/// This factory class creates instances of various specimen form configurations.
/// </summary>
internal class SpecimenFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a form configuration based on the definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>An instance of the specified form configuration, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addspecimentagform" => new AddSpecimenTagFormConfig(),
            "batchaddspecimentagform" => new BatchAddSpecimenTagFormConfig(),
            "managespecimenattachmentsform" => new ManageSpecimenAttachmentsFormConfig(),
            "managecultureattachmentsform" => new ManageCultureAttachmentsFormConfig(),
            "addisolateform" => new AddIsolateFormConfig(),
            "ast" => new ASTFormConfig(),
            "batchspecimenapprovaloneform" => new BatchSpecimenApprovalOneConfig(),
            "batchspecimenapprovaltwoform" => new BatchSpecimenApprovalTwoConfig(),
            "batchsubmitconfirmationform" => new BatchSubmitConfirmationFormConfig(),
            "createneoshieldspecimenform" => new CreateNeoshieldSpecimenFormConfig(),
            "createneoshieldspecimenforpatientform" => new CreateNeoshieldSpecimenForPatientFormConfig(),
            "createspecimenreceivedform" => new CreateSpecimenReceivedFormConfig(),
            "createspecimenreceivedforpatientform" => new CreateSpecimenReceivedForPatientFormConfig(),
            "createspecimenrequestform" => new CreateSpecimenRequestFormConfig(),
            "createspecimenrequestforpatientform" => new CreateSpecimenRequestForPatientFormConfig(),
            "culturecommentform" => new CultureCommentFormConfig(),
            "culturelistform" => new CultureListFormConfig(),
            "deletecommentform" => new DeleteCommentFormConfig(),
            "deletecultureform" => new DeleteCultureFormConfig(),
            "deleteisolateform" => new DeleteIsolateFormConfig(),
            "editaliquotform" => new EditAliquotFormConfig(),
            "editcommentform" => new EditCommentFormConfig(),
            "editcommentforselectorform" => new EditCommentForSelectorFormConfig(),
            "editisolateform" => new EditIsolateFormConfig(),
            "editpatientcollectionform" => new EditPatientCollectionFormConfig(),
            "ordercommentsform" => new OrderCommentsFormConfig(),
            "removedirecttestform" => new RemoveDirectTestFormConfig(),
            "removeculturetestform" => new RemoveCultureTestFormConfig(),
            "restartspecimenform" => new RestartSpecimenFormConfig(),
            "specimencommentform" => new SpecimenCommentFormConfig(),
            _ => null,
        };
    }
}
