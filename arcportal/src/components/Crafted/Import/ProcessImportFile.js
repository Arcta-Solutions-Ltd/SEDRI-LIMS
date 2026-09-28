import React, {useState} from 'react';
import ArcUpload from '../../Forms/ArcUpload/ArcUpload';
import { CompoundButton } from '@fluentui/react';
import Post from '../../../Data/Post';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const ProcessImportFile = (props) => {

    const [data, setData] = useState();
    const config = {Id: "uploaddata"}
    let dataArray = [];

    const changeHandler = (id, data) => {
        setData(data);
    }

    const loadClickHandler = () => {
        if (data !== undefined) {
            dataArray = data;
            const nextValuesToPost = dataArray.slice(0,50);
            Post('import/import', nextValuesToPost, dataPostedSuccessfully, errorWhenPostingData);
        }
    }

    const dataPostedSuccessfully = () => {
        dataArray.splice(0,50);
        loadClickHandler();
    }

    const errorWhenPostingData = () => {

    }

    return (
        <div className="app-crafted-content">
            <ArcUpload config={config} changeHandler={changeHandler}></ArcUpload>
            <div className="cultureorganismselection-buttons">
                    <CompoundButton primary onClick={() => loadClickHandler()} >
                        <div className="printpublish-button">
                            <br />
                            {TranslateTag("@SpeFul@", props.language)}
                        </div>
                    </CompoundButton>
            </div>
        </div>
    );
};
  
export default ProcessImportFile;