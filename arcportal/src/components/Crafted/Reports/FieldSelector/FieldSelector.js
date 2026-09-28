import React, { useEffect, useState } from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import Post from '../../../../Data/Post';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import ArcFieldSelector from '../../../Forms/ArcFieldSelector/ArcFieldSelector';

const FieldSelector = (props) => {

    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [searchResults, setSearchResults] = useState([]);

    useEffect(() => {
        const criteria = { Name: props.config.QueryName, Parameters: props.data };
        Post('query/filteredget', criteria, fieldResultsHandler, errorWhenRetrievingData);
    }, [props.config, props.data]);

    const fieldResultsHandler = (data) => {
        const formatString = props.data.filter(d => d.key === "format")[0].value;
        const newData = data.map((record) => { return {label: record.label, value: record.value, enabled: "Yes" }});
        let lineNumber = 1;
        for (const line of newData) {
            line.combo = lineNumber;
            if (formatString > "467" && formatString < "473") {
                lineNumber = lineNumber === 1 ? 2 : 1;
            }
        }
        setSearchResults(newData);
        props.changeHandler("FieldSelector",newData)
    }

    const errorWhenRetrievingData = (response) => {
        updateErrorStatus({visible: true, message: response.data});
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    const changeHandler = (id, data) => {
        setSearchResults(data);
        props.changeHandler("FieldSelector",data)
    }

    const options = [{key: 1, text: "1"}, {key: 2, text: "2"}]
    const fieldConfig = { Id: "FieldSelector", Options: options, CanMoveEntries: true, IncludeOptions: true }

    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            <ArcFieldSelector config={fieldConfig} data={searchResults} changeHandler={changeHandler} canenable={true} language={props.language}></ArcFieldSelector>

            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        </div>
    )
}

export default FieldSelector;