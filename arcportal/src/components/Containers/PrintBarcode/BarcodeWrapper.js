import React from 'react';
import BarcodePage from './BarcodePage';

export class BarcodeWrapper extends React.PureComponent {
    componentDidMount() {
        setTimeout(() => {
            this.forceUpdate();
        }, 100);
    }

    render() {
        return (
            <div className="barcode-wrapper">
                <BarcodePage config={this.props.config} record={this.props.record}></BarcodePage>
            </div>
        );
    }
}

export default BarcodeWrapper;
