using arc.app.Common;

namespace arc.app.Config.Pages.Request;

internal class DeleteRequestPageConfig : IDefinition
{
    public string Get()
    {
        return @"{
                            name: 'deleterequestpage',
                            pageTitle: '@GenDel@',
                            text: '@NeoReqDelA@',
                            required: 'RequestId',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'RequestId', type: 'text', label: '@NeoReqRef@', placeholder: '@GenLisA@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";
    }
}
