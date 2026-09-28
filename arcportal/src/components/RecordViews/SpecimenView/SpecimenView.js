import React, { useState } from 'react';

import './SpecimenView.css';
import { PrimaryButton, DefaultButton, CommandBarButton, IconButton, Pivot, PivotItem, TooltipHost } from '@fluentui/react';
import InlineMenu from '../../General/InlineMenu/InLineMenu';
import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
import { useId } from 'react';

const SpecimenView = (props) => {

    const [showContextualMenu, updateShowContextualMenu] = useState({ visible: false, target: ""});

    const calloutProps = { gapSpace: 10 };
    const tooltipId = useId();

    const menuClickHandler = (event) => {
        updateShowContextualMenu({ visible: true, target: event.target});
    };

    const closeMenuHandler = () => {
        updateShowContextualMenu({ visible: false, target: ""});
    };

    const itemButtons = props.buttons.filter((button) => {
        return button.onSelect && button.key !== 'view';
    });

    const menuItems = MapButtonsToContextMenu(itemButtons, props.buttonClickHandler);

    let largePanelButtonStyles={
        root: { marginRight: '10px', padding: '10px', backgroundColor: 'rgba(230, 230, 250, 0)'},
        label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
    }

    let smallPanelButtonStyles={
        root: { padding: '10px', backgroundColor: 'rgba(230, 230, 250, 0)'},
        label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
    };

    let smallRightMostPanelButtonStyles={
        root: { marginRight: '10px', padding: '10px', backgroundColor: 'rgba(230, 230, 250, 0)'},
        label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
    };

    let specimenDetails = (
        <div className='specimenview-content'>

{/* Top row */}

            <br />
            <div className='specimenview-specimen-details-with-header-full-width'>
                <div className='specimenview-specimen-details-assignments'>
                    <div className='specimenview-subtitle-assignments'>
                        Assignments:
                    </div>
                    <div className='specimenview-field-last'>
                        <div className='specimenview-itemname'>
                            Accession Number:
                        </div>
                        <div className='specimenview-itemvalue'>
                            {props.record.AccessionNumber}
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-field-last'>
                            <div className='specimenview-itemname'>
                                Current State:
                            </div>
                            <div className='specimenview-itemvalue'>
                                {props.record.State}
                            </div>
                        </div>
                    </div>
                </div>
            </div>

{/* Middle row */}

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Patient Collection details:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editcollection')}}
                        />
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Patient Name:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.PatientName}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Patient Unique ID:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {18244382156431}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Location:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.PatientLocation}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Ward:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.Ward}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Admission Date:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.AdmissionDate}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Contact Number:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.ClinicalContactNo}
                                </div>
                            </div>
                            <div className='specimenview-field-last'>
                                <div className='specimenview-itemname'>
                                    Diagnosis:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.Diagnosis}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

{/* 2nd block in middle row */}

                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Specimen Details:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editspecimen')}}
                        />
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Specimen Type:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.SpecimenType}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Specimen Site:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.SpecimenSite}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Specimen Weight:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.BottleOnlyWeight}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Existing Bar-Code:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.ExistingBarcode}
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Collection Date/Time:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.CollectionDateTime}&nbsp;-&nbsp;13:45
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Received Date/Time:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    {props.record.ReceivedDateTime}&nbsp;-&nbsp;09:17
                                </div>
                            </div>
                            <div className='specimenview-field-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

{/* Bottom row */}

            <div className='specimenview-specimen-details-with-header-full-width'>
                <div className='specimenview-subtitle'>
                    Initial Assessment:
                    <CommandBarButton
                        iconProps={{ iconName: 'Edit' }}
                        text="Edit"
                        styles={largePanelButtonStyles}
                        onClick={() => {props.buttonClickHandler('editinitassess')}}
                    />
                </div>
                <div className='specimenview-specimen-details'>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-field-long'>
                            <div className='specimenview-itemname'>
                                Specimen Condition:
                            </div>
                            <div className='specimenview-itemvalue'>
                                {props.record.ReceivedCondition}.
                            </div>
                        </div>
                        <div className='specimenview-field-last'>
                            <div className='specimenview-itemname'>
                                Message:
                            </div>
                            <div className='specimenview-itemvalue'>
                                {/* {props.record.ReceivedCondition} */}
                                Requires urgent handling.
                            </div>
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-field'>
                            <div className='specimenview-itemname'>
                                Specimen Appearance:
                            </div>
                            <div className='specimenview-itemvalue'>
                                Cloudy
                            </div>
                        </div>
                        <div className='specimenview-field-last'>
                            <div className='specimenview-itemname'>
                            </div>
                            <div className='specimenview-itemvalue'>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );

