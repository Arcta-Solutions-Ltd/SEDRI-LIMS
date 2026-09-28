using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class RestartSpecimenPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'restartspecimenpage',
                            pageTitle: '@SpeRes@',
                            text: '@SpeCha@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'StateId', type: 'dropdown', label: '@SpeSpeQ@', required: true, optionsName: 'shortstatelist' }
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
