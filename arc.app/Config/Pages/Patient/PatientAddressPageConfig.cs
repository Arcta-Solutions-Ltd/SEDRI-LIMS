using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Represents the configuration for the patient address page. Shared by the add and edit patient
    /// forms, so a field added here appears on both.
    /// </summary>
    internal class PatientAddressPageConfig : IDefinition
    {
        /// <summary>
        /// Constructs and returns the JSON configuration string for the patient address page.
        /// </summary>
        /// <returns>A JSON string defining the patient address page configuration.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'patientaddresspage',
                            pageTitle: '@PatAddB@',
                            configureActions: 'add,edit,delete',
                            text: '@PatEdiB@.',
                            pageGroup: 'patient',
                            tablename: 'patient',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AddressLine1', type: 'singleline', label: '@PatAddD@', Max: 50 },
                                                { id: 'AddressLine2', type: 'singleline', label: '@PatAddE@', Max: 50 },
                                                { id: 'LocationId', type: 'hierarchicalpicker', placeholder: '@LocSel@', label: '@GenLoc@', optionsName: 'LocationList', Configurable: 'No', allowAdd: true },
                                                { id: 'ZipCode', type: 'singleline', label: '@PatZip@', Max: 50 },
                                                { id: 'TelephoneNumber', type: 'singleline', label: '@PatTel@', placeholder: '@PatEntD@', Max: 30 }
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

//Need to keep this to show how to configure a location field
//{
//id: 'LocationId', type: 'hierarchy', placeholder: '@LocSel@', label: '@GenLoc@', optionsName: 'LocationList', displayCode: true, codeLabel: '@LocLocB@', codePlaceholder: '@LocEntB@',
//                                                    levels:[{ label: '@PatPro@', placeholder: '@PatSelB@'}, { label: '@PatDis@', placeholder: '@PatSelC@'}, { label: '@PatSub@', placeholder: '@PatSelD@'}]
//                                                },
