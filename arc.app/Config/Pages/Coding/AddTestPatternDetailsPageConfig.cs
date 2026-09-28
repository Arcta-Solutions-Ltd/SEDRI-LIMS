using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddTestPatternDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addtestpatterndetailspage',
                            pageTitle: '@TesAddE@',
                            text: '@TesAddF@.',
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
                                                { id: 'antibioticgrid', type: 'fieldgrid', gridfields: [
                                                        { id: 'antibioticid', type: 'combobox', gridTitle: '@GenAnt@', optionsName: 'antibiotic', width: 'wide', required: true, placeholder: '@GenEntC@' },
                                                        { id: 'testmethodid', type: 'dropdown', gridTitle: '@BreTes@', optionsName: 'testmethod', width: 'small', dynamic: true, required: true },
                                                        { id: 'guidelinesid', type: 'dropdown', gridTitle: '@TesGui@', optionsName: 'guidelines', width: 'small', required: true },
                                                        { id: 'dosage', type: 'number', gridTitle: '@GenDos@', Min: '0', Max: '999', MaxDPs: '0', width: 'narrow', required: true },
                                                        { id: 'categoryid', type: 'combobox', gridTitle: '@GenCat@', optionsName: 'drugcategory', width: 'extrawide', required: false },
                                                        { id: 'printonreport', type: 'toggle', gridTitle: '@GenPriE@', width: 'small', required: true }
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


//columns: [
//{ 
//    key: 'col1',
//    fieldWidth: 'wide',
//    itemWidth: 'wide',
//    formGroups: [
//        { 
//            key: 'fg1',
//            fields: [
//                { id: 'antibioticgrid', type: 'fieldgrid', label: '@GenAnt@', gridfields: [
//                        { id: 'AntibioticId', type: 'filteredcombo', optionsName: 'antibiotic', required: true },
//                        { id: 'TestMethodId', type: 'dropdown', label: '@BreTes@', placeholder: '@BreTes@', optionsName: 'testmethod', width: 'narrow', dynamic: true, required: true },
//                        { id: 'Dosage', type: 'singleline', mask: '999', width: 'narrow', required: true }
//                    ]
//                }
//            ]
//        }
//    ]
//}
//]
