import React, {useState} from 'react';
import { Modal } from '@fluentui/react';
import Wizard from '../Wizard/Wizard';


const WizardModal = (props) => {

    const [isFormOpen, updateIsFormOpen] = useState(props.isFormOpen);
 
    const closeWindowHandler = () => {
        updateIsFormOpen(false);
        props.closeWindowHandler();
    }

    return (

        <Modal
            titleAriaId={"InitialSystemConfiguration"}y
            isOpen={isFormOpen}
            isBlocking={true}
        >
            <Wizard config={props.config} closeWindowHandler={closeWindowHandler}></Wizard>

        </Modal>
    )
};
  
export default WizardModal;

