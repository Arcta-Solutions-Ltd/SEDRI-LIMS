using arc.app.Common;

namespace arc.app.Config.Pages;
internal class DeleteImagePageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                name: 'deleteimagepage',
                pageTitle: '@ImgDel@',
                text: '@ImgDelB@.',
                columns: [
                    {
                        key: 'col1',
                        fieldWidth: 'wide',
                        itemWidth: 'wide',
                        formGroups: [
                            {
                                key: 'fg1',
                                fields: [
                                    { id: 'name', type: 'text', label: '@GenNam@' },
                                    { id: 'description', type: 'text', label: '@GenDes@' },
                                ]
                            }
                        ]
                    }
                ],
                nextButton: { show: true, buttonText: '@GenDelC@' }
            }";

        return page;
    }
}
