using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DipstickTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'dipsticktestpage',
                            pageTitle: '@TesDip@',
                            text: '@TesEntO@.',
                            configureActions: 'add,edit',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'phId', type: 'combobox', label: '@TesPh@', optionsName: 'ph'},
                                                { id: 'specificGravityId', type: 'combobox', label: '@TesSpe@', optionsName: 'SpecificGravity'},
                                                { id: 'proteinId', type: 'combobox', label: '@TesProA@', optionsName: 'ProteinDipstick'},
                                                { id: 'ketonesId', type: 'combobox', label: '@TesKet@', optionsName: 'KetonesDipstick'},
                                                { id: 'glucoseId', type: 'combobox', label: '@TesGluA@', optionsName: 'GlucoseDipstick'},
                                                { id: 'bloodId', type: 'combobox', label: '@GenBlo@', optionsName: 'BloodDipstick'},
                                                { id: 'leucocytesId', type: 'combobox', label: '@TesLeu@', optionsName: 'LeucocytesDipstick'},
                                                { id: 'nitritesId', type: 'combobox', label: '@TesNit@', optionsName: 'NitritesDipstick'},
                                                { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
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

