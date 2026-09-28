import React from 'react';
import ReactDOM from 'react-dom';
import {createStore, combineReducers} from 'redux';
import { Provider } from 'react-redux';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import configReducer from './store/Reducers/ConfigReducer';
import displayReducer from './store/Reducers/DisplayReducer';
import axios from 'axios';
import { composeWithDevTools } from '@redux-devtools/extension';
import  { setLayerHostSelector }  from '@fluentui/react';

axios.defaults.baseURL = process.env.REACT_APP_LIMS_API_BASE_URL;
// WAPT-007: Enable credentials for CSRF protection
axios.defaults.withCredentials = true;

setLayerHostSelector('body');

const rootReducer = combineReducers({
  config: configReducer,
  display: displayReducer
});

const store = createStore(rootReducer, composeWithDevTools());

const container = document.getElementById('root')
const root = ReactDOM.createRoot(container)
root.render(
   <Provider store={store}>
     <App />
   </Provider>
)


