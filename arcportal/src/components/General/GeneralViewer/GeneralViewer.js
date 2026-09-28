import React from 'react';
import './GeneralViewer.css';
import FieldViewer from './FieldViewer/FieldViewer';
import { sectionHasVisibleContent, subsectionHasVisibleContent } from './fieldViewerVisibility';

/**
 * GeneralViewer component renders sections and subsections with their respective fields.
 * 
 * @param {Object} props - The props object containing the sections and language data.
 * @param {Object[]} props.data.Sections - An array of section objects.
 * @param {string} props.data.Sections[].Id - The unique identifier for the section.
 * @param {string} [props.data.Sections[].Title] - The title of the section.
 * @param {Object[]} [props.data.Sections[].Fields] - An array of field objects within the section.
 * @param {Object[]} [props.data.Sections[].SubSections] - An array of subsection objects within the section.
 * @param {string} props.language - The language to be used for displaying fields.
 * @returns {JSX.Element} The rendered GeneralViewer component.
 */
const GeneralViewer = (props) => {
    const sectionsToRender = (props.data?.Sections ?? []).filter(sectionHasVisibleContent);
    return (
        <div>
            {sectionsToRender.map((section) => (
                <div
                    key={section.Id}
                    className='generalviewer-sectioncontent'
                    data-testid={section.Id ? `record-section-${section.Id}` : undefined}
                >
                    {section.Title ? ( 
                        <div className='generalviewer-sectiontitle'>
                            {section.Title}
                        </div>
                    ) : (
                        <div className='generalviewer-no-section-title'></div>
                    )}
                    {section.Fields?.length > 0 && ( 
                        <FieldViewer data={section.Fields} language={props.language} />
                    )}
                    {section.SubSections?.length > 0 && ( 
                        section.SubSections
                            .filter(subsectionHasVisibleContent)
                            .map((subsection) => (
                            <div key={subsection.Id} className='generalviewer-subsectioncontent' data-testid={subsection.Id ? `record-section-${subsection.Id}` : undefined}>
                                {subsection.Title ? ( 
                                    <div className='generalviewer-subsectiontitle'>
                                        {subsection.Title}
                                    </div>
                                ) : null}
                                {subsection.Fields?.length > 0 && (
                                    <FieldViewer data={subsection.Fields} language={props.language} />
                                )}
                            </div>
                        ))
                    )}
                </div>
            ))}
            <br />
        </div>
    );
};

export default GeneralViewer;


