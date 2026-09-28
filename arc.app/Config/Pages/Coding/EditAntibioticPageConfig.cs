using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class EditAntibioticPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        name: 'editantibioticpage',
                        pageTitle: '@AntManEdi@',
                        text: '@AntManEditSub@.',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'AntibioticName', type: 'singleline', label: '@GenNam@', required: true, Max: 50},
                                            { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, Max: 10},
                                            { id: 'Atc', type: 'singleline', label: '@AntManAtc@', Max: 12},
                                            { id: 'Cid', type: 'singleline', label: '@AntManCid@', Max: 12},
                                            { id: 'Loinc', type: 'singleline', label: '@AntManLoi@', Max: 200},
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";
        }
    }
}
