import React, { useEffect, useState } from 'react';
import { Callout, DelayedRender, Spinner } from '@fluentui/react';
import './ArcCallout.css';
import FormattedJsonDetails from '../../Crafted/Queue/FormattedJsonDetails/FormattedJsonDetails';
import FindCaseInsensitiveProperty from '../../../Utils/General/FindCaseInsensitiveProperty';
import {
    fetchFormattedTestResultsForCallout,
    rowNeedsLazyCalloutFetch,
} from '../../../Utils/Specimen/testCalloutLoader';

const ArcCallout = (props) => {

    const [enable, setEnable] = useState(false);
    const [mouseOver, setMouseOver] = useState(false);
    const [lazyLoading, setLazyLoading] = useState(false);
    const [lazyDisplayData, setLazyDisplayData] = useState(null);
    const [lazyError, setLazyError] = useState(false);

    useEffect(() => {
        if(enable == false && props.trigger == true)
        {
            setEnable(true);
        }
        if (props.trigger == false && mouseOver == false) {
            setEnable(false);   
        }
    }, [props.trigger, mouseOver]);
    useEffect(() => {
        if(mouseOver == false && props.trigger == false)
        {
            setEnable(false);
        } 
    }, [mouseOver]);

    useEffect(() => {
        if (!props.calloutLazyLoad || !props.trigger || !props.item) {
            return;
        }
        if (!rowNeedsLazyCalloutFetch(props.item)) {
            setLazyDisplayData(null);
            setLazyLoading(false);
            setLazyError(false);
            return;
        }

        const idKey = FindCaseInsensitiveProperty(props.item, 'Id');
        const testRowId = idKey ? props.item[idKey] : undefined;
        if (testRowId === undefined || testRowId === null || !props.calloutParentType) {
            return;
        }

        setLazyLoading(true);
        setLazyError(false);
        fetchFormattedTestResultsForCallout(
            props.calloutParentType,
            testRowId,
            (row) => {
                setLazyLoading(false);
                const resultsKey = FindCaseInsensitiveProperty(row, 'TestResults');
                const testResults = resultsKey ? row[resultsKey] : undefined;
                if (testResults !== undefined && testResults !== null && testResults !== '[]') {
                    try {
                        const parsed = typeof testResults === 'string' ? JSON.parse(testResults) : testResults;
                        if (typeof parsed === 'object' && parsed !== null) {
                            setLazyDisplayData(parsed);
                            return;
                        }
                    } catch (e) {
                        /* ignore parse errors */
                    }
                }
                setLazyDisplayData(null);
            },
            () => {
                setLazyLoading(false);
                setLazyError(true);
                setLazyDisplayData(null);
            }
        );
    }, [props.calloutLazyLoad, props.calloutParentType, props.item, props.trigger]);

    let visible = false;
    let displayData;

    const key = FindCaseInsensitiveProperty(props.item, "TestResults");
    const testResults = props.item[key];
    if (testResults !== undefined && testResults !== null && testResults !== "[]") {
        try {
            displayData = typeof testResults === 'string' ? JSON.parse(testResults) : testResults;
            if (typeof displayData === 'object' && displayData !== null) {
                visible = true;
            }
        } catch (e) {
            /* ignore parse errors */
        }
    }

    if (!visible && lazyDisplayData !== null) {
        displayData = lazyDisplayData;
        visible = true;
    }

    const showLoading = props.calloutLazyLoad && lazyLoading && enable && props.trigger;
    const showCallout = (visible || showLoading || lazyError) && enable && props.trigger;

    return (
        <div>
        {showCallout && (
            <Callout className="arccallout-margin"
                gapSpace={-10}
                target={props.event}
                calloutMaxHeight={600}
                coverTarget={false}
                onMouseEnter={ (event) => { setMouseOver(true)}}
                onMouseLeave={ (event) => { setMouseOver(false)}}
            >
                <DelayedRender>
                    <div>
                        {showLoading && (
                            <div data-testid="test-callout-loading">
                                <Spinner label="" />
                            </div>
                        )}
                        {!showLoading && lazyError && (
                            <div data-testid="test-callout-error">—</div>
                        )}
                        {!showLoading && !lazyError && visible && (
                            <FormattedJsonDetails data={displayData}></FormattedJsonDetails>
                        )}
                    </div>
                </DelayedRender>
            </Callout>
        )}
        </div>

    );
};
  
export default ArcCallout;
