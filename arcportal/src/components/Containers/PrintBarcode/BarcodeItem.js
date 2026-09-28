import React from 'react';
import { connect } from 'react-redux';
import './BarcodeItem.css';
import BarcodeLinear from './BarcodeLinear';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import QRCode from 'qrcode.react';

const BarcodeItem = (props) => {
    const { barcodeprintconfigs, language, config, record } =
        props;

    const printConfig =
        barcodeprintconfigs.find((cfg) => cfg.Name === config.type) || {};

    const barcode = printConfig.UseAccessionNumberForBarcode && config.selectedRecord.accessionnumber
        ? config.selectedRecord.accessionnumber
        : config.selectedRecord.barcode;

    const barcodeLabel = printConfig.DisplayCode ? barcode : ' ';

    const labelCaptions = printConfig.LabelCaptions || {};

    const displayFields = buildDisplayFields(
        labelCaptions,
        printConfig,
        record,
        language
    );

    return (
        <div className="barcodeitem-content" style={buildStyles(printConfig)}>
            <div
                className="barcodeitem-barcode"
                style={{ maxHeight: printConfig.MaxBarcodeHeight }}
            >
                <div
                    className={`barcodeitem-linear ${
                        printConfig.Linear ? '' : 'app-invisible'
                    }`}
                    style={{ margin: printConfig.BarcodePadding }}
                >
                    <BarcodeLinear
                        barcode={barcode}
                        barcodeLabel={barcodeLabel}
                        barcodeHeight={printConfig.LinearHeight}
                    />
                </div>

                <div
                    className={`barcodeitem-qr ${
                        printConfig.QR ? '' : 'app-invisible'
                    }`}
                    style={{ margin: printConfig.BarcodePadding }}
                >
                    <QRCode
                        value={barcode}
                        renderAs="svg"
                        size={printConfig.QRSize}
                    />
                </div>
            </div>

            {displayFields}
        </div>
    );
};

/** @typedef {Record<string,string>} LabelCaptionsMap */

/**
 * Builds label rows using server-provided captions on the barcode layout (not Redux forms).
 * @param {LabelCaptionsMap} labelCaptions Lowercase field ids to caption tokens (@...@) or literals.
 */
const buildDisplayFields = (labelCaptions, printConfig, record, language) => {
    const fields = String(printConfig.LabelFields || '')
        .split(',')
        .map((f) => f.trim())
        .filter(Boolean);

    return fields.map((fieldName) => {
            const fieldLookupKey = fieldName.toLowerCase();
            let captionToken =
                labelCaptions[fieldLookupKey] ||
                (fieldLookupKey === 'accessionnumber' ? '@SpeAcc@' : null);

            if (!captionToken) {
                return null;
            }

            const fieldId = fieldLookupKey;
            const titleText = captionToDisplayText(captionToken, language);

            return (
                    <div
                        key={fieldId}
                        className="barcodeitem-field"
                        style={{ fontSize: printConfig.FieldFontSize }}
                    >
                        {printConfig.SuppressFieldLabels !== true && (
                            <div
                                className="barcodeitem-fieldtitle"
                                style={{ width: printConfig.FieldNameWidth }}
                            >
                                {titleText}:
                            </div>
                        )}
                        <div className="barcodeitem-fieldvalue">
                            {record[fieldId] ?? 'Field not available!'}
                        </div>
                    </div>
            );
        }).filter(Boolean);
};

/**
 * Renders catalogue token as translated text when wrapped in {@code @...@}; otherwise returns raw caption.
 */
const captionToDisplayText = (token, language) => {
    if (!token || !String(token).trim()) {
        return '';
    }
    const t = String(token).trim();
    if (t.startsWith('@') && t.endsWith('@')) {
        return TranslateTag(t, language);
    }
    return t;
};

const buildStyles = (printConfig) => {
    return {
        flexDirection: printConfig.SideBySide ? 'row' : 'column',
        marginLeft: printConfig.LeftMargin,
        marginTop: printConfig.TopMargin,
        width: printConfig.ItemWidth,
        height: printConfig.ItemHeight,
        border: printConfig.Border ? '1px #ccc solid' : '0px',
    };
};

const mapStateToProps = (state) => {
    return {
        barcodeprintconfigs: state.config.barcodeprintconfigs,
        language: state.config.language,
    };
};

export default connect(mapStateToProps)(BarcodeItem);
