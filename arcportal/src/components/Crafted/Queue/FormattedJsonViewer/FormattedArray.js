import React from 'react';
import './FormattedJsonViewer.css';
import FormattedSection from './FormattedSection';

/**
 * Renders a collection of payload rows as a grid. When the backend supplies column headings the grid gets a
 * heading row and the block title sits on it; otherwise the title falls back to sitting on the first row, which
 * is how grids without headings have always rendered.
 *
 * @param {Object} props
 * @param {Array} props.data - Rows, each with a `ChildItems` array of cells.
 * @param {string} [props.title] - Heading of the grid.
 * @param {number} [props.titleWidth] - Longest label length in the block, used to pick the title column width.
 * @param {Array<string>} [props.columnHeadings] - Translated headings in the same order as each row's cells.
 * @param {string} [props.path] - Accumulated id path, used to keep nested grid DOM ids unique.
 */
const FormattedArray = (props) => {

    const path = props.path ?? "";
    const headings = Array.isArray(props.columnHeadings) ? props.columnHeadings : [];
    const titleClass = props.titleWidth > 30 ? "formattedjsonviewer-title-wide" : "formattedjsonviewer-title";

    return (
        <div id={`formattedjson-grid-${path}`}>
                {headings.length > 0 && (
                    <div className="formattedjsonviewer-gridrowcontainer formattedjsonviewer-gridheadings" id={`formattedjson-grid-${path}-headings`}>
                        <div className={titleClass}>{props.title}:</div>
                        {headings.map((heading, headingIndex) => (
                            <div key={headingIndex} className="formattedsection-label">{heading}</div>
                        ))}
                    </div>
                )}
                {props.data.map((element, index) => {

                    let title = headings.length === 0 && index === 0 ? props.title + ":" : "";

                    return(<div key={index}>
                        <FormattedSection data={element.ChildItems} title={title} index={index} titleWidth={props.titleWidth} path={`${path}-row-${index}`} id={`formattedjson-grid-${path}-row-${index}`}></FormattedSection>
                    </div>);
                })}

        </div>
    );
};
  
export default FormattedArray;
