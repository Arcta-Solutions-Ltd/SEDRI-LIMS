using arc.app.Common;
using arc.app.Config.Events.Specimen;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Factory class for creating instances of specimen-related event configurations based on the provided definition name.
    /// </summary>
    internal class SpecimenEventFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates an instance of the specified event configuration.
        /// </summary>
        /// <param name="definitionName">The name of the event configuration to create.</param>
        /// <returns>An instance of the corresponding event configuration, or <c>null</c> if the definition name is not recognized.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "ackreceipt" => new ACKReceiptEventConfig(),
                "addculture" => new AddCultureEventConfig(),
                "addspecimentag" => new AddSpecimenTagEventConfig(),
                "addisolateevent" => new AddIsolateEventConfig(),
                "managespecimenattachments" => new ManageSpecimenAttachmentsEventConfig(),
                "managecultureattachments" => new ManageCultureAttachmentsEventConfig(),
                "batchaddspecimentag" => new BatchAddSpecimenTagEventConfig(),
                "batchspecimenapprovalone" => new BatchSpecimenApprovalOneEventConfig(),
                "batchspecimenapprovaltwo" => new BatchSpecimenApprovalTwoEventConfig(),
                "batchsubmitconfirmation" => new BatchSubmitConfirmationEventConfig(),
                "culturecommentevent" => new CultureCommentEventConfig(),
                "day0benchread" => new Day0BenchReadEventConfig(),
                "day1benchread" => new Day1BenchReadEventConfig(),
                "deletecomment" => new DeleteCommentEventConfig(),
                "deleteculture" => new DeleteCultureEventConfig(),
                "deleteisolateevent" => new DeleteIsolateEventConfig(),
                "editaliquotevent" => new EditAliquotEventConfig(),
                "editcomment" => new EditCommentEventConfig(),
                "editcommentforselector" => new EditCommentForSelectorEventConfig(),
                "editculture" => new EditCultureEventConfig(),
                "editisolateevent" => new EditIsolateEventConfig(),
                "editspecimen" => new EditSpecimenEventConfig(),
                "neoshieldspecimen" => new NeoshieldSpecimenEventConfig(),
                "newreceivedspecimen" => new ReceivedSpecimenEventConfig(),
                "ordercomments" => new OrderCommentsEventConfig(),
                "rejectspecimen" => new RejectSpecimenEventConfig(),
                "remotespecimen" => new RemoteSpecimenEventConfig(),
                "removeculturetest" => new RemoveCultureTestEventConfig(),
                "removedirecttest" => new RemoveDirectTestEventConfig(),
                "restartspecimen" => new RestartSpecimenEventConfig(),
                "specimenapprovalone" => new SpecimenApprovalOneEventConfig(),
                "specimenapprovaltwo" => new SpecimenApprovalTwoEventConfig(),
                "specimencancelrequest" => new SpecimenCancelRequestEventConfig(),
                "specimencomment" => new SpecimenCommentEventConfig(),
                "specimenprintpreview" => new SpecimenPrintPreviewEventConfig(),
                "submitspecimen" => new SubmitSpecimenEventConfig(),
                "viewspecimenrecord" => new ViewSpecimenRecordEventConfig(),
                _ => null,
            };
        }
    }
}
