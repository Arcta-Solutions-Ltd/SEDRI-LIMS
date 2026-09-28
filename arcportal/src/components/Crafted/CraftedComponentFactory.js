import React from 'react';
import TestFieldSelector from './Alerts/TestFieldSelector/TestFieldSelector';
import InterfaceCriteriaSelector from './Instruments/InterfaceCriteriaSelector/InterfaceCriteriaSelector';
import LabelFields from './Config/Label/LabelFields';
import WorkflowEntrySelector from './Config/WorkflowEntrySelector/WorkflowEntrySelector';

const CraftedComponentFactory = (props) => {

    let componentToDisplay = (null);
    switch(props.config.Id.toLowerCase()) {
        case "testgrid":
            componentToDisplay = <TestFieldSelector config={props.config} data={props.data} changeHandler={props.changeHandler} ></TestFieldSelector>
            break;
        case "ruletestconditiongrid":
            componentToDisplay = <TestFieldSelector config={props.config} data={props.data} changeHandler={props.changeHandler} ></TestFieldSelector>
            break;
        case "interfacecriteria":
            componentToDisplay = <InterfaceCriteriaSelector config={props.config} data={props.data} changeHandler={props.changeHandler} fieldChangeHandler={props.fieldChangeHandler} ></InterfaceCriteriaSelector>
            break;
        case "labelfields":
            componentToDisplay = <LabelFields config={props.config} data={props.data} changeHandler={props.changeHandler} ></LabelFields>
            break;
        case "workflowrulegrid":
            componentToDisplay = <WorkflowEntrySelector config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} ></WorkflowEntrySelector>
            break;
        default:
            componentToDisplay = <div>No crafted component found</div>
            break;
    }

    return (
        <div>
            {componentToDisplay}
        </div>
    );
};
  
export default CraftedComponentFactory;