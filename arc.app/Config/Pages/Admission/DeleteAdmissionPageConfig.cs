using arc.app.Common;

namespace arc.app.Config.Pages.Admission;

internal class DeleteAdmissionPageConfig : IDefinition
{
    public string Get()
    {
        return @"{
                            name: 'deleteadmissionpage',
                            pageTitle: '@GenDel@',
                            text: '@NeoAdmDelA@',
                            required: 'DateOfAdmission',
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
                                                { id: 'DateOfAdmission', type: 'text', label: '@NeoAdmDat@', placeholder: '@GenLisA@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";
    }
}
