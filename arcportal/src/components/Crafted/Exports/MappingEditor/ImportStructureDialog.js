import React, { useState, useEffect, useRef } from 'react';
import { Dialog, DialogType, DialogFooter } from '@fluentui/react/lib/Dialog';
import { PrimaryButton, DefaultButton, TextField, ChoiceGroup, MessageBar, MessageBarType } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { parseStructure } from './structureImport';

/**
 * Dialog used to import an existing JSON or XML document as the starting
 * structure for the mapping editor. The user can either choose a file or paste
 * the content directly, and optionally force the format (otherwise it is
 * auto-detected). The parsed structure is returned to the editor via onConfirm;
 * because importing replaces the current tree, a warning is shown when the
 * editor already holds a non-empty structure.
 *
 * @param {object} props
 * @param {boolean} props.visible
 * @param {boolean} props.hasExistingContent - true when the current tree already has children.
 * @param {Array}   props.language
 * @param {function(tree:object):void} props.onConfirm - receives the parsed canonical tree.
 * @param {function():void} props.onDismiss
 */
const ImportStructureDialog = (props) => {
    const [text, setText] = useState('');
    const [format, setFormat] = useState('auto');
    const [fileName, setFileName] = useState('');
    const [error, setError] = useState('');
    const fileInputRef = useRef(null);

    useEffect(() => {
        if (props.visible) {
            setText('');
            setFormat('auto');
            setFileName('');
            setError('');
            if (fileInputRef.current) fileInputRef.current.value = '';
        }
    }, [props.visible]);

    const formatOptions = [
        { key: 'auto', text: TranslateTag('@ExpProMapImportAuto@', props.language) || 'Auto-detect' },
        { key: 'json', text: TranslateTag('@ExpProMapJson@', props.language) || 'JSON' },
        { key: 'xml', text: TranslateTag('@ExpProMapXml@', props.language) || 'XML' }
    ];

    const onPickFile = () => {
        if (fileInputRef.current) fileInputRef.current.click();
    };

    const onFileChange = (e) => {
        const file = e.target.files && e.target.files[0];
        if (!file) return;
        setFileName(file.name);
        setError('');
        const reader = new FileReader();
        reader.onload = () => {
            setText(typeof reader.result === 'string' ? reader.result : '');
            const lower = file.name.toLowerCase();
            if (lower.endsWith('.json')) setFormat('json');
            else if (lower.endsWith('.xml')) setFormat('xml');
        };
        reader.onerror = () => {
            setError(TranslateTag('@ExpProMapImportReadErr@', props.language) || 'The selected file could not be read.');
        };
        reader.readAsText(file);
    };

    const handleConfirm = () => {
        let tree;
        try {
            tree = parseStructure(text, format);
        } catch (ex) {
            setError(ex && ex.message ? ex.message : String(ex));
            return;
        }
        props.onConfirm(tree);
    };

    return (
        <Dialog
            hidden={!props.visible}
            onDismiss={props.onDismiss}
            minWidth={520}
            dialogContentProps={{ type: DialogType.normal, title: TranslateTag('@ExpProMapImport@', props.language) || 'Import structure' }}
            modalProps={{ isBlocking: true }}
        >
            <div className="mappingeditor-import-intro">
                {TranslateTag('@ExpProMapImportIntro@', props.language)
                    || 'Import an existing JSON or XML document to use as a starting structure. Choose a file or paste the content below, then map profile fields onto the imported attributes.'}
            </div>

            {props.hasExistingContent && (
                <MessageBar messageBarType={MessageBarType.warning} isMultiline={true} styles={{ root: { marginTop: 8, marginBottom: 4 } }}>
                    {TranslateTag('@ExpProMapImportOverwrite@', props.language)
                        || 'Importing will replace the structure you have already built.'}
                </MessageBar>
            )}

            <ChoiceGroup
                label={TranslateTag('@ExpProMapImportFmt@', props.language) || 'Format'}
                options={formatOptions}
                selectedKey={format}
                onChange={(_, opt) => setFormat(opt?.key ?? 'auto')}
                styles={{ flexContainer: { display: 'flex', gap: 12 } }}
            />

            <div className="mappingeditor-import-file">
                <DefaultButton
                    id="mapping-import-choose-file"
                    iconProps={{ iconName: 'Upload' }}
                    text={TranslateTag('@ExpProMapImportChoose@', props.language) || 'Choose file'}
                    onClick={onPickFile}
                />
                {fileName && <span className="mappingeditor-import-filename">{fileName}</span>}
                <input
                    ref={fileInputRef}
                    type="file"
                    accept=".json,.xml,.txt,application/json,text/xml,application/xml"
                    style={{ display: 'none' }}
                    onChange={onFileChange}
                />
            </div>

            <TextField
                id="mapping-import-text"
                label={TranslateTag('@ExpProMapImportPaste@', props.language) || 'Or paste content'}
                multiline
                rows={10}
                value={text}
                onChange={(_, v) => { setText(v ?? ''); setError(''); }}
                styles={{ field: { fontFamily: 'Consolas, "Courier New", monospace', fontSize: 12 } }}
            />

            {error && (
                <MessageBar messageBarType={MessageBarType.error} isMultiline={true} styles={{ root: { marginTop: 8 } }}>
                    {error}
                </MessageBar>
            )}

            <DialogFooter>
                <PrimaryButton
                    id="mapping-import-confirm"
                    onClick={handleConfirm}
                    text={props.hasExistingContent
                        ? (TranslateTag('@ExpProMapImportReplace@', props.language) || 'Replace and import')
                        : (TranslateTag('@ExpProMapImportDo@', props.language) || 'Import')}
                />
                <DefaultButton id="mapping-import-cancel" onClick={props.onDismiss} text={TranslateTag('@GenCanA@', props.language) || 'Cancel'} />
            </DialogFooter>
        </Dialog>
    );
};

export default ImportStructureDialog;
