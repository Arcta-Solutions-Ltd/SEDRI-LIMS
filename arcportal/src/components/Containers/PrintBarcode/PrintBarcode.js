import React, { useState, useRef, useEffect, useCallback } from 'react';
import {connect} from 'react-redux';
import './PrintBarcode.css';
import { useReactToPrint } from 'react-to-print';
import BarcodeWrapper from './BarcodeWrapper';
import Post from '../../../Data/Post';


const PrintBarcode = (props) => {

    const [record, setRecord] = useState(null);
    const [barcodeReady, setBarcodeReady] = useState(false);

    const componentRef = useRef();
    const handlePrint = useReactToPrint({
        content: () => componentRef.current,
        onAfterPrint: () => { props.done() }
    });

    const errorWhenRetrievingData = (response) => {
        console.error("Error retrieving barcode data:", response);
    }

    const dataRetrievedSuccessfully = useCallback((data, index) => {
        setRecord(data);
    }, []);

    useEffect(() => {
        if (record) {
            const timer = setTimeout(() => {
                setBarcodeReady(true);
            }, 150);

            return () => clearTimeout(timer);
        }
    }, [record]);

    useEffect(() => {
        if (barcodeReady && componentRef.current) {
            handlePrint();
        }
    }, [barcodeReady, handlePrint]);

    useEffect(() => {
        let config = props.barcodeprintconfigs.filter((config) => {
            return config.Name === props.type;
        })[0];
        const criteria = { Name: config.FieldQuery, Parameters: [{ Key: 'id', Value: props.selectedRecord.id }]};
        Post('query/filteredget', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData, 0);
    }, []);


    return (
        <div>
            {record !== null ? (
                <div className='printbarcode-hide'>
                    <BarcodeWrapper ref={componentRef} config={props} record={record}/>
                </div>
            ) : (null)}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        barcodeprintconfigs: state.config.barcodeprintconfigs,
    };
}

export default connect(mapStateToProps)(PrintBarcode);
