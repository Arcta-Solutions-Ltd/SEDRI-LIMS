import React from 'react';
import { PrimaryButton, CompoundButton, Icon } from '@fluentui/react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import './CultureOrganismSelection.css';


const CultureOrganismSelection = (props) => {

    const searchIcon = <Icon iconName="Search" />;

    const fields = props.config.Columns[0].FormGroups[0].Fields;
    const organismId = fields.filter(f => f.Id == "organismid")[0].value;

    const changeHandler = (id, value) => {
        let changes = [];
        
        if (id == 'OrganismId') {
            changes = [{ 
                key: "OrganismId", 
                value: { Key: "OrganismId", value: value}
            }];            
        }
        props.changeHandler("multiplechanges", changes, { rootCopy: true});
    }

    const organismSearchClickHandler = () => {
        props.rightButtonClick([], "custom");
    };

    const prevClickHandler = () => {
        props.leftButtonClick([]);
    };

    const nextClickHandler = () => {
        props.rightButtonClick([]);
    };

    const localKeyDown = function(e) {
        props.onKeyDown(e, true);
    };

    const organismListConfig = { Id: "OrganismId", Type: 'organismlist', value: organismId, includeOrText: true };

    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            <div className="app-formcolumn">

                <SingleLineField key="OrganismId" config={organismListConfig} changeHandler={changeHandler} language={props.language} onKeyDown={localKeyDown}></SingleLineField>

                <div className="cultureorganismselection-buttons">
                    <CompoundButton primary onClick={() => organismSearchClickHandler()} >
                        <div className="printpublish-button">
                            {searchIcon}
                            <br />
                            {TranslateTag("@SpeFul@", props.language)}
                        </div>
                    </CompoundButton>
                </div>

            </div>

            <div className='cultureorganismselection-navpushdown'>

            </div>

            <div className='cultureorganismselection-navigation'>
                <div className="cultureorganismselection-labels">
                    <div>
                        {'<- ' + TranslateTag("@SpeGroB@", props.language)}
                    </div>
                    <div>
                        {TranslateTag("@SpeAddF@", props.language) + ' ->'}
                    </div>
                </div>
                <div className="cultureorganismselection-navbuttons">
                    <div className="app-button">
                        <PrimaryButton text={TranslateTag("@GenBac@", props.language)} onClick={() => prevClickHandler()}/>
                    </div>
                    <div className="app-button">
                        <PrimaryButton text={TranslateTag("@GenNex@", props.language)} onClick={() => nextClickHandler(false)}/>
                    </div>
                </div>
            </div>
        </div>
    )
};

export default CultureOrganismSelection;
