import React from 'react';
import './FormattedJsonViewer.css';
import FormattedJsonDetails from '../FormattedJsonDetails/FormattedJsonDetails';
import { fragmentForElement } from './formattedJsonIds';

/**
 * Renders one row of a grid. Scalar cells sit side by side on the row itself; any nested collection or object
 * the row carries is rendered as its own indented block underneath, so a special consideration row or a
 * susceptibility override reads as part of the AST line it belongs to instead of as raw JSON in a cell.
 *
 * @param {Object} props
 * @param {Array} props.data - The row's cells.
 * @param {string} [props.title] - Text for the row's leading title cell; blank for rows after the first.
 * @param {number} [props.index] - Position of the row within the grid.
 * @param {number} [props.titleWidth] - Longest label length in the block, used to pick the title column width.
 * @param {string} [props.path] - Accumulated id path for the row, used to build cell ids.
 * @param {string} [props.id] - DOM id for the row.
 */
const FormattedSection = (props) => {

    let titleClass = props.titleWidth > 30 ? "formattedjsonviewer-title-wide" : "formattedjsonviewer-title";
    const data = Array.isArray(props.data) ? props.data : [];
    const path = props.path ?? "";

    const isNested = (element) => Array.isArray(element?.ArrayItems) || Array.isArray(element?.ChildItems);
    const cells = data.filter((element) => !isNested(element));
    const nested = data.filter(isNested);

    return (
    <div id={props.id}>
        <div className={"formattedjsonviewer-gridrowcontainer"}>
            <div className={titleClass}>{props.title}</div>
                {cells.map((element, index) => {
                        const fragment = fragmentForElement(element, index);
                        let contents = element?.Contents;
                        if (typeof contents === 'object' && contents !== null) {
                            contents = JSON.stringify(contents);
                        }
                        if (contents === null || contents === undefined || contents === 'null') {
                            contents = '';
                        }
                        return(
                        <div key={fragment} id={`formattedjson-grid-${path}-${fragment}`} className="formattedsection-item">{contents}</div>
                        )
                    })
                }
        </div>
        {nested.length > 0 && (
            <div className="formattedjsonviewer-nested">
                <FormattedJsonDetails data={nested} titleWidth={props.titleWidth} path={path}></FormattedJsonDetails>
            </div>
        )}
        </div>
    );
};

export default FormattedSection;
