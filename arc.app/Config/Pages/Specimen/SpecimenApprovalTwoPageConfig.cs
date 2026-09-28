using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenApprovalTwoPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'specimenapprovaltwopage',
                            pageTitle: '@SpeSpeI@',
                            viewTitle: 'Specimen Approval',
                            text: '@SpeApp@.',
                            nextItemButton: { buttontext: '@SpeNex@', show: true },
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Decision', type: 'radio', label: '@GenDec@', required: false, optionsName: 'specimenApproval', defaultValue: '522'},
                                                { id: 'ApprovalCommentL2Id', type: 'combobox', label: '@SpeReaE@', required: false, placeholder: '@SpeAddI@', optionsName: 'ApprovalComment'},
                                                { id: 'ReasonTwo', type: 'multiline', label: '@SpeReaC@', required: false, placeholder: '@SpeAddG@'},
                                                { id: 'ReportFilter', type: 'reportfilter' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
