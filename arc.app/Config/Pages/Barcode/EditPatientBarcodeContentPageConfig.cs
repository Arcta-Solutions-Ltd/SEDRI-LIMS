using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditPatientBarcodeContentPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'editpatientbarcodecontentpage',
                            pageTitle: '@CfgLabP@',
                            text: '@CfgLabQ@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'title', type: 'text', label: '@CfgLabR@' },
                                                { id: 'Linear', type: 'toggle', label: '@CfgLabS@' },
                                                { id: 'QR', type: 'toggle', label: '@CfgLabT@' },
                                                { id: 'DisplayCode', type: 'toggle', label: '@CfgLabU@' },
                                                { id: 'LabelFields', type: 'crafted', label: '@CfgLabV@', parameter1: 'patientlabelavailablefields', parameter2: 'createspecimenreceivedform' },
                                                { id: 'Border', type: 'toggle', label: '@CfgLabW@' },
                                                { id: 'ItemsPerRow', type: 'singleline', label: '@CfgLabX@', required: true },
                                                { id: 'NumberOfRows', type: 'singleline', label: '@CfgLabY@', required: true }
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

