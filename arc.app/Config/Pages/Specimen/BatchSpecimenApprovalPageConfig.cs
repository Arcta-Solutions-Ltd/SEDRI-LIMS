using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class BatchSpecimenApprovalPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'batchspecimenapprovalpage',
                            pageTitle: '@SpeSpeI@',
                            viewTitle: 'Specimen Approval',
                            text: '@SpeApp@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Decision', type: 'radio', label: '@GenDec@', required: false, optionsName: 'specimenApproval', optionsName: 'specimenApproval', defaultValue: '522'},
                                                { id: 'ApprovalCommentL1Id', type: 'combobox', label: '@SpeReaD@', required: false, placeholder: '@SpeAddI@', optionsName: 'ApprovalComment'},
                                                { id: 'ReasonOne', type: 'multiline', label: '@SpeReaB@', required: false, placeholder: '@SpeAddG@'}
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
