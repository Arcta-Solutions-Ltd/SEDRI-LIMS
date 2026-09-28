import React, {useEffect, useState} from 'react';
import { connect } from 'react-redux';
import { DefaultButton, IconButton, TooltipHost } from '@fluentui/react';
import DiaryContents from './DiaryContents/DiaryContents';
import GetVisibleButtons from '../../../Utils/Forms/GetVisibleButtons';
import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import FormHandler from '../FormHandler/FormHandler';
import Post from '../../../Data/Post';
import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import './Diary.css';


const Diary = (props) => {

  const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
  const [formStartConfig, setFormStartConfig] = useState({});
  const [diaryConfig] = useState(props.diaries.filter((diary) => {
    return diary.Name.toLowerCase() === props.type.toLowerCase();
  })[0]);
  const [diaryData, setDiaryData] = useState();
  const [diaryVisibility, setDiaryVisibility] = useState(true);

  useEffect(() => {
    const criteria = { Name: diaryConfig.InitialQuery, Parameters: [{ Key: 'id', Value: props.itemId }] };
    Post('query/filteredget', criteria, diaryRetrievedSuccessfully, errorWhenRetrievingDiary);
  }, [props, diaryConfig])

  const refreshDiary = () => {
    const criteria = { Name: diaryConfig.InitialQuery, Parameters: [{ Key: 'id', Value: props.itemId }] };
    Post('query/filteredget', criteria, diaryRetrievedSuccessfully, errorWhenRetrievingDiary);
  }

  const setFormConfig = (button, recordId) => {
    setFormStartConfig({
      button: button, 
      id: recordId, 
      view: props.type, 
      refresh: refreshDiary,
      containerVisibility: setDiaryVisibility
     })
  }

  const buttonClickHandler = (button) => {
    setFormConfig(button, props.itemId);
  }

  const linkClickHandler = (recordId) => {
    setFormConfig({ UIEvent: 'monitoringjsonviewer'}, recordId);
  }

  const errorCloseHandler = () => {
    updateErrorStatus({visible: false, message: ''});
  }

  const diaryRetrievedSuccessfully = (data) => {
    TransformDatesInJson(data);
    setDiaryData(data);
  }

  const errorWhenRetrievingDiary = (response) => {
    updateErrorStatus({visible: true, message: response.data});
  }

  const visibleButtons = GetVisibleButtons(diaryConfig.Buttons, true, props.stateId);

  let footerButtons = (
    <div className='diary-buttons'>
        <div className='diary-button'>
            <DefaultButton
                text={TranslateTag("@GenExi@", props.language)}
                onClick={props.cancel}
                styles={{
                    root: { border: '0px', padding: '2px', backgroundColor: '#ddd'},
                    rootHovered: { backgroundColor: '#ccc' },
                    label: {  }}}
            />
        </div>
    </div>
  );

  const contentCss = diaryVisibility ? "" : "app-invisible"

  return (
      <div className="diary-page">
        <div className = {contentCss}>
          <div>
            <div className='diary-titlebar'>
                <TooltipHost
                    content={TranslateTag("@GenExi@", props.language)}
                    id={100}
                >
                    <IconButton
                        iconProps={{iconName: 'Back'}}
                        onClick={props.cancel}
                    />
                </TooltipHost>
                <div className='diary-title'>
                  {diaryConfig.Title}
                </div>
            </div>
            <div className='app-heading-text'>{diaryConfig.HeaderText}</div>
          </div>
          <div className="diary-contents">
            <DiaryContents data={diaryData} onClick={linkClickHandler}></DiaryContents>
          </div>
        </div>
        <FormHandler startConfig={formStartConfig}></FormHandler>
        <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        {footerButtons}
      </div>
  )
}

const mapStateToProps = state => {
  return {
      diaries: state.config.diaries,
      language: state.config.language,
  };
}

export default connect(mapStateToProps)(Diary);