import React from 'react';
import WizardModal from '../../../Forms/WizardModal/WizardModal';

import './InitialSetUp.css';

const InitialSetUp = (props) => {

    const wizardConfig = {
        endpoint: "setup/setup",
        columns: 2,
        pages: [
            { Name: "Language", pageTitle: "Language", Visible: true,
                Columns: [{
                    key: "col1",
                    FormGroups: [{
                        key: 'fg1',
                        Fields: [{Id: 'Language', Type: "dropdown", Label: "Language", Required: true, Multiselect: false, 
                            Placeholder: "Select Language",
                            Options: [
                                { key: 'English', text: 'English' },
                                { key: 'French', text: 'French' },
                                { key: 'Spanish', text: 'Spanish' }
                            ]
                        }]
                    }]
                }]
            },
            { Name: "Settings", pageTitle: "Settings", Visible: true,
                Columns: [{
                    key: "col1",
                    FormGroups: [{
                        key: 'fg2',
                        column: 1,
                        Separator: "User Setup",
                        Fields: [{Id: 'MultipleUserFlag', Type: "toggle", Label: "Multiple Users", Required: true, value: "Yes"}]
                    },
                    {
                        key: 'fg3',
                        Separator: "Laboratory Setup",
                        Fields: [{Id: 'MultipleLaboratoryFlag',Type: "toggle", Label: "Multiple Laboratories", Required: true, value: "No"}]                           
                    }]
                }]
            },
            { Name: "Users", pageTitle: "Users", Visible: true,
                Columns: [{
                    key: "col1",
                    FormGroups: [{
                        key: 'fg4',
                        Separator: "System Administrator",
                        Fields: [{Id: 'SystemAdminUserName', Type: "singleline", Label: "Username", Required: true},
                            {Id: 'SystemAdminFirstName', Type: 'singleline', Label: 'First Name'},
                            {Id: 'SystemAdminLastName', Type: 'singleline', Label: 'Last Name'},
                            {Id: 'SystemAdminPassword', Type: "password", Label: "Password", Required: true},
                            {Id: 'SystemAdminConfirmPassword', Type: "password", Label: "Confirm Password", Required: true}]
                    }]                    
                },
                {
                    key: "col2",
                    FormGroups: [{
                            key: 'fg5',
                            Separator: "Organisation Administrator",
                            Fields: [{Id: 'OrgAdminUserName', Type: "singleline", Label: "Username", Required: true},
                                {Id: 'OrgAdminFirstName', Type: 'singleline', Label: 'First Name'},
                                {Id: 'OrgAdminLastName', Type: 'singleline', Label: 'Last Name'},
                                {Id: 'OrgAdminPassword', Type: "password", Label: "Password", Required: true},
                                {Id: 'OrgAdminConfirmPassword', Type: "password", Label: "Confirm Password", Required: true}]                          
                    }]
                }]
            }
        ]
    }
    
    return (
        <WizardModal config={wizardConfig} isFormOpen={true} closeWindowHandler={props.closeWindowHandler}></WizardModal>
    )
};
  
export default InitialSetUp;