import React, { useEffect, useState } from 'react';
import { Panel, PanelType, PrimaryButton, DefaultButton } from '@fluentui/react';

import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { hasOverrideReason } from '../astSusceptibilityOverrideUtils';

import '../../../Forms/SinglePageEntry/SinglePageEntry.css';
import '../../../Forms/SinglePage/SinglePage.css';
import '../../../Forms/FormColumn/FormColumn.css';
import '../AST.css';

/**
 * Side panel for entering or editing manual susceptibility override audit information.
 * Layout matches SinglePagePanel / SinglePageEntry used elsewhere in the app.
 */
const ASTSusceptibilityOverridePanel = (props) => {
    const {
        visible,
        onDismiss,
        onSave,
        language,
        auditRequired,
        pendingSusceptibilityLabel,
        overriddenFromLabel,
        initialCannedCommentId,
        initialFreeText,
        cannedOptions,
        editMode,
    } = props;

    const [cannedId, setCannedId] = useState('');
    const [freeText, setFreeText] = useState('');

    useEffect(() => {
        if (visible) {
            setCannedId(initialCannedCommentId ? String(initialCannedCommentId) : '');
            setFreeText(initialFreeText ?? '');
        }
    }, [visible, initialCannedCommentId, initialFreeText]);

    const pageTitle = TranslateTag('@AstSusPan@', language);
    const pageDescription = TranslateTag('@AstSusPanDesc@', language);
    const newSusceptibilityLabel = TranslateTag('@AstSusNew@', language);
    const overriddenFromTag = TranslateTag('@AstSusOverFrom@', language);
    const cannedLabel = TranslateTag('@AstSusCan@', language);
    const freeTextLabel = TranslateTag('@AstSusFree@', language);
    const enterPlaceholder = TranslateTag('@GenEntA@', language);

    const cannedConfig = {
        Id: 'ast-susceptibility-override-canned-input',
        Type: 'combobox',
        Label: cannedLabel,
        Placeholder: enterPlaceholder,
        value: cannedId,
        Options: (cannedOptions ?? []).map((o) => ({ key: String(o.key), text: o.text })),
    };

    const freeTextConfig = {
        Id: 'ast-susceptibility-override-freetext',
        Type: 'multiline',
        Label: freeTextLabel,
        Placeholder: enterPlaceholder,
        value: freeText,
    };

    const susceptibilityConfig = {
        Id: 'ast-susceptibility-override-susceptibility',
        Type: 'text',
        Label: newSusceptibilityLabel,
        value: pendingSusceptibilityLabel ?? '',
    };

    const overriddenFromConfig = {
        Id: 'ast-susceptibility-override-overridden-from',
        Type: 'text',
        Label: overriddenFromTag,
        value: overriddenFromLabel ?? '',
    };

    const handleSave = () => {
        const payload = {
            CannedCommentId: cannedId ? Number(cannedId) : 0,
            FreeTextComment: freeText ?? '',
        };
        if (auditRequired && !hasOverrideReason(payload)) {
            return;
        }
        onSave(payload);
    };

    const showCannedField = editMode || !freeText.trim() || Boolean(cannedId);
    const showFreeTextField = editMode || !cannedId || Boolean(freeText.trim());
    const panelFieldKey = `${visible}-${initialCannedCommentId}-${initialFreeText ?? ''}-${editMode ? 'edit' : 'new'}`;

    return (
        <Panel
            isOpen={visible}
            onDismiss={onDismiss}
            type={PanelType.medium}
            hasCloseButton={false}
            isLightDismiss
        >
            <div className="singlepage-content">
                <div className="singlepageentry-content">
                    <div className="singlepageentry-title">{pageTitle}</div>
                    <div className="singlepageentry-headertext">
                        <TextDisplay text={pageDescription} />
                    </div>
                    <div className="formcolumn-2col">
                        {pendingSusceptibilityLabel && newSusceptibilityLabel ? (
                            <SingleLineField
                                key={`${panelFieldKey}-susceptibility`}
                                config={susceptibilityConfig}
                                changeHandler={() => {}}
                            />
                        ) : null}
                        {overriddenFromLabel && overriddenFromTag ? (
                            <SingleLineField
                                key={`${panelFieldKey}-overridden-from`}
                                config={overriddenFromConfig}
                                changeHandler={() => {}}
                            />
                        ) : null}
                        {showCannedField ? (
                            <SingleLineField
                                key={`${panelFieldKey}-canned`}
                                config={cannedConfig}
                                changeHandler={(id, val) => {
                                    setCannedId(val ?? '');
                                    if (val) {
                                        setFreeText('');
                                    }
                                }}
                            />
                        ) : null}
                        {showFreeTextField ? (
                            <SingleLineField
                                key={`${panelFieldKey}-freetext`}
                                config={freeTextConfig}
                                changeHandler={(id, val) => {
                                    setFreeText(val ?? '');
                                    if (val?.trim()) {
                                        setCannedId('');
                                    }
                                }}
                            />
                        ) : null}
                    </div>
                </div>
                <br />
                <div className="singlepage-navigation">
                    <div className="singlepage-righthand-buttons">
                        <div className="app-button">
                            <DefaultButton
                                id="ast-susceptibility-override-cancel"
                                text={TranslateTag('@GenCan@', language)}
                                onClick={onDismiss}
                            />
                        </div>
                        <div className="app-button">
                            <PrimaryButton
                                id="ast-susceptibility-override-save"
                                text={TranslateTag('@GenSav@', language)}
                                onClick={handleSave}
                            />
                        </div>
                    </div>
                </div>
            </div>
        </Panel>
    );
};

export default ASTSusceptibilityOverridePanel;
