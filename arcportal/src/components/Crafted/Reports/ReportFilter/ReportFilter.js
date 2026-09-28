import React, {useEffect, useState} from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import Post from '../../../../Data/Post';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const ReportFilter = (props) => {

    const [directTestSelector, setDirectTestSelector] = useState();
    const [cultureList, setCultureList] = useState([]);
    const [commentsList, setCommentsList] = useState([]);

    useEffect(() => {
        const criteria = { Name: 'ReportFilterContents', Parameters: [{ Key: 'id', Value: props.specimenid }] };
        Post('query/filteredget', criteria, dynamicListsRetrieved, errorWhenRetrievingData);
    }, [])
    
    /**
     * Propagates ReportFilter grid toggle changes to the parent form so print preview can reload.
     * @param {string} key - Grid field id (DirectTestSelector, SelectorGrid, or CommentGrid).
     * @param {Object} value - Updated grid row values for the changed grid.
     */
    const selectorChangeHandler = (key, value) => {
        if (key === "DirectTestSelector") {
            props.changeHandler(key, { DirectTestList: value, Cultures: cultureList, Comments: commentsList }); 
            setDirectTestSelector(value);           
        }
        if (key === "SelectorGrid") {
            props.changeHandler(key, { DirectTestList: directTestSelector, Cultures: value, Comments: commentsList }); 
            setCultureList(value);
        }
        if (key === "CommentGrid") {
            props.changeHandler(key, { DirectTestList: directTestSelector, Cultures: cultureList, Comments: value }); 
            setCommentsList(value);
        }
    };    

    /**
     * Re-fetches specimen-level comment toggles after the comment edit sub-form saves.
     * @param {*} _x - Unused save callback argument from FieldGrid.
     * @param {*} _y - Unused save callback argument from FieldGrid.
     */
    const commentChangeReceived = (data) => {
        const results = dynamicListsRetrieved(data);
        props.changeHandler("CommentGrid", results); 
    }

    const dynamicListsRetrieved = (data) => {
        const directTestList = data.DirectTestList.map((c) => { return { DirectTest: c.Name, PrintOnReport: c.PrintOnReport, Id: c.Id}});
        setDirectTestSelector(directTestList);

        const cultureList = data.Cultures.map((c) => { return { Culture: c.Name, PrintOnReport: c.PrintOnReport, Id: c.Id}});
        setCultureList(cultureList);

        const commentsList = data.Comments.map((c) => { return { Comment: c.Comment, PrintOnReport: shouldPrintCommentOnReport(c.Id, c.PrintOnReport), Id: c.Id}});
        setCommentsList(commentsList);

        return { DirectTestList: directTestSelector, Cultures: cultureList, Comments: commentsList };
    }

    const errorWhenRetrievingData = (error) => {
        let x = 1;
    }

    /**
     * Re-fetches specimen-level comment toggles after the comment edit sub-form saves.
     */
    const onCommentSave = () => {
        const criteria = { Name: 'ReportFilterContents', Parameters: [{ Key: 'id', Value: props.specimenid }] };
        Post('query/filteredget', criteria, commentChangeReceived, errorWhenRetrievingData);
    }

    /**
     * Notifies the parent form after culture print selector save so print preview reloads.
     * Isolate comment toggles persist immediately to the database; preview reads those flags server-side.
     */
    const onCultureSave = () => {
        props.changeHandler("SelectorGrid", { DirectTestList: directTestSelector, Cultures: cultureList, Comments: commentsList }); 
    }

    const shouldPrintCommentOnReport = (id, defaultValue) => {
        const { config } = props;
        
        if (config.value?.Comments) {
            const commentRecord = config.value.Comments.find(c => c.Id === id);
            return commentRecord ? commentRecord.PrintOnReport : defaultValue;
        }
        
        return defaultValue;
    };
    

    const directtestSelectorConfig = { Id: "DirectTestSelector", Type: 'fieldgrid', Label: TranslateTag("@RepDir@", props.language), GridFields: [
        { Id: 'Id', Type: 'hidden'},
        { Id: 'DirectTest', Type: 'text', Width: 'extrawide' },
        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
    ], value: directTestSelector, RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeGridFormButton: false };
    const selectorGridConfig = { Id: "SelectorGrid", Type: 'fieldgrid', Label: TranslateTag("@RepCulA@", props.language), Icon: 'Edit',onSave: onCultureSave, IconText: '@CulEdi@', GridFields: [
        { Id: 'Id', Type: 'hidden'},
        { Id: 'Culture', Type: 'text', Width: 'extrawide' },
        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
    ], value: cultureList, RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeGridFormButton: true, FormUIEvent: "cultureprintselectoruievent"};
    const commentGridConfig = { Id: "CommentGrid", Type: 'fieldgrid', Label: TranslateTag("@RepCom@", props.language), Icon: 'Edit',onSave: onCommentSave, IconText: '@GenComG@', GridFields: [
        { Id: 'Id', Type: 'hidden'},
        { Id: 'Comment', Type: 'text', Width: 'extrawide' },
        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
    ], value: commentsList, RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeGridFormButton: true, FormUIEvent: "editcommentforselectoruievent"};

    let directTestDisplay = (null); 
    if (Array.isArray(directTestSelector) && directTestSelector.length > 0) {
        directTestDisplay = <SingleLineField key="DirectTestSelectorId" config={directtestSelectorConfig} changeHandler={selectorChangeHandler}></SingleLineField>
    }
    let commentDisplay = (null);
    if (Array.isArray(commentsList) && commentsList.length > 0) {
        commentDisplay = <SingleLineField key="CommentSelector" config={commentGridConfig} changeHandler={selectorChangeHandler} language={props.language}></SingleLineField>
    }
    let cultureDisplay = (null);
    if (Array.isArray(cultureList) && cultureList.length > 0) {
        cultureDisplay = <SingleLineField key="PrintSelector" config={selectorGridConfig} changeHandler={selectorChangeHandler} language={props.language}></SingleLineField>
    }


    return (
        <div id="reportfilter-root">
            {directTestDisplay}
            {commentDisplay}    
            {cultureDisplay}               
        </div>
    )
};

export default ReportFilter;