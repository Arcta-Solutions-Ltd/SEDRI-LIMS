using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the "Specimen Record View."
/// </summary>
internal class SpecimenRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the specimen record view.
    /// </summary>
    /// <remarks>
    /// The configuration includes details such as the title, name, type, workflow, buttons,
    /// and regions for managing specimen records.
    /// Embedded culture actions (edit, add, delete) can change the parent specimen workflow state;
    /// <c>refreshRecordOnEmbeddedSave</c> ensures the Specimen Details region reloads after those saves.
    /// </remarks>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object representing the configured view.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@SpeSpeG@',
                        'name': 'specimenrecordview',
                        'type': 'recordview',
                        'singleItemName': '@SinSpe@',
                        'workflow': 'SpecimenDefault',
                        'refreshRecordOnEmbeddedSave': true,
                        singleQuery: 'singlespecimenforspecimenlist',
                        buttons: [
                            { key: 'ackreceipt', text: '@SpeAckA@', icon: 'ReceiptCheck', primaryAction: 2, uievent: 'ackreceiptuievent', workflow: true, onFinish: 'refresh' },
                            { key: 'cancelrequest', text: '@SpeCanA@', icon: 'Cancel', primaryAction: 3, uievent: 'specimencancelrequestuievent', workflow: true, onFinish: 'refresh' },
                            { key: 'rejectspecimen', text: '@SpeRej@', icon: 'Cancel', primaryAction: 10, uievent: 'rejectspecimen', workflow: true, onFinish: 'refresh' },
                            { key: 'testheader', text: '@SpeDir@', icon: 'TestExploreSolid', primaryAction: 4, uievent: 'directtestuievent', workflow: true,
                                buttons: [
                                    { key: 'testselection', text: '@GenSpeA@', icon: 'TestStep', uievent: 'testselectionuievent', workflow: true, onFinish: 'refresh' },
                                    { key: 'directtests', text: '@GenVieA@', icon: 'TestExploreSolid', uievent: 'directtestuievent', workflow: true, onFinish: 'refresh' }
                                ]
                            },
                            { key: 'addculture', text: '@SpeAddD@', icon: 'TestPlan', uievent: 'addcultureuievent', primaryAction: 1, workflow: true, onFinish: 'refresh' },
                            { key: 'submit', text: '@GenSub@', icon: 'Generate', primaryAction: 2, uievent: 'submitconfirmationuievent', workflow: true, onFinish: 'refresh' },
                            { key: 'approvalone', text: '@GenFir@', icon: 'DocumentApproval', primaryAction: 3, uievent: 'specimenapprovaloneuievent', workflow: true, onFinish: 'refresh' },
                            { key: 'approvaltwo', text: '@GenFirA@', icon: 'DocumentApproval', primaryAction: 4, uievent: 'specimenapprovaltwouievent', workflow: true, onFinish: 'refresh' },
                            { key: 'editspecimen', text: '@SpeEdiB@', icon: 'Edit', primaryAction: 5, uievent: 'editspecimenuievent', workflow: true, onFinish: 'refresh' },
                            { key: 'diary', text: '@GenDiaA@', icon: 'DietPlanNotebook', primaryAction: 6, uievent: 'specimendiaryuievent' },
                            { key: 'Reports', text: '@GenVieD@', icon: 'ReportDocument', workflow: false, primaryAction: 7, uievent: 'specimenreportuievent', onFinish: 'alwaysrefresh' },
                            { key: 'printbarcode1', text: '@GenPri@', icon: 'QRCode', primaryAction: 8, uievent: 'printspecimenbarcode1uievent', workflow: false },
                            { key: 'printbarcode2', text: '@GenPriA@', icon: 'QRCode', primaryAction: 9, uievent: 'printspecimenbarcode2uievent', workflow: false },
                            { key: 'AddComment', text: '@GenAddC@', icon: 'CommentAdd', primaryAction: 10, uievent: 'specimencommentuievent', workflow: false, onFinish: 'update' },
                            { key: 'movetopatient', text: '@PatMov@', icon: 'Merge', primaryAction: 11, uievent: 'movepatientuievent', onFinish: 'update', workflow: false },
                            { key: 'PrintPreview', text: '@GenTog@', icon: 'Print', primaryAction: 12, uievent: 'specimenprintpreviewuievent', workflow: false },
                            { key: 'addspecimentag', text: '@GenTagK@', icon: 'Tag', primaryAction: 13,uievent: 'addspecimentaguievent', onFinish: 'refresh' },
                            { key: 'manageattachments', text: '@GenAtts@', icon: 'Attach', uievent: 'managespecimenattachmentsuievent', workflow: false, onFinish: 'refresh' }
                        ],
                        'regions': [
                            { 'id': 'specimendetails', 'type': 'standard', 'queryName': 'specimenforspecimenview' },
                            { 'id': 'directtests', 'type': 'crafted', 'title': '@SpeDir@', 'name': 'directtestsgrid', 'queryName': 'testlistforspecimenlite' },
                            { 'id': 'cultures', 'type': 'listview', 'title': '@SpeCulB@', 'listViewName': 'cultures', 'name': 'isolatelist' },
                            { 'id': 'attachments', 'type': 'standard', 'queryName': 'specimenattachmentsforspecimenview', 'title': '@GenAtts@' },
                            { 'id': 'specimencomments', 'type': 'listview', 'title': '@GenComE@', 'listViewName': 'specimencomments' },
                            { 'id': 'reports', 'type': 'crafted', 'title': '@RepRep@', 'Name': 'reporthistorygrid', 'queryName': 'reportlist' },
                            { 'id': 'instrumentresults', 'type': 'listview', 'title': '@InsIns@', 'listViewName': 'specimeninstresults' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
