using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Alert Approval" page.
/// Contains the CodingStatus dropdown (defaulting to Approved) and hidden AlertId field.
/// </summary>
internal class AddAlertApprovalPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Alert Approval" page.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'addalertapprovalpage',
            pageTitle: '@BreAddApp@',
            text: '@BreAddAppB@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'AlertId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'dropdown', label: '@GenStaA@', required: true, optionsName: 'CodingApprovalStatus', defaultValue: '145' }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}
