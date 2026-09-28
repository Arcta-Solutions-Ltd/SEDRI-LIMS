using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Defines the configuration for the Specimen Approval Page.
    /// </summary>
    internal class SpecimenApprovalPageConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON configuration for the specimen approval page.
        /// </summary>
        /// <returns>A string containing the JSON configuration.</returns>
        public string Get()
        {
            var page = @"{
                        name: 'specimenapproval',
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
                                            { id: 'ApprovalCommentL1Id', type: 'combobox', label: '@SpeReaD@', required: false, placeholder: '@SpeAddI@', optionsName: 'ApprovalComment'},
                                            { id: 'ReasonOne', type: 'multiline', label: '@SpeReaB@', required: false, placeholder: '@SpeAddG@'},
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
