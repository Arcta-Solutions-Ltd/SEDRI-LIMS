using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditLocationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editlocationpage',
                            pageTitle: '@LocEdi@',
                            text: '@LocEdiA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenLoc@', required: true, placeholder: '@LocEntA@', Max: 50 },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', placeholder: '@LocEntB@', Max: 15 },
                                                { id: 'ParentLocationId', type: 'picker', label: '@LocPar@', placeholder: '@LocSelA@', optionsName: 'LocationList', dynamic: true },
                                                { id: 'Latitude', type: 'number', label: '@GenLat@', placeholder: '@LocEntC@', Min: '-90', Max: '90', MaxDPs: '5' },
                                                { id: 'Longitude', type: 'number', label: '@GenLon@', placeholder: '@LocEntD@', Min: '-180', Max: '180', MaxDPs: '5' },
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
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
