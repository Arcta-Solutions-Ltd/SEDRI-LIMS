import React, { useState } from 'react';
import ListView from '../../General/ListView/ListView';
import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { connect } from 'react-redux';

const IqcResultsGrid = (props) => {
    const [selectedIndex, setSelectedIndex] = useState({});

    const groupBy = (arr, key) => {
        return Object.entries(arr)
            .map((obj) => obj[1])
            .reduce((rv, x) => {
                (rv[x[key]] = rv[x[key]] || []).push(x);
                return rv;
            }, {});
    };

    const columns = [
        {
            Key: 'alert',
            Name: '',
            FieldName: '',
            MinWidth: 20,
            MaxWidth: 20,
            IsResizable: false,
            IsCollapsible: false,
        },
        {
            Key: 'column1',
            Name: 'Antibiotic',
            FieldName: 'AntibioticName',
            IsCollapsible: true,
        },
        {
            Key: 'menu',
            Name: '',
            FieldName: '',
            MinWidth: 120,
            MaxWidth: 120,
            IsResizable: true,
            IsCollapsible: false,
        },
        {
            Key: 'column2',
            Name: 'Result',
            FieldName: 'Value',
            MinWidth: 120,
            MaxWidth: 120,
            IsResizable: true,
            IsCollapsible: false,
        },
    ];
    
    const hasPermission = (formName) =>
        props.forms.filter((f) => f.Name === formName).length > 0;
    const notInCompletedState = props.stateid.toString() !== "1069";

    const menuButtons = [];
    let PrimaryAction = 1;
    if (hasPermission('editiqcresultform') && notInCompletedState) {
        menuButtons.push({
            Icon: 'Edit',
            Key: 'editiqcresultuievent',
            PrimaryAction,
            Text: TranslateTag('@QuaEdiTesRes@', props.language),
            OnFinish: 'refresh',
            UIEvent: 'editiqcresultuievent',
            workflow: true,
        });
        PrimaryAction++;
    }
    if (hasPermission('deleteiqcresultform') && notInCompletedState) {
        menuButtons.push({
            Icon: 'Delete',
            Key: 'deleteiqcresultuievent',
            PrimaryAction,
            Text: TranslateTag('@QuaDelIqcTesRes@', props.language),
            OnFinish: 'refresh',
            UIEvent: 'deleteiqcresultuievent',
            workflow: true,
        });
    }

    const buttonClickHandler = (button, item) => {
        const id = item.id === undefined ? item.Id : item.id;
        props.onButtonClick({
            button: button,
            id,
            stateid: item.stateid,
        });
    };

    const menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);

    const selectionChangeHandler = (selectionState) => {
        setSelectedIndex(selectionState.getSelectedIndices());
    };

    const itemSelected = (item, selected) => {
        if (selected) {
            var arrayOfSelectedItems = [];
            arrayOfSelectedItems[0] = item;
        }
    };

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    };

    if (props.data !== undefined && props.data.length > 1) {
        const groupedOrganisms = groupBy(props.data[1], 'OrganismName');
        return (
            <div className="directtestsgrid-content">
                {Object.entries(groupedOrganisms).map((organism) => {
                    return (
                        <div key={organism[0]}>
                            <div className="managerecord-sectiontitle">
                                {organism[0]}
                            </div>
                            <div className="managerecord-section-table">
                                <ListView
                                    type={'grid'}
                                    parentId={props.id}
                                    refresh={props.refresh}
                                    columns={columns}
                                    listData={organism[1]}
                                    menuItems={menuItems}
                                    itemSelected={itemSelected}
                                    selectedRecord={selectedIndex}
                                    selectionChanged={selectionChangeHandler}
                                    basicModeButtonHandler={
                                        embeddedModeButtonHandler
                                    }
                                    onClick={buttonClickHandler}
                                    isDataLoaded={true}
                                    basic={true}
                                    displaySummary={true}
                                ></ListView>
                            </div>
                        </div>
                    );
                })}
            </div>
        );
    }

    return <div></div>;
};

const mapStateToProps = (state) => {
    return {
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language
    };
};

export default connect(mapStateToProps)(IqcResultsGrid);
