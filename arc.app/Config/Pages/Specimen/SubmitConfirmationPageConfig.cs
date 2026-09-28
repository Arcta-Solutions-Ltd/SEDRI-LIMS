using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SubmitConfirmationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'submitconfirmationpage',
                            pageTitle: '@SpeSubA@',
                            text: '@SpeYou@.',
                            configureActions: 'add,edit',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
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

//{ id: 'GrowthId', type: 'radio', label: '@SpeGroA@', required: false, optionsName: 'submitSpecimen' }
