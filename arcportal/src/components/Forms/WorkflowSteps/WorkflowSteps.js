import React from 'react';
import './WorkflowSteps.css';
import { Icon } from '@fluentui/react/lib/Icon';

const WorkflowSteps = (props) => {

    const pagesToDisplay = props.config.Pages.filter(p => p.Visible);
    const workflowStepsContentCss = props.fullScreen ? "workflowsteps-content-wide" : "workflowsteps-content";
    const workflowStepsTitleCss = props.fullScreen ? "workflowsteps-title-wide" : "workflowsteps-title";


    return (
        <div>
            <div className={workflowStepsTitleCss}>
                {props.config.Title}
            </div>
            <div className={workflowStepsContentCss}>
                {pagesToDisplay.map((page, index) => {
                    const checked = props.completedPages.filter((name) => { return page.Name === name}).length !== 0;
                    let icon = null;
                    if (index < (pagesToDisplay.length - 1)) {
                        icon = (
                            <div className="workflowsteps-connector">
                                <Icon iconName='SortDown' />
                            </div>
                        );
                    }
                    const checkIcon = checked ? "CheckBoxCompositeReversed" : "CheckBox"
                    return (
                        <div className="workflowsteps-item" key={page.Name}>
                            <div className="workflowsteps-step">
                                <Icon iconName={checkIcon} className="testing"/>
                                <div>&nbsp;&nbsp;</div> 
                                <div>{page.PageTitle}</div>
                            </div>
                            {icon}
                        </div>
                    )
                })}
            </div>
        </div>
    )
};

export default WorkflowSteps;