using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration class for the add instrument profile page (step 1). Labels: Profile Name, then Instrument (machine list combobox).
    /// </summary>
    internal class AddInstrumentProfilePageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the add instrument profile page.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'addinstrumentprofilepage',
                            pageTitle: '@InsAdd@',
                            text: '@InsAddB@.',
                            required: 'InstrumentName,InstrumentMachineId',
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
                                                { id: 'InstrumentName', type: 'singleline', label: '@InsProNam@', required: true, placeholder: '@InsEnt@'},
                                                { id: 'InstrumentMachineId', type: 'combobox', label: '@GenIns@', required: true, placeholder: '@GenSelK@', optionsName: 'InstrumentMachine', multiSelect: false }
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'DirectTestId', rule: 'isempty'}],
                                            fields: [
                                                { id: 'CultureTypeId', type: 'combobox', label: '@CulTyp@', required: false, multiSelect: false, placeholder: '@SpeSelK@', optionsName: 'CultureType' }
                                            ]
                                        },
                                        { 
                                            key: 'fg3',
                                            fields: [
                                                { id: 'SpecimenTypeId', type: 'combobox', label: '@SpeSpeB@', required: false, multiselect: true, placeholder: '@SpeSelC@', optionsName: 'SpecimenType' }
                                            ]
                                        },
                                        { 
                                            key: 'fg4',
                                            rules:[{ effect: 'visible', field: 'CultureTypeId', rule: 'isempty'}, { effect: 'visible', field: 'CultureTestId', rule: 'isempty'}, { effect: 'visible', field: 'ConfigListId', rule: 'isempty'}],
                                            fields: [
                                                { id: 'DirectTestId', type: 'combobox', label: '@ConDirA@', required: false, multiselect: true, placeholder: '@CulSelA@', optionsName: 'DirectTestConfigList' }
                                            ]
                                        },
                                        { 
                                            key: 'fg5',
                                            rules:[{ effect: 'visible', field: 'DirectTestId', rule: 'isempty'}, { effect: 'visible', field: 'ConfigListId', rule: 'isempty'}],
                                            fields: [
                                                { id: 'CultureTestId', type: 'combobox',  label: '@SpeCulC@', placeholder: '@GenTesD@', multiSelect: true, width: 200, optionsName: 'culturetestconfiglist'}
                                            ]
                                        },
                                        { 
                                            key: 'fg6',
                                            rules:[{ effect: 'visible', field: 'DirectTestId', rule: 'isempty'}, { effect: 'visible', field: 'CultureTestId', rule: 'isempty'}, { effect: 'visible', field: 'CultureTypeId', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'CodingListId', type: 'dropdown', label: '@GenCodB@', placeholder: '@GenCodB@', optionsName: 'Coding', multiSelect: false, removeFixed: true, dynamic: true }
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
