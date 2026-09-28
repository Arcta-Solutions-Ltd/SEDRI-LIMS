import React from 'react';
import RecordReportComponentFactory from './RecordReportComponentFactory';

export class RecordReport extends React.PureComponent {

    constructor(props) {
        super(props);
        this.props = props;
    };

    render() {

        this.contents =  this.props.config.Sections.map((section) => (
            <RecordReportComponentFactory config={section} data={this.props.data}></RecordReportComponentFactory>
        ));

        return (
            <div>
                {this.contents}
            </div>
        );
    }
}

