using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class RejectSpecimenPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'rejectspecimen',
                            pageTitle: '@SpeRej@',
                            text: '@SpeInd@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ReceivedConditionId', type: 'dropdown', label: '@SpeSpeE@', required: true, multiselect: false, placeholder: '@SpeSelA@', optionsName: 'ReceivedCondition' },
                                                { id: 'SelectReasonId', type: 'combobox', label: '@SpeReaG@', required: true, multiselect: false, placeholder: '@SpeEntC@', optionsName: 'RejectionReasonList' },
                                                { id: 'RejectionReason', type: 'multiline', label: '@SpeReaH@', required: false, placeholder: '@SpeEntC@' }
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
