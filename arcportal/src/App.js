import React from 'react';
import './App.css';
import Layout from './components/Layout/Layout';
import GlobalErrorHandler from './Utils/ErrorHandling/GlobalErrorHandler';
import { initializeIcons } from '@fluentui/react';

initializeIcons('/static/fonts/');

function App() {
    //GlobalErrorHandler();
    return (
        <div className="App">
            <Layout></Layout>
        </div>
    );
}

export default App;
