using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditLabelDimensionsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'editlabeldimensionspage',
                            pageTitle: '@CfgLabI@',
                            text: '@CfgLabJ@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'LeftMargin', type: 'singleline', label: '@CfgLabK@', required: true },
                                                { id: 'TopMargin', type: 'singleline', label: '@CfgLabL@', required: true },
                                                { id: 'ItemWidth', type: 'singleline', label: '@CfgLabM@', required: true },
                                                { id: 'ItemHeight', type: 'singleline', label: '@CfgLabN@', required: true },
                                                { id: 'BottomPadding', type: 'singleline', label: '@CfgLabO@', required: true }
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

