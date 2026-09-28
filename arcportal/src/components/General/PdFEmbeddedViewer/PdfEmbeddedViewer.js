import React from 'react';

const PdfEmbeddedViewer = (props) => {
    let pdfUrl;

    if (props.data !== undefined) {
        const pdfBlob = props.data.output('blob');
        pdfUrl = URL.createObjectURL(pdfBlob);
    }

    let pdfDisplay = null;
    if (pdfUrl !== undefined) {
        pdfDisplay = (
          <embed src={pdfUrl} type="application/pdf" width="100%" height="100%" />
        )
    }

    return (
        <div style={{ width: '100%', height: '100vh' }}>
            {pdfDisplay}
        </div>
    );
}
export default PdfEmbeddedViewer