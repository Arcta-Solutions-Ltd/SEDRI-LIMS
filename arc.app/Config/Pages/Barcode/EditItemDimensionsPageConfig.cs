using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditItemDimensionsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'edititemdimensionspage',
                            pageTitle: '@CfgLab@',
                            text: '@CfgLabA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'LinearHeight', type: 'singleline', label: '@CfgLabB@', required: true },
                                                { id: 'QRSize', type: 'singleline', label: '@CfgLabC@', required: true },
                                                { id: 'SideBySide', type: 'toggle', label: '@CfgLabD@' },
                                                { id: 'FieldFontSize', type: 'singleline', label: '@CfgLabE@', required: true },
                                                { id: 'BarcodePadding', type: 'singleline', label: '@CfgLabF@', required: true },
                                                { id: 'FieldNameWidth', type: 'singleline', label: '@CfgLabG@', required: true },
                                                { id: 'FieldTotalWidth', type: 'singleline', label: '@CfgLabH@', required: true }
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

