using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class PatientCommentPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'patientcommentpage',
                            pageTitle: '@PatPatE@',
                            text: '@PatAddC@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { key: 'fg1',
                                            fields: [
                                                { id: 'Comment', type: 'multiline', label: '@GenComT@', required: true, placeholder: '@GenEntA@'}
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
