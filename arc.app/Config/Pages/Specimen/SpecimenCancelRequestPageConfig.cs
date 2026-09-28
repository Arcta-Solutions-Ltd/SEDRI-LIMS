using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenCancelRequestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'specimencancelrequest',
                            pageTitle: '@SpeCan@',
                            text: '@SpeCanB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AccessionNumber', type: 'text', label: '@SpeAcc@'},
                                                { id: 'CancellationReason', type: 'multiline', label: '@SpeCanC@', required: false, placeholder: '@SpeEntJ@' }
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
