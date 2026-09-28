using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the Edit User Details page.
/// This configuration defines the layout and properties of the page, including the page name, title,
/// descriptive text, required fields and their validation rule, as well as the structure of the form
/// consisting of columns, form groups, and individual fields.
/// </summary>
internal class EditUserDetailsPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON string that defines the configuration of the Edit User Details page.
    /// The configuration specifies:
    /// - The page's name and title.
    /// - An informational text.
    /// - Required fields with a rule indicating that all specified fields (UserName, Email, Roles) are mandatory.
    /// - A single column layout containing a form group with fields for UserName, FirstName, LastName, Email,
    ///   Roles (as a dropdown with multi-select capabilities), and a toggle for Enabled.
    /// </summary>
    /// <returns>A JSON string representing the page configuration.</returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'usereditdetails',
                        pageTitle: '@UseEdiA@',
                        text: '@UseEdi@.',
                        required: 'UserName,Email,Roles',
                        requiredRule: 'and',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'UserName', type: 'text', label: '@UseUseB@', Max: 30},
                                            { id: 'FirstName', type: 'singleline', label: '@UseFir@', Max: 30},
                                            { id: 'LastName', type: 'singleline', label: '@UseLas@', Max: 30},
                                            { id: 'Email', type: 'singleline', label: '@UseEma@', required: true, Max: 60},
                                            { id: 'Roles', type: 'dropdown', label: '@RolRolC@', required: true, multiSelect: true, placeholder: 'Select Role', optionsName: 'RoleList', dynamic: true },
                                            { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true}
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
