import React, {useEffect} from 'react';
import { TooltipHost } from '@fluentui/react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { Icon } from '@fluentui/react/lib/Icon';
import { IconButton } from '@fluentui/react';

const ASTRuleRowContents = (props) => {

    const index = props.index;

    const newEntry = [
        { Id: 'Antibiotic', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.Antibiotic, Disabled: true },
        { Id: 'Guidelines', Type: 'dropdown', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.Guidelines, Disabled: true },
        { Id: 'TestResult', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.TestResult, Disabled: true },
        { Id: 'IncludeOnReport', Type: 'toggle', Label: '', value: 'No', TabIndex: -1, value: props.value.IncludeOnReport, Disabled: true},
        { Id: 'ApplyRule', Type: 'toggle', Label: '', value: 'No', TabIndex: -1, value: props.value.ApplyRule || 'No', Disabled: false}
    ];

    const categories = ['antibiotic', 'guidelines', 'testresult'];
    categories.forEach(category => {
        const index = props.lists.findIndex(l => l.Name === category);
        if (index !== -1) {
            const mappedOptions = props.lists[index].Options.map(option => ({ key: option.Key, text: option.Text }));
            newEntry.find(i => i.Id.toLowerCase() == category).Options.push(...mappedOptions);
        }
    });

    const changeHandler = (id, value) => {
        // Handle ApplyRule toggle separately for embedded expert rule rows
        if (id === 'ApplyRule' && props.index !== undefined && props.applyRuleHandler) {
            props.applyRuleHandler(props.type, props.index, props.specialIndex, value, props.value);
            return;
        }
        
        const row = props.index;
        const specialRow = props.specialIndex;
        const type = props.type;
        const newValue = { ...props.value, [id]: value };
        props.changeHandler(type, row, specialRow, newValue, id);
    }

    return (       
        <div key={index} className='astform-test-pattern-row'>
            <div className='astform-expert-rule-level'>
                {!props.hideIcon && (
                    <TooltipHost               
                        content={props.value.ExpertRuleLine ? 
                        (
                        <span>
                            <strong>{props.value.ExpertRuleName + ':'}</strong> {props.value.ExpertRuleText}
                        </span>
                        )
                        : ''}
                        id={100}
                    >
                         <div
                            onMouseEnter={ props.onIconHover}          
                            onMouseLeave={props.onIconLeave}
                        >

                        <div className={ (props.value.ExpertRuleLine ? 'astform-expert-rule-item' : 'white') }>
                            <div>
                                {props.value.ExpertRuleLine ? <Icon iconName="DecisionSolid" /> : "" }
                            </div>
                        </div>  
                        </div>          
                    </TooltipHost>
                )}
            </div>
                        
            <div className='astform-manual-drug'>
                <SingleLineField
                    key="antibiotic"
                    config={newEntry[0]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className='astform-manual-guidelines'>
                <SingleLineField
                    key="guidelines"
                    config={newEntry[1]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className='astform-manual-susceptibility'>
                <SingleLineField
                    key="susceptibility"
                    config={newEntry[2]}
                    changeHandler={changeHandler}
                    >
                </SingleLineField>
            </div>
            <div className='astform-manual-include-in-report'>
                <div className='astform-toggle'>
                    <SingleLineField
                        key="includeinreport"
                        config={newEntry[3]}
                        changeHandler={changeHandler}
                    >
                    </SingleLineField>
                </div>
            </div>
            {props.index !== undefined && !props.hideApplyToggle && (
                <div className='astform-manual-apply-rule'>
                    <div className='astform-toggle'>
                        <SingleLineField
                            key="applyrule"
                            config={newEntry[4]}
                            changeHandler={changeHandler}
                        >
                        </SingleLineField>
                    </div>
                </div>
            )}
        </div>
    );
};
  
export default ASTRuleRowContents;