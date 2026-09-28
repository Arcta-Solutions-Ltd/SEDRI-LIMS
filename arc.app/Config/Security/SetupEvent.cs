namespace arc.app.Config.Security
{
    public class SetupEvent
    {
        public string GetConfig()
        {
            return @"{
                eventName: 'Setup', eventType: 'FormEntry' ,  
                form: { formType: 'WizardModal', columns: 2,
                    pages: [
                        { pageTitle: 'Language', 
                            formGroups: [{
                                column: 1,
                                fields: [{id: 'Language', type: 'dropdown', label: 'Language', multiselect: 'Yes', placeholder: 'Select Language',
                                    options: [
                                        { key: 'English', text: 'English' },
                                        { key: 'French', text: 'French' },
                                        { key: 'Spanish', text: 'Spanish' }
                                    ]
                                }]
                            }]
                        },
                        { pageTitle: 'Settings',
                            formGroups: [{
                                column: 1,
                                separator: 'User Setup',
                                fields:[{ id: 'MultipleUserFlag', type: 'toggle', label: 'Multiple Users'}]
                            },
                            {
                                column: 2,
                                separator: 'Laboratory Setup',
                                fields:[{ id: 'MultipleLaboratoryFlag',type: 'toggle', label: 'Multiple Laboratories'}]                           
                            }]
                        },
                        { pageTitle: 'Users',
                            formGroups: [{
                                column: 1,
                                separator: 'System Administrator',
                                fields:[{ id: 'SystemAdminUserName', type: 'singleline', label: 'Username'},
                                    { id: 'SystemAdminFirstName', type: 'singleline', label: 'First Name'},
                                    { id: 'SystemAdminLastName', type: 'singleline', label: 'Last Name'},
                                    { id: 'SystemAdminPassword', type: 'password', label: 'Password'},
                                    { id: 'SystemAdminConfirmPassword', type: 'password', label: 'Confirm Password'}]
                            },
                            {
                                column: 2,
                                separator: 'Organisation Administrator',
                                fields:[{ id: 'OrgAdminUserName', type: 'singleline', label: 'Username'},
                                    { id: 'OrgAdminFirstName', type: 'singleline', label: 'First Name'},
                                    { id: 'OrgAdminLastName', type: 'singleline', label: 'Last Name'},
                                    { id: 'OrgAdminPassword', type: 'password', label: 'Password'},
                                    { id: 'OrgAdminConfirmPassword', type: 'password', label: 'Confirm Password'}]                          
                            }]
                        }]
                },
                validationRules: [
                    { field: 'Language', rule: 'required', message: 'You must enter a language'},
                    { field: 'MultipleUserFlag', rule: 'required', message: 'You must specify single or mulitple users'},
                    { field: 'MultipleLaboratoryFlag', rule: 'required', message: 'You must specify single or mulitple laboratories'},
                    { field: 'SystemAdminUserName', rule: 'required', Conditions:[{ field: 'MultipleUserFlag', comparison: '=', value: 'Yes'}], message: 'System Admin Username must be entered' },
                    { field: 'SystemAdminPassword', rule: 'required', Conditions:[{ field: 'MultipleUserFlag', comparison: '=', value: 'Yes'}], message: 'System Admin Password must be entered'},  
                    { field: 'SystemAdminConfirmPassword', rule: 'required', Conditions:[{ field: 'MultipleUserFlag', comparison: '=', value: 'Yes'}], message: 'System Admin confirmation password must be entered'},
                    { field: 'OrgAdminUserName', rule: 'required', message: 'Organisation Admin Username must be entered'},  
                    { field: 'OrgAdminPassword', rule: 'required', message: 'Organisation Admin Password must be entered'},
                    { field: 'OrgAdminConfirmPassword', rule: 'required', message: 'Organisation Admin confirmation password must be entered'},
                    { rule: 'valuecompare', firstValue: '#SystemAdminPassword', comparison: '=', secondValue: '#SystemAdminConfirmPassword', message: 'System admin passwords must be the same'},
                    { rule: 'valuecompare', firstValue: '#OrgAdminPassword', comparison: '=', secondValue: '#OrgAdminConfirmPassword', message: 'Organisation admin passwords must be the same'}
                ]
            }";
        }
    }
}