/* Direct tests */

    let directTests = (
        <div className='specimenview-content'>

{/* Top row */}

            <div className='specimenview-subtitle'>
                &nbsp;
                <div className='pecimenview-sectionbuttons'>
                    <CommandBarButton
                        iconProps={{ iconName: 'Add' }}
                        text="Add Test(s)"
                        onClick={() => props.buttonClickHandler('addtests')}
                        styles={{
                            root: { marginBottom: '5px', padding: '8px', backgroundColor: 'whitesmoke'},
                            rootHovered: { backgroundColor: '#eee' },
                            label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
                        }}
                    />
                </div>
            </div>

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Cell Count:
                        <div className='specimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Edit' }}
                                text="Edit Results"
                                onClick={() => props.buttonClickHandler('editCellCount')}
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    WBC (/mm):
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    WBC Qualitative Result:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Moderate
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    Polymorphonuclear:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                        </div>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    RBC (/mm):
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    RBC Qualitative Result:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Moderate
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    Mononuclear:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Gram Stain:
                        <div className='pecimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Edit' }}
                                text="Edit Results"
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname-short'>
                                    WBC:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    ARE SEEN
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname-short'>
                                    Organism:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    FUNGAL ELEMENTS
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname-short'>
                                    Organism:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    GNDC - INTRACELLULAR
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname-short'>
                                    Organism:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Mixed Flora
                                </div>
                            </div>
                        </div>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    Epi Cells:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    ARE SEEN
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                    NOT SEEN
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                    +++
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                    SCANTY
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

{/* 2nd row of tests */}

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        ZN Stain:
                        <div className='pecimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Edit' }}
                                text="Edit Results"
                                onClick={() => props.buttonClickHandler('editZNStain')}
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    AFB Quantity:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    SCANTY
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Auramine:
                        <div className='pecimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Edit' }}
                                text="Edit Results"
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>

                    <div className='specimenview-specimen-details-narrow'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    Result:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

