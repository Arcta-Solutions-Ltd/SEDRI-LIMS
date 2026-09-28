import React, {useEffect} from 'react';
import { TooltipHost } from '@fluentui/react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { Icon } from '@fluentui/react/lib/Icon';
import { IconButton } from '@fluentui/react';

const categoryColours = [ 'black', 'red', 'blue', 'green' ];

const ASTRowContents = (props) => {

    const index = props.index;

    const newEntry = [
        { Id: 'Drugcategory', Type: '', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.DrugCategory },
        { Id: 'Antibiotic', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.Antibiotic},
        { Id: 'Dosage', Type: 'number', Label: '', value: '', Min: '0', Max: '999', MaxDPs: '0', TabIndex: -1, Key: 'dosage', value: props.value.Dosage },
        { Id: 'Guidelines', Type: 'dropdown', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.Guidelines },
        { Id: 'ZoneDiameter', Type: 'number', Label: '', value: '', Min: '6', Max: '50', MaxDPs: '0', Key: 'zonediameter', value: props.value.ZoneDiameter},
        { Id: 'TestResult', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: props.value.TestResult },
        { Id: 'IncludeOnReport', Type: 'toggle', Label: '', value: 'No', TabIndex: -1, value: props.value.IncludeOnReport},
        { Id: 'Mic', Type: 'micdosage', Label: '', value: '', Min: '0', Max: '9999', MaxDPs: '3' }
    ];

    const categories = ['drugcategory', 'antibiotic', 'guidelines', 'testresult'];
    categories.forEach(category => {
        const index = props.lists.findIndex(l => l.Name === category);
        if (index !== -1) {
            const mappedOptions = props.lists[index].Options.map(option => ({ key: option.Key, text: option.Text }));
            newEntry.find(i => i.Id.toLowerCase() == category).Options.push(...mappedOptions);
        }
    });

    const dosageClass = props.type === "disk" && !props.ExpertRuleLine ? "astform-manual-dosage" : "astform-manual-dosage hidden";
    const zoneDiameterClass = props.type === "disk" && !props.ExpertRuleLine ? "astform-manual-zone-diameter" : "app-invisible";
    const micClass = props.type === "mic" && !props.ExpertRuleLine ? "astform-test-pattern-row-item" : "app-invisible";

    const changeHandler = (id, value) => {
        const row = props.index;
        const specialRow = props.specialIndex;
        const type = props.type;
        const newValue = { ...props.value, [id]: value };
        props.changeHandler(type, row, specialRow, newValue, id);
    }

    return (       
        <div key={index} className='astform-test-pattern-row'>
            <div className='astform-antibiotic-level'>
                <TooltipHost
                    content={newEntry[0].Options[newEntry[0].value > 1219 ? newEntry[0].value - 1220 : 0]?.text}
                    id={100}
                >
                    <div className={ 'astform-antibiotic-level-item-' + ((newEntry[0].value > 1219) ? categoryColours[newEntry[0].value - 1220] : 'white') }>
                        <div>
                            { newEntry[0].value > 1219 ? newEntry[0].value - 1219 : "" }
                        </div>
                    </div>
                </TooltipHost>
            </div>

            <div className={`astform-manual-drug ${props.isHighlighted ? 'astform-row-highlight' : ''}`}>
                <SingleLineField
                    key="antibiotic"
                    config={newEntry[1]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className={dosageClass}>
                <SingleLineField
                    key="dosage" 
                    config={newEntry[2]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className='astform-manual-guidelines'>
                <SingleLineField
                    key="guidelines"
                    config={newEntry[3]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className={zoneDiameterClass}>
                <SingleLineField
                    key="zonediameter"
                    config={newEntry[4]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className={micClass}>
                <SingleLineField
                    key="mic"
                    config={newEntry[7]}
                    changeHandler={changeHandler}
                >
                </SingleLineField>
            </div>
            <div className='astform-manual-susceptibility'>
                <div className={props.isHighlighted ? 'astform-row-highlight' : ''}>
                <SingleLineField
                    key="susceptibility"
                    config={newEntry[5]}
                    changeHandler={changeHandler}
                    >
                </SingleLineField>
                </div>
            </div>
            <div className='astform-manual-include-in-report'>
                <div className='astform-toggle'>
                    <SingleLineField
                        key="includeinreport"
                        config={newEntry[6]}
                        changeHandler={changeHandler}
                    >
                    </SingleLineField>
                </div>
            </div>
            <div>
                <IconButton
                    tabIndex={-1}
                    iconProps={{iconName: 'Cancel'}}
                    onClick={() => {props.deleteHandler(index, props.specialIndex, props.type)}}
                />
            </div>
        </div>
    );
};
  
export default ASTRowContents;