using arc.app.Common;

namespace arc.app.Config.Pages.Coding;

/// <summary>
/// Configuration for the "Add Breakpoint Approval" page.
/// Contains the CodingStatus dropdown (defaulting to Approved) and hidden BreakpointId field.
/// </summary>
internal class AddBreakpointApprovalPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Breakpoint Approval" page.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'addbreakpointapprovalpage',
            pageTitle: '@BreAddApp@',
            text: '@BreAddAppB@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'BreakpointId', type: 'hidden' },
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