{/* 3rd row of tests */}

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        India Ink:
                        <div className='pecimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Add' }}
                                text="Add Results"
                                onClick={() => props.buttonClickHandler('editIndiaInk')}
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-no-results'>
                        ------ Results pending ------
                    </div>
                </div>

                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Wet Prep:
                        <div className='pecimenview-sectionbuttons'>
                            <CommandBarButton
                                iconProps={{ iconName: 'Add' }}
                                text="Add Results"
                                styles={smallPanelButtonStyles}
                            />
                            <CommandBarButton
                                iconProps={{ iconName: 'Delete' }}
                                text="Remove"
                                styles={smallRightMostPanelButtonStyles}
                            />
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-no-results'>
                        ------ Results pending ------
                    </div>
                </div>
            </div>
        </div>
    );

    // First attempt at providing specimen options - didn't like so replaced with a single vertical ellipse method, but keeping this
    // for now in case we have a change of heart.
    //
    // let topMenu = itemButtons.map((button) => {
    //     return (
    //         <CommandBarButton
    //             styles={{
    //                 root: { marginBottom: '0px', paddingLeft: '5px', backgroundColor: 'whitesmoke'},
    //                 rootHovered: { backgroundColor: '#eee' },
    //                 label: { color: '#0078d7', fontSize: '14px', fontWeight: '400' }
    //             }}
    //             iconProps={{iconName: button.icon}}
    //             text={button.text}
    //             onClick={() => {props.buttonClickHandler(button.key)}}
    //         />
    //     )            
    // });

    let cultureDetails = (
        <div className='specimenview-content'>
            <div className='specimenview-subtitle'>
                &nbsp;
                <div className='specimenview-sectionbuttons'>
                    <CommandBarButton
                        iconProps={{ iconName: 'Add' }}
                        text="Add AST Test(s)"
                        onClick={() => props.buttonClickHandler('addtests')}
                        styles={{
                            root: { marginBottom: '5px', padding: '8px', backgroundColor: 'whitesmoke'},
                            rootHovered: { backgroundColor: '#eee' },
                            label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
                        }}
                    />
                </div>
            </div>

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-with-header'>
                    <div className='specimenview-subtitle'>
                        Organism:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editCultureOrganism')}}
                        />
                    </div>
                    <div className='specimenview-specimen-details'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-highlight'>
                                <div className='specimenview-itemname-short'>
                                    Name:
                                </div>
                                <div className='specimenview-itemvalue-special'>
                                    Legionella interrogans serovar
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname-short'>
                                    Group:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Sal
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname-short'>
                                    Code:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    LGP
                                </div>
                            </div>
                            <div className='specimenview-field-last'>
                                <div className='specimenview-itemname-short'>
                                    Serotype:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Type A
                                </div>
                            </div>
                        </div>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Quantity:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    +++
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                    Positive Date / Time:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    17/08/19&nbsp;&nbsp;&nbsp;17:54
                                </div>
                            </div>
                            <div className='specimenview-field'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    Serotype Profile:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Method Used:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editCultureIDMethod')}}
                        />
                    </div>
                    <div className='specimenview-specimen-details'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    API / ID Panel:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    API 20E
                                </div>
                            </div>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    ID Profile:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    % ID:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div className='specimenview-row-of-details-narrow'>
                <div className='specimenview-specimen-details-with-header'>
                    <div className='specimenview-subtitle'>
                        Guidance and Caveats:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editCultureGuidance')}}
                        />
                    </div>
                    <div className='specimenview-specimen-details'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-long-last'>
                                <div className='specimenview-itemname'>
                                    Comment 1:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Aerococcus, Micrococcus, Corynebacterium, Bacillus,Vibrio species, Pseudomonas fluorescens, Pseudomonas stutzeri on blood culture.  Contaminant. Please repeat blood culture.
                                </div>
                            </div>
                            <div className='specimenview-field-long-last'>
                                <div className='specimenview-itemname'>
                                    Comment 2:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    E.coli on blood culture.  If patient has not had antibiotics please send a urine sample. Consider investigating urinary tract.
                                </div>
                            </div>
                            <div className='specimenview-field-long-last'>
                                <div className='specimenview-itemname'>
                                    Extra Details / Notes:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Double-check results.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className='specimenview-specimen-details-narrow-with-header'>
                    <div className='specimenview-subtitle'>
                        Other:
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editCultureOther')}}
                        />
                    </div>
                    <div className='specimenview-specimen-details'>
                        <div className='specimenview-specimen-details-col'>
                            <div className='specimenview-field-narrow'>
                                <div className='specimenview-itemname'>
                                    Aliquot ID:
                                </div>
                                <div className='specimenview-itemvalue'>
                                    100534351
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                    Display in Report?
                                </div>
                                <div className='specimenview-itemvalue'>
                                    Yes
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                            <div className='specimenview-field-narrow-last'>
                                <div className='specimenview-itemname'>
                                </div>
                                <div className='specimenview-itemvalue'>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );

    let ASTDetails = (
        <div className='specimenview-content'>
            <div className='specimenview-subtitle'>
                &nbsp;
                <div className='specimenview-sectionbuttons'>
                    <CommandBarButton
                        iconProps={{ iconName: 'Add' }}
                        text="Add AST Test(s)"
                        onClick={() => props.buttonClickHandler('addtests')}
                        styles={{
                            root: { marginBottom: '5px', padding: '8px', backgroundColor: 'whitesmoke'},
                            rootHovered: { backgroundColor: '#eee' },
                            label: { color: '#0078d7', fontSize: '14px', fontWeight: '500' }
                        }}
                    />
                </div>
            </div>

            <div className='specimenview-dummy-force-margin'>
            <div className='specimenview-specimen-details-with-header-full-width'>
                <div className='specimenview-subtitle'>
                    Disk Tests:
                    <CommandBarButton
                        iconProps={{ iconName: 'Edit' }}
                        text="Edit"
                        styles={largePanelButtonStyles}
                        onClick={() => {props.buttonClickHandler('editinitassess')}}
                    />
                </div>
                <div className='specimenview-specimen-details'>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Antibiotic
                        </div>
                        <div className='specimenview-column-field'>
                            Ampicillin
                        </div>
                        <div className='specimenview-column-field'>
                            Amikacin
                        </div>
                        <div className='specimenview-column-field'>
                            Ceftazidime
                        </div>
                        <div className='specimenview-column-field'>
                            Doripenem
                        </div>
                        <div className='specimenview-column-field'>
                            Ampicillin
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Amikacin
                        </div>
                        <div className='specimenview-column-field'>
                            Ceftazidime
                        </div>
                        <div className='specimenview-column-field'>
                            Doripenem
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Dose
                        </div>
                        <div className='specimenview-column-field'>
                            AMP_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            AMK_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            CAZ_ND30
                        </div>
                        <div className='specimenview-column-field'>
                            DOR_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            AMP_ND10
                        </div>
                        {/* <div className='specimenview-column-field'>
                            AMK_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            CAZ_ND30
                        </div>
                        <div className='specimenview-column-field'>
                            DOR_ND10
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Zone Diamter (mm)
                        </div>
                        <div className='specimenview-column-field'>
                            9
                        </div>
                        <div className='specimenview-column-field'>
                            11
                        </div>
                        <div className='specimenview-column-field'>
                            12
                        </div>
                        <div className='specimenview-column-field'>
                            5
                        </div>
                        <div className='specimenview-column-field'>
                            9
                        </div>
                        {/* <div className='specimenview-column-field'>
                            11
                        </div>
                        <div className='specimenview-column-field'>
                            12
                        </div>
                        <div className='specimenview-column-field'>
                            5
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Susceptibility
                        </div>
                        <div className='specimenview-column-field specimenview-red'>
                            Resistant
                        </div>
                        <div className='specimenview-column-field specimenview-amber'>
                            Intermediate
                        </div>
                        <div className='specimenview-column-field specimenview-green'>
                            Susceptibile
                        </div>
                        <div className='specimenview-column-field specimenview-red'>
                            Resistant
                        </div>
                        <div className='specimenview-column-field specimenview-red'>
                            Resistant
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Intermediate
                        </div>
                        <div className='specimenview-column-field'>
                            Susceptibile
                        </div>
                        <div className='specimenview-column-field'>
                            Resistant
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title-wide'>
                            Notes
                        </div>
                        <div className='specimenview-column-field'>
                            Results broadly as expected.
                        </div>
                        <div className='specimenview-column-field'>
                            Consider repeating.
                        </div>
                        <div className='specimenview-column-field'>
                            Repeated after suspicious results.
                        </div>
                        <div className='specimenview-column-field'>
                            As expected.
                        </div>
                        <div className='specimenview-column-field'>
                            Results broadly as expected.
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Consider repeating.
                        </div>
                        <div className='specimenview-column-field'>
                            Repeated after suspicious results.
                        </div>
                        <div className='specimenview-column-field'>
                            As expected.
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Display in Report?
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        <div className='specimenview-column-field'>
                            No
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        {/* <div className='specimenview-column-field'>
                            No
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div> */}
                    </div>
                </div>
            </div>
            </div>
            <div className='specimenview-dummy-force-margin'>
            <div className='specimenview-specimen-details-with-header-full-width'>
                <div className='specimenview-subtitle'>
                    Strip Tests:
                    <CommandBarButton
                        iconProps={{ iconName: 'Edit' }}
                        text="Edit"
                        styles={largePanelButtonStyles}
                        onClick={() => {props.buttonClickHandler('editinitassess')}}
                    />
                </div>
                <div className='specimenview-specimen-details'>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Antibiotic
                        </div>
                        <div className='specimenview-column-field'>
                            Ampicillin
                        </div>
                        <div className='specimenview-column-field'>
                            Amikacin
                        </div>
                        <div className='specimenview-column-field'>
                            Ceftazidime
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Doripenem
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Dose
                        </div>
                        <div className='specimenview-column-field'>
                            AMP_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            AMK_ND10
                        </div>
                        <div className='specimenview-column-field'>
                            CAZ_ND30
                        </div>
                        {/* <div className='specimenview-column-field'>
                            DOR_ND10
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            MIC (ug/mL)
                        </div>
                        <div className='specimenview-column-field'>
                            9
                        </div>
                        <div className='specimenview-column-field'>
                            11
                        </div>
                        <div className='specimenview-column-field'>
                            12
                        </div>
                        {/* <div className='specimenview-column-field'>
                            5
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Susceptibility
                        </div>
                        <div className='specimenview-column-field specimenview-green'>
                            Susceptibile
                        </div>
                        <div className='specimenview-column-field specimenview-red'>
                            Resistant
                        </div>
                        <div className='specimenview-column-field specimenview-amber'>
                            Intermediate
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Resistant
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title-wide'>
                            Notes
                        </div>
                        <div className='specimenview-column-field'>
                            Results broadly as expected.
                        </div>
                        <div className='specimenview-column-field'>
                            Consider repeating.
                        </div>
                        <div className='specimenview-column-field'>
                            Repeated after suspicious results.
                        </div>
                        {/* <div className='specimenview-column-field'>
                            As expected.
                        </div> */}
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-column-title'>
                            Display in Report?
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        <div className='specimenview-column-field'>
                            No
                        </div>
                        <div className='specimenview-column-field'>
                            Yes
                        </div>
                        {/* <div className='specimenview-column-field'>
                            Yes
                        </div> */}
                    </div>
                </div>
            </div>
            </div>

            <div className='specimenview-specimen-details-with-header-full-width'>
                <div className='specimenview-specimen-details-assignments'>
                    <div className='specimenview-subtitle-assignments'>
                        Other:
                    </div>
                    <div className='specimenview-field-medium-last'>
                        <div className='specimenview-itemname'>
                            Test Pattern:
                        </div>
                        <div className='specimenview-itemvalue'>
                            EUCAST - Pseudomonas aeruginosa
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <div className='specimenview-field-last'>
                            <div className='specimenview-itemname-short'>
                                ESBL:
                            </div>
                            <div className='specimenview-itemvalue'>
                                Negative
                            </div>
                        </div>
                    </div>
                    <div className='specimenview-specimen-details-col'>
                        <CommandBarButton
                            iconProps={{ iconName: 'Edit' }}
                            text="Edit"
                            styles={largePanelButtonStyles}
                            onClick={() => {props.buttonClickHandler('editinitassess')}}
                        />
                    </div>
                </div>
            </div>
        </div>
    );


    return (
        <div className='specimenview-page'>
            <div className='specimenview-titlebar'>
                <div className='specimenview-title'>
                    Specimen Record
                </div>
                <div>
                    {/* {topMenu} */}
                    <TooltipHost
                        content={'More options'}
                        id={tooltipId}
                        calloutProps={calloutProps}
                    >
                        <IconButton
                            iconProps={{ iconName: 'MoreVertical' }}
                            onClick={(event) => menuClickHandler(event)}
                            styles={{
                                root: { marginBottom: '5px', padding: '14px', backgroundColor: 'whitesmoke'},
                                rootHovered: { backgroundColor: '#eee' },
                                label: { color: '#0078d7', fontSize: '16px', fontWeight: '400' },
                                icon: { fontSize: '20px', color: '#0078d7' }
                            }}
                        />
                    </TooltipHost>
                </div>
            </div>
            <InlineMenu
                showMenu={showContextualMenu.visible}
                closeMenu={closeMenuHandler}
                target={showContextualMenu.target}
                items={menuItems}>
            </InlineMenu>
            <Pivot defaultSelectedKey={props.page !== undefined ? props.page : '0'}>
                <PivotItem
                    headerText="Specimen Details" itemKey='0'
                    headerButtonProps={{
                    'data-order': 1,
                    'data-title': 'Specimen Details',
                    }}
                >
                    {specimenDetails}
                </PivotItem>
                <PivotItem headerText="Direct Tests" itemKey='1'>
                    {directTests}
                </PivotItem>
                <PivotItem headerText="Culture #1" itemKey='2'>
                    {cultureDetails}
                </PivotItem>
                <PivotItem headerText="Culture #1 AST" itemKey='3'>
                    {ASTDetails}
                </PivotItem>
            </Pivot>
            <div className='specimenview-buttons'>
                <div className='specimenview-button'>
                    <DefaultButton
                        text='Exit'
                        onClick={props.cancel}
                        styles={{
                            root: { border: '0px', padding: '2px', backgroundColor: '#ddd'},
                            rootHovered: { backgroundColor: '#ccc' },
                            label: {  }}}
                    />
                </div>
                <div className='specimenview-right-buttons'>
                    <div className='specimenview-button'>
                        <PrimaryButton
                            text='Previous Specimen'
                        />
                    </div>
                    <div className='specimenview-button'>
                        <PrimaryButton
                            text='Next Specimen'
                        />
                    </div>
                </div>
            </div>
        </div>

    )
};

export default SpecimenView;
