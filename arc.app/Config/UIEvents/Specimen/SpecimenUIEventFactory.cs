using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Factory class for creating instances of specimen-related UI event configurations based on the provided definition name.
    /// </summary>
    internal class SpecimenUIEventFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates an instance of the specified UI event configuration.
        /// </summary>
        /// <param name="definitionName">The name of the UI event configuration to create.</param>
        /// <returns>An instance of the corresponding UI event configuration, or <c>null</c> if the definition name is not recognized.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "ackreceiptuievent" => new ACKReceiptUIEventConfig(),
                "addcultureuievent" => new AddCultureUIEventConfig(),
                "managespecimenattachmentsuievent" => new ManageSpecimenAttachmentsUIEventConfig(),
                "addspecimentaguievent" => new AddSpecimenTagUIEventConfig(),
                "addisolateuievent" => new AddIsolateUIEventConfig(),
                "managecultureattachmentsuievent" => new ManageCultureAttachmentsUIEventConfig(),
                "addspecimentests" => new AddSpecimenTestsUIEventConfig(),
                "ast" => new ASTUIEventConfig(),
                "batchaddspecimentaguievent" => new BatchAddSpecimenTagUIEventConfig(),
                "batchspecimenapprovaloneuievent" => new BatchSpecimenApprovalOneUIEventConfig(),
                "batchspecimenapprovaltwouievent" => new BatchSpecimenApprovalTwoUIEventConfig(),
                "batchsubmitconfirmationuievent" => new BatchSubmitConfirmationUIEventConfig(),
                "createneoshieldspecimen" => new CreateNeoshieldSpecimenUIEventConfig(),
                "createneoshieldspecimenforpatientuievent" => new CreateNeoshieldSpecimenForPatientUIEventConfig(),
                "createspecimenreceived" => new CreateSpecimenReceivedUIEventConfig(),
                "createspecimenreceivedforpatientuievent" => new CreateSpecimenReceivedForPatientUIEventConfig(),
                "createspecimenrequest" => new CreateSpecimenRequestUIEventConfig(),
                "createspecimenrequestforpatientuievent" => new CreateSpecimenRequestForPatientUIEventConfig(),
                "culturecommentuievent" => new CultureCommentUIEventConfig(),
                "deletecultureuievent" => new DeleteCultureUIEventConfig(),
                "deletecommentuievent" => new DeleteCommentUIEventConfig(),
                "deleteisolateuievent" => new DeleteIsolateUIEventConfig(),
                "editcommentuievent" => new EditCommentUIEventConfig(),
                "editcommentforselectoruievent" => new EditCommentForSelectorUIEventConfig(),
                "editcultureuievent" => new EditCultureUIEventConfig(),
                "editisolateuievent" => new EditIsolateUIEventConfig(),
                "editpatientcollection" => new EditPatientCollectionUIEventConfig(),
                "editspecimenuievent" => new EditSpecimenUIEventConfig(),
                "ordercommentsuievent" => new OrderCommentsUIEventConfig(),
                "printspecimenbarcode1uievent" => new PrintSpecimenBarcode1UIEventConfig(),
                "printspecimenbarcode2uievent" => new PrintSpecimenBarcode2UIEventConfig(),
                "rejectspecimen" => new RejectSpecimenUIEventConfig(),
                "rejectspecimencondition" => new RejectSpecimenConditionUIEventConfig(),
                "removeculturetestuievent" => new RemoveCultureTestUIEventConfig(),
                "removedirecttestuievent" => new RemoveDirectTestUIEventConfig(),
                "restartspecimenuievent" => new RestartSpecimenUIEventConfig(),
                "specimenapprovaloneuievent" => new SpecimenApprovalOneUIEventConfig(),
                "specimenapprovaltwouievent" => new SpecimenApprovalTwoUIEventConfig(),
                "specimencancelrequestuievent" => new SpecimenCancelRequestUIEventConfig(),
                "specimencommentuievent" => new SpecimenCommentUIEventConfig(),
                "specimendiaryuievent" => new SpecimenDiaryUIEventConfig(),
                "specimenprintpreviewuievent" => new SpecimenPrintPreviewUIEventConfig(),
                "submitconfirmationuievent" => new SubmitConfirmationUIEventConfig(),
                "viewspecimenrecord" => new ViewSpecimenRecordUIEventConfig(),
                "viewculturerecord" => new ViewCultureRecordUIEventConfig(),
                "viewtestrecord" => new ViewTestRecordUIEventConfig(),
                "day0benchread" => new Day0BenchReadUIEventConfig(),
                "day1benchread" => new Day1BenchReadUIEventConfig(),
                "editaliquotuievent" => new EditAliquotUIEventConfig(),
                _ => null,
            };
        }
    }
}
