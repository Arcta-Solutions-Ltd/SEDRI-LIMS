import React from 'react';
import './FormattedJsonDetails.css';
import FormattedArray from '../FormattedJsonViewer/FormattedArray';
import { fragmentForElement, joinPath } from '../FormattedJsonViewer/formattedJsonIds';

/**
 * Renders a list of formatted payload fields as label/value lines. Collections render as grids through
 * {@link FormattedArray}, and nested objects recurse back into this component.
 *
 * @param {Object} props
 * @param {Array|Object} props.data - Rendered items from the backend, or a plain object to show as lines.
 * @param {string} [props.title] - Heading of the block this list belongs to.
 * @param {number} [props.titleWidth] - Longest label length in the block, used to pick the label column width.
 * @param {string} [props.path] - Accumulated id path of the parent, used to keep nested DOM ids unique.
 */
const FormattedJsonDetails = (props) => {

    let lineClass = "formattedjsonviewer-line";
    let titleWidth = 0;
    let dataToDisplay = "";
    const path = props.path ?? "";

    if (Array.isArray(props.data)) {
        props.data.forEach(element => {
            if(element.ArrayItems != undefined){
                if(element.Label !== null && element.Label !== undefined && element.Label.length > titleWidth && element.ArrayItems !== null && element.ArrayItems.length !== 0){
                    titleWidth = element.Label.length;
                }
            }
            else {
                if(element.Label !== null && element.Label !== undefined && element.Label.length > titleWidth){
                    titleWidth = element.Label.length;
            }        
        }
        });
        dataToDisplay = props.data;
    }
    else 
    {
        dataToDisplay = Object.entries(props.data).map(([key, value]) => ({ Key: key, Label: key, Contents: value }));
    }


    let labelWidth = titleWidth > 30 ? "formattedjsondetails-label-wide" : "formattedjsondetails-label";

    return (
        <div>
            {dataToDisplay.map((element, index) => {
                const divider = element.Label === null ? "" : ":";
                const elementPath = joinPath(path, fragmentForElement(element, index));
                lineClass = "formattedjsondetails-line" ;

                if (Array.isArray(element.ArrayItems)) {
                    return(<FormattedArray key={elementPath} data={element.ArrayItems} title={element.Label} titleWidth={titleWidth} columnHeadings={element.ColumnHeadings} path={elementPath}></FormattedArray>)
                } else if (Array.isArray(element.ChildItems)) {
                    return(
                        <div key={elementPath}>
                            <div className={lineClass} id={`formattedjson-line-${elementPath}`}>
                                <div className={labelWidth} id={`formattedjson-label-${elementPath}`}>{element.Label}{divider}</div>
                                <div className="formattedjsondetails-item" id={`formattedjson-value-${elementPath}`}>
                                    <FormattedJsonDetails data={element.ChildItems} title={element.Label} titleWidth={titleWidth} path={elementPath}></FormattedJsonDetails>
                                </div>
                            </div>
                        </div>
                    )
                } else {
                    let contents = element.Contents;
                    const isObjectOrArray = typeof contents === 'object' && contents !== null;
                    return(
                        <div key={elementPath}>
                            {contents !== undefined && contents !== null && contents !== "null" &&
                            (isObjectOrArray || (contents !== '' && contents.length > 0)) ? (
                                <div className={lineClass} id={`formattedjson-line-${elementPath}`}>
                                    <div className={labelWidth} id={`formattedjson-label-${elementPath}`}>{element.Label}{divider}</div>
                                    <div className="formattedjsondetails-item" id={`formattedjson-value-${elementPath}`}>
                                        {isObjectOrArray ? (
                                            Array.isArray(contents) ? (
                                                <FormattedArray data={contents} title={element.Label} titleWidth={titleWidth} path={elementPath}></FormattedArray>
                                            ) : (
                                                <FormattedJsonDetails data={contents} title={element.Label} titleWidth={titleWidth} path={elementPath}></FormattedJsonDetails>
                                            )
                                        ) : contents}
                                    </div>
                                </div>
                            ) : (null) }
                        </div>
                    )
                }
            })}
        </div>
    );
};
  
export default FormattedJsonDetails;
