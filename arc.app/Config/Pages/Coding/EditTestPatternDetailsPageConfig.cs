using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditTestPatternDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edittestpatterndetailspage',
                            pageTitle: '@TesEdi@',
                            text: '@TesEdiA@.',
                            wide: true,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticGrid', type: 'fieldgrid', draggable: true, gridfields: [
                                                        { id: 'AntibioticId', type: 'combobox', gridTitle: '@GenAnt@', optionsName: 'antibiotic', width: 'wide', required: true, placeholder: '@GenEntC@' },
                                                        { id: 'TestMethodId', type: 'dropdown', gridTitle: '@BreTes@', optionsName: 'testmethod', width: 'small', dynamic: true, required: true },
                                                        { id: 'GuidelinesId', type: 'dropdown', gridTitle: '@TesGui@', optionsName: 'guidelines', width: 'small', required: true },
                                                        { id: 'Dosage', type: 'number', gridTitle: '@GenDos@', Min: '0', Max: '999', MaxDPs: '0', width: 'narrow', required: true },
                                                        { id: 'CategoryId', type: 'combobox', gridTitle: '@GenCat@', optionsName: 'drugcategory', width: 'extrawide', required: false },
                                                        { id: 'PrintOnReport', type: 'toggle', gridTitle: '@GenPriE@', width: 'small', required: true }
                                                    ]
                                                }
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
