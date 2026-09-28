// import React from 'react';
// import './Confirmation.css';
// import { PrimaryButton, DefaultButton } from 'office-ui-fabric-react';
// import { Dialog, DialogType, DialogFooter } from 'office-ui-fabric-react/lib/Dialog';
// import { ContextualMenu } from 'office-ui-fabric-react/lib/ContextualMenu';


// const Confirmation = (props) => {

//     const dialogContentProps = {
//         type: DialogType.largeHeader,
//         title: props.config.Title,
//         subText: props.config.Text
//     };

//     const dragOptions = {
//         moveMenuItemText: 'Move',
//         menu: ContextualMenu,
//         keepInBounds: true,
//     };

//     const modelProps = {
//         isBlocking: true,
//         isModeless: true,
//         dragOptions: dragOptions
//     };

//     return (
//         <Dialog
//             hidden={false}
//             dialogContentProps={dialogContentProps}
//             modalProps={modelProps}
//             minWidth={props.config.Width}
//         >
//             <DialogFooter>
//                 <DefaultButton onClick={props.close} text="Cancel" />
//                 <PrimaryButton onClick={props.close} text="Ok" />
//             </DialogFooter>
//         </Dialog>
//     )
// };
  
// export default Confirmation;