import React, {useState, useEffect} from 'react';
import Post from '../../../../Data/Post';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SimpleCard from '../../../General/SimpleCard/SimpleCard';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import { IconButton } from '@fluentui/react';
import './OrganismList.css';


const OrganismList = (props) => {

    const [searchResults, setSearchResults] = useState();
    const [errorStatus, setErrorStatus] = useState({visible: false, message: ''});

    const iconStyles = {
        root: { fontSize: '48px', color: 'darkblue' }
    };

    useEffect(() => {      
        const parameters = props.data.filter((f) => f.Key !== undefined && f.value !== undefined && (f.Key.toLowerCase() === "search" || f.Key.toLowerCase() === "genusid" || f.Key.toLowerCase() === "speciesid" || f.Key.toLowerCase() === "subspeciesid" || f.Key.toLowerCase() === "serotypeid"));
        const criteria = { Name: props.config.QueryName, Parameters: parameters };
        Post('query/filteredget', criteria, resultsHandler, errorWhenRetrievingData);
    }, [props.config, props.data]);

    const resultsHandler = (data) => {
        setSearchResults(data);
    }

    const nextClickHandler = () => {
        props.rightButtonClick([], "custom");
    };

    const errorWhenRetrievingData = (response) => {
        const errorMessage = response.data !== undefined ? response.data : response;
        setErrorStatus({visible: true, message: errorMessage});
    }

    const errorCloseHandler = () => {
        setErrorStatus({visible: false, message: ""});
    }

    const buttonClickHandler = (data) => {
        var searchResultsCopy = [...searchResults];
        var resultToChange = searchResultsCopy.filter((s) => s.Id == data.Id);
        resultToChange[0].selected = resultToChange[0].selected === undefined ? true : ! resultToChange[0].selected;

        if (resultToChange[0].selected) {
            const changes = [{ key: "OrganismId", value: { Key: "OrganismId", value: data.Id}}, { key: "OrganismName", value: { Key: "OrganismName", value: data.Description  }  }];
            props.changeHandler("multiplechanges", changes, { rootCopy: true});  
        }

        var resultToChange = searchResultsCopy.filter((s) => s.selected && s.Id != data.Id);
        if (resultToChange.length > 0) {
            resultToChange[0].selected = false;
        }

        setSearchResults(searchResultsCopy);
    }

    const keySelectHandler = (data) => {
        const changes = [{ key: "OrganismId", value: { Key: "OrganismId", value: data.Id}}, { key: "OrganismName", value: { Key: "OrganismName", value: data.Description  }  }];
        props.rightButtonClick(changes, "custom");
    }

    const genusText = TranslateTag("@GenGen@",props.language);
    const speciesText = TranslateTag("@GenSpeB@",props.language);
    const subspeciesText = TranslateTag("@GenSubA@",props.language);
    const serotypeText = TranslateTag("@GenSer@",props.language);
    const synonymText = TranslateTag("@OrgSys@",props.language);

    let dataToDisplay = [];
    let backButton = (null);
    if (searchResults != undefined) {
        dataToDisplay = searchResults.map((item) => {
            let returnValue = {
                Description: item.Description,
                Id: item.Id
            }

            if (item.Genus !== null) {
                returnValue.Genus = genusText + " : " + item.Genus;
            }
            if (item.Species !== null) {
                returnValue.Species = speciesText + " : " + item.Species;
            }
            if (item.SubSpecies !== null) {
                returnValue.SubSpecies = subspeciesText + " : " + item.SubSpecies;
            }
            if (item.Serotype !== null) {
                returnValue.Serotype = serotypeText + " : " + item.Serotype;
            }
            if (item.Synonyms !== null) {
                returnValue.Synonyms = synonymText + " : " + item.Synonyms;
            }

            if (item.selected) {
                returnValue.colour = "green";
            }
            return returnValue;
        })

        if (searchResults.length > 6) {
            backButton = (
                <div>
                    <div className="app-button organismlist-button">
                        <IconButton
                            styles={iconStyles}
                            iconProps={{iconName: 'Back'}}
                            onClick={props.leftButtonClick}
                        />
                    </div>
                </div>
            )
        }
    }

    // Experimental hot-key.
    document.onkeydown = function(e) {
        if (e.key === "ArrowLeft") {
            props.leftButtonClick();
        }
    };


    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            {backButton}
            <SimpleCard data={dataToDisplay} onClick={buttonClickHandler} onDoubleClick={() => nextClickHandler(false)} onKeySelect={keySelectHandler}></SimpleCard>
            <br />
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        </div>
    )
};

export default OrganismList;