import React from 'react';

//import sedriclogo_pure from '../../../assets/images/sedriclogo_pure.png';
//import sedri_lims_logo3 from '../../../assets/images/sedri-lims-logo3.png';
// import sedri_lims_logo3 from '../../../assets/images/sedri-lims-logo-OPNEUMO.png';
import sedri_lims_logo3 from '../../../assets/images/SEDRILIMS Logo.png';

import './Logo.css'

const logo = () => {
    return (
        <div className="logo-content">
            {/* <img src={sedriclogo_pure} alt="Sedric Logo" height="100%"/> */}
            {/* <img src={sedri_lims_logo3} alt="Sedric Logo" height="100%"/> */}
            <img src={sedri_lims_logo3} alt="Sedric Logo" height="100%"/>
        </div>
    )
};

export default logo;
