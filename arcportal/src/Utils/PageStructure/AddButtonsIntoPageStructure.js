import React from 'react';
import { PrimaryButton } from '@fluentui/react';
import AreOtherPagesInvisible from './AreOtherPagesInvisible';
import IsThisTheLastPage from './IsThisTheLastPage';
import GetNextPage from './GetNextPage';
import GetPreviousPage from './GetPreviousPage';
import IsThisTheFirstPage from './IsThisTheFirstPage';

const AddButtonsIntoPageStructure = (
    pageStructure,
    handleNavButtonClick,
    handleFinishButtonClick
) => {
    let returnStructure = [...pageStructure];

    for (const page of returnStructure) {
        let buttons = [];
        if (!IsThisTheFirstPage(page, pageStructure)) {
            const previousPage = GetPreviousPage(page, pageStructure);
            buttons.push(
                <div className="app-button" key="Previous">
                    <PrimaryButton
                        text="Previous"
                        onClick={() => handleNavButtonClick(previousPage)}
                    />
                </div>
            );
        }
        if (!AreOtherPagesInvisible(page, pageStructure)) {
            const nextPage = GetNextPage(page, pageStructure);
            buttons.push(
                <div className="app-button" key="Next">
                    <PrimaryButton
                        text="Next"
                        onClick={() => handleNavButtonClick(nextPage)}
                    />
                </div>
            );
        }
        if (IsThisTheLastPage(page, pageStructure)) {
            buttons.push(
                <div className="app-button" key="Finish">
                    <PrimaryButton
                        text="Finish"
                        onClick={handleFinishButtonClick}
                    />
                </div>
            );
        }
        page.buttons = buttons;
    }

    return returnStructure;
};

export default AddButtonsIntoPageStructure;
