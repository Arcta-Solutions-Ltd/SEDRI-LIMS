import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Label, PrimaryButton, IconButton, ProgressIndicator, Text, TooltipHost } from '@fluentui/react';
import postMultipart from '../../../Data/PostMultipart';
import axios from 'axios';

const ArcUpload = (props) => {
    const inputRef = useRef(null);
    const [percent, setPercent] = useState(0);
    const [uploading, setUploading] = useState(false);
    const [fileName, setFileName] = useState('');
    const [fileSize, setFileSize] = useState(0);
    const [error, setError] = useState('');
    const [lastId, setLastId] = useState(undefined);
    const isMulti = props?.config?.MultiSelect === true;
    const [items, setItems] = useState([]); // entries: {uploadId,name,size,percent,status:'Queued'|'Uploading'|'Complete'|'Failed', fileId?, fileRef?}

    const contentTypes = Array.isArray(props?.config?.ContentTypes) ? props.config.ContentTypes : undefined;
    const accept = props.accept || props?.config?.Accept || (contentTypes && contentTypes.length > 0 ? contentTypes.join(',') : 'image/*');
    const allowedContentTypes = contentTypes;
    const maxSize = props.maxSize || props?.config?.MaxSize || 10 * 1024 * 1024; // 10MB default
    const category = props?.config?.Category || 'image';

    const prettySize = useMemo(() => {
        if (!fileSize) return '';
        const units = ['B', 'KB', 'MB', 'GB'];
        let s = fileSize; let i = 0;
        while (s >= 1024 && i < units.length - 1) { s /= 1024; i++; }
        return `${s.toFixed(1)} ${units[i]}`;
    }, [fileSize]);

    // Download helper
    const downloadById = useCallback(async (id, suggestedName) => {
        try {
            const token = localStorage.getItem('arctoken');
            const resp = await axios.get(`file/${id}/download`, {
                responseType: 'blob',
                headers: token ? { Authorization: `Bearer ${token}` } : {}
            });
            const ct = (resp.headers['content-type'] || '').toLowerCase();
            const blob = new Blob([resp.data], { type: ct || 'application/octet-stream' });
            const url = URL.createObjectURL(blob);
            // Try to parse filename from Content-Disposition
            const cd = resp.headers['content-disposition'] || '';
            let filename = suggestedName || 'download';
            const m = /filename\*=UTF-8''([^;]+)|filename="?([^";]+)"?/i.exec(cd);
            if (m) filename = decodeURIComponent(m[1] || m[2] || filename);
            if (ct.startsWith('image/') || ct === 'application/pdf') {
                window.open(url, '_blank');
                setTimeout(() => URL.revokeObjectURL(url), 60000);
            } else {
                const a = document.createElement('a');
                a.href = url; a.download = filename;
                document.body.appendChild(a); a.click(); a.remove();
                URL.revokeObjectURL(url);
            }
        } catch (e) {
            setError('Download failed.');
        }
    }, []);

    // Hydrate from initial value (comma-separated ids) on mount/prop change
    useEffect(() => {
        const raw = props?.config?.value;
        if (raw === undefined || raw === null) {
            // external clear
            setItems([]);
            return;
        }
        const csv = String(raw);
        const ids = csv.split(',').map(s => parseInt(s.trim(), 10)).filter(n => Number.isFinite(n) && n > 0);
        if (ids.length === 0) {
            setItems([]);
            return;
        }
        const uniqueIds = Array.from(new Set(ids));
        const effectiveIds = isMulti ? uniqueIds : [uniqueIds[0]];

        let cancelled = false;
        const token = localStorage.getItem('arctoken');
        const fetchOne = async (id) => {
            try {
                const resp = await axios.get(`file/${id}/metadata`, { headers: token ? { Authorization: `Bearer ${token}` } : {} });
                const meta = resp?.data || {};
                return {
                    uploadId: `seed-${id}`,
                    name: meta.originalFileName || `file-${id}`,
                    size: typeof meta.fileSize === 'number' ? meta.fileSize : 0,
                    percent: 100,
                    status: 'Complete',
                    fileId: id
                };
            } catch {
                return {
                    uploadId: `seed-${id}`,
                    name: `file-${id}`,
                    size: 0,
                    percent: 0,
                    status: 'Failed',
                    fileId: id
                };
            }
        };

        (async () => {
            const results = await Promise.all(effectiveIds.map(fetchOne));
            if (cancelled) return;
            setItems(results);
        })();

        return () => { cancelled = true; };
    }, [props?.config?.value, isMulti]);

    const chooseFile = useCallback(() => {
        if (inputRef.current) inputRef.current.click();
    }, []);

    const clearFile = useCallback(() => {
        setPercent(0);
        setUploading(false);
        setFileName('');
        setFileSize(0);
        setError('');
        if (typeof props.onClear === 'function') props.onClear();
        // Emit a cleared value (undefined or 0) to match other controls semantics
        props.changeHandler(props.config.Id, undefined);
    }, [props]);

    const doUpload = useCallback(async (file) => {
        setError('');
        if (!file) return;
        if (file.size > maxSize) {
            setError('File is too large.');
            return;
        }
        // Block executables by extension
        const nameLower = (file.name || '').toLowerCase();
        const blockedExt = ['.exe','.msi','.bat','.cmd','.ps1','.sh','.dll','.com','.scr'];
        if (blockedExt.some(ext => nameLower.endsWith(ext))) {
            setError('This file type is not permitted.');
            return;
        }
        // Enforce ContentTypes whitelist if provided
        if (allowedContentTypes && allowedContentTypes.length > 0) {
            const mime = (file.type || '').toLowerCase();
            const extOk = allowedContentTypes.some(t => t.startsWith('.') && nameLower.endsWith(t.toLowerCase()));
            const mimeOk = allowedContentTypes.some(t => !t.startsWith('.') && mime === t.toLowerCase());
            if (!extOk && !mimeOk) {
                setError('This file type is not permitted.');
                return;
            }
        }
        if (!isMulti) {
            setUploading(true);
            setPercent(0);
            setFileName(file.name);
            setFileSize(file.size);
            // Seed unified list for single mode so the row appears with progress/actions
            setItems([{ uploadId: `${Date.now()}-${Math.random().toString(36).slice(2)}`, name: file.name, size: file.size, percent: 0, status: 'Uploading', fileRef: file }]);
        }

        const formData = new FormData();
        formData.append('file', file);
        formData.append('category', category);

        if (!isMulti) {
            // Single-file flow updates first item in unified list
            postMultipart(
                'file/upload',
                formData,
                (data) => {
                    const imageId = (data && typeof data.id === 'number') ? data.id : 0;
                    setItems(prev => {
                        const next = [...prev];
                        if (next[0]) next[0] = { ...next[0], percent: 100, status: 'Complete', fileId: imageId };
                        return next;
                    });
                    props.changeHandler(props.config.Id, imageId);
                    setLastId(imageId);
                    setUploading(false);
                },
                () => {
                    setError('Upload failed. Please try again.');
                    setPercent(0);
                    setUploading(false);
                    setItems(prev => {
                        const next = [...prev];
                        if (next[0]) next[0] = { ...next[0], percent: 0, status: 'Failed' };
                        return next;
                    });
                    setLastId(undefined);
                    props.changeHandler(props.config.Id, undefined);
                },
                undefined,
                (loaded, total, p) => {
                    setPercent(p);
                    setItems(prev => {
                        const next = [...prev];
                        if (next[0] && next[0].status === 'Uploading') next[0] = { ...next[0], percent: p };
                        return next;
                    });
                }
            );
            return;
        }

        // Multi-file flow: add item and upload with per-item progress using a stable id
        const uploadId = `${Date.now()}-${Math.random().toString(36).slice(2)}`;
        setItems((prev) => ([...prev, { uploadId, name: file.name, size: file.size, percent: 0, status: 'Uploading', fileRef: file }]));
        postMultipart(
            'file/upload',
            formData,
            (data) => {
                const imageId = (data && typeof data.id === 'number') ? data.id : 0;
                setItems((prev) => {
                    const next = prev.map(it => it.uploadId === uploadId ? { ...it, percent: 100, status: 'Complete', fileId: imageId } : it);
                    const ids = next.filter(x => x.status === 'Complete' && typeof x.fileId === 'number').map(x => x.fileId);
                    props.changeHandler(props.config.Id, ids.length > 0 ? (isMulti ? ids.join(',') : ids[0]) : undefined);
                    return next;
                });
            },
            () => {
                setItems((prev) => {
                    const next = prev.map(it => it.uploadId === uploadId ? { ...it, percent: 0, status: 'Failed' } : it);
                    const ids = next.filter(x => x.status === 'Complete' && typeof x.fileId === 'number').map(x => x.fileId);
                    props.changeHandler(props.config.Id, ids.length > 0 ? (isMulti ? ids.join(',') : ids[0]) : undefined);
                    return next;
                });
            },
            undefined,
            (loaded, total, p) => {
                setItems((prev) => prev.map(it => it.uploadId === uploadId ? { ...it, percent: p, status: 'Uploading' } : it));
            }
        );
    }, [category, isMulti, maxSize, props]);

    const onInputChange = useCallback((e) => {
        if (!e.target.files || e.target.files.length === 0) return;
            if (isMulti) {
            const files = Array.from(e.target.files);
            files.forEach((f) => void doUpload(f));
        } else {
            const file = e.target.files[0];
            void doUpload(file);
        }
    }, [doUpload, isMulti]);

    // Drag & Drop handlers
    const [isDragOver, setIsDragOver] = useState(false);
    const onDragOver = useCallback((e) => {
        e.preventDefault();
        setIsDragOver(true);
    }, []);
    const onDragLeave = useCallback((e) => {
        e.preventDefault();
        setIsDragOver(false);
    }, []);
    const onDrop = useCallback((e) => {
        e.preventDefault();
        setIsDragOver(false);
        if (e.dataTransfer && e.dataTransfer.files && e.dataTransfer.files.length > 0) {
            if (isMulti) {
                const files = Array.from(e.dataTransfer.files);
                files.forEach((f) => void doUpload(f));
            } else {
                const file = e.dataTransfer.files[0];
                void doUpload(file);
            }
        }
    }, [doUpload, isMulti]);

    const dropZoneStyle = useMemo(() => ({
        border: `2px ${isDragOver ? 'solid' : 'dashed'} #999`,
        borderRadius: 6,
        padding: 16,
        background: isDragOver ? 'rgba(0,0,0,0.04)' : 'transparent',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        gap: 12,
        cursor: uploading ? 'not-allowed' : 'pointer'
    }), [isDragOver, uploading]);

    const fileRow = (
        <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginTop: 8 }}>
            <TooltipHost content={fileName}>
                <Text block>{fileName || 'No file selected'}</Text>
            </TooltipHost>
            {fileSize > 0 && <Text variant="small" styles={{ root: { color: '#666' } }}>{prettySize}</Text>}
            {fileName && (
                <IconButton iconProps={{ iconName: 'Cancel' }} ariaLabel="Remove file" onClick={clearFile} disabled={uploading} />
            )}
        </div>
    );

    return (
        <React.Fragment>
            <Label>{props.config.Label}</Label>

            {/* Hidden file input */}
            <input
                ref={inputRef}
                type="file"
                accept={accept}
                multiple={isMulti}
                onChange={onInputChange}
                data-testid="arcupload-file-input"
                style={{ display: 'none' }}
            />

            {/* Drop zone card */}
            <div
                role="button"
                tabIndex={0}
                onClick={uploading ? undefined : chooseFile}
                onKeyDown={(e) => { if (!uploading && (e.key === 'Enter' || e.key === ' ')) chooseFile(); }}
                onDragOver={onDragOver}
                onDragLeave={onDragLeave}
                onDrop={onDrop}
                aria-disabled={uploading}
                style={dropZoneStyle}
            >
                <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                    <i className="ms-Icon ms-Icon--CloudUpload" aria-hidden="true" />
                    <Text>Drag & drop an image here or</Text>
                </div>
                <PrimaryButton text={uploading ? 'Uploading…' : 'Choose file'} disabled={uploading} onClick={(e) => { e.stopPropagation(); chooseFile(); }} />
            </div>

            {items.length > 0 && (
                <div style={{ marginTop: 8, display: 'flex', flexDirection: 'column', gap: 6 }}>
                    {items.map((it, i) => (
                        <div key={`${it.uploadId || it.name}-${i}`} style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                            <TooltipHost content={it.name}><Text block>{it.name}</Text></TooltipHost>
                            <Text variant="small" styles={{ root: { color: '#666' } }}>{it.size ? `${(it.size/1024/1024).toFixed(1)} MB` : ''}</Text>
                            {it.status === 'Uploading' && (
                                <div style={{ flex: 1 }}>
                                    <ProgressIndicator percentComplete={(it.percent || 0) / 100} label={`${it.percent || 0}%`} />
                                </div>
                            )}
                            {it.status === 'Failed' && <Text variant="small" styles={{ root: { color: '#a4262c' } }}>Failed</Text>}
                            {(it.status === 'Complete' && typeof it.fileId === 'number') && (
                                <IconButton iconProps={{ iconName: 'Download' }} ariaLabel="Download" title="Download" onClick={() => downloadById(it.fileId, it.name)} />
                            )}
                            {it.status !== 'Uploading' && (
                                <IconButton iconProps={{ iconName: 'Cancel' }} ariaLabel="Remove" onClick={() => {
                                    setItems((prev) => {
                                        const next = prev.filter((_, idx) => idx !== i);
                                        const ids = next.filter(x => x.status === 'Complete' && typeof x.fileId === 'number').map(x => x.fileId);
                                        // single: emit undefined when list cleared; multi: emit comma-separated ids
                                        props.changeHandler(props.config.Id, isMulti ? (ids.length > 0 ? ids.join(',') : undefined) : undefined);
                                        if (!isMulti && inputRef.current) inputRef.current.value = '';
                                        return next;
                                    });
                                }} />
                            )}
                        </div>
                    ))}
                </div>
            )}

            {uploading && (
                <div style={{ marginTop: 8 }} aria-live="polite">
                    <ProgressIndicator percentComplete={percent / 100} label={`Uploading… ${percent}%`} description={fileName ? `${fileName} • ${prettySize}` : undefined} />
                </div>
            )}

            {!!error && (
                <Text variant="small" styles={{ root: { color: '#a4262c', marginTop: 4 } }}>{error}</Text>
            )}
        </React.Fragment>
    )
}

export default ArcUpload;