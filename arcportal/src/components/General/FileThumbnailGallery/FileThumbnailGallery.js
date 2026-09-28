import React, { useCallback, useEffect, useRef, useState } from 'react';
import axios from 'axios';
import * as pdfjsLib from 'pdfjs-dist';
import 'pdfjs-dist/build/pdf.worker';
import { Icon } from '@fluentui/react';
import './FileThumbnailGallery.css';

/**
 * Displays a gallery of file thumbnails for uploaded files. Supports images (inline thumbnail),
 * PDFs (first-page thumbnail), and other types (file icon + filename). Clicking a thumbnail
 * opens the file in a new browser tab.
 *
 * @param {Object} props - Component props
 * @param {string|number|Array<number>} props.fileIds - Comma-separated ids, single id, or array of file attachment ids
 * @param {string} [props.language] - Language for translations (reserved for future use)
 * @param {boolean} [props.readOnly=true] - Whether the gallery is read-only (always true for view mode)
 */
const FileThumbnailGallery = (props) => {
    const [items, setItems] = useState([]);
    const [loading, setLoading] = useState(true);
    const itemsRef = useRef([]);

    const parseIds = useCallback((raw) => {
        if (raw === undefined || raw === null) return [];
        if (Array.isArray(raw)) return raw.filter((n) => Number.isFinite(n) && n > 0);
        const csv = String(raw);
        return csv.split(',').map((s) => parseInt(s.trim(), 10)).filter((n) => Number.isFinite(n) && n > 0);
    }, []);

    const openInBrowser = useCallback(async (id, suggestedName) => {
        try {
            const token = localStorage.getItem('arctoken');
            const resp = await axios.get(`file/${id}/download`, {
                responseType: 'blob',
                headers: token ? { Authorization: `Bearer ${token}` } : {}
            });
            const ct = (resp.headers['content-type'] || '').toLowerCase();
            const blob = new Blob([resp.data], { type: ct || 'application/octet-stream' });
            const url = URL.createObjectURL(blob);
            if (ct.startsWith('image/') || ct === 'application/pdf') {
                window.open(url, '_blank');
                setTimeout(() => URL.revokeObjectURL(url), 60000);
            } else {
                const cd = resp.headers['content-disposition'] || '';
                let filename = suggestedName || 'download';
                const m = /filename\*=UTF-8''([^;]+)|filename="?([^";]+)"?/i.exec(cd);
                if (m) filename = decodeURIComponent(m[1] || m[2] || filename);
                const a = document.createElement('a');
                a.href = url;
                a.download = filename;
                document.body.appendChild(a);
                a.click();
                a.remove();
                URL.revokeObjectURL(url);
            }
        } catch (e) {
            console.error('Failed to open file', e);
        }
    }, []);

    useEffect(() => {
        const ids = parseIds(props.fileIds);
        if (ids.length === 0) {
            setItems([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        const token = localStorage.getItem('arctoken');

        const fetchMetadata = async (id) => {
            try {
                const resp = await axios.get(`file/${id}/metadata`, {
                    headers: token ? { Authorization: `Bearer ${token}` } : {}
                });
                const meta = resp?.data || {};
                return {
                    id,
                    name: meta.originalFileName || `file-${id}`,
                    contentType: (meta.contentType || '').toLowerCase(),
                    fileSize: meta.fileSize
                };
            } catch {
                return { id, name: `file-${id}`, contentType: '', fileSize: 0 };
            }
        };

        const loadThumbnail = async (meta) => {
            if (meta.contentType.startsWith('image/')) {
                try {
                    const resp = await axios.get(`file/${meta.id}/download`, {
                        responseType: 'blob',
                        headers: token ? { Authorization: `Bearer ${token}` } : {}
                    });
                    const url = URL.createObjectURL(resp.data);
                    return { ...meta, thumbnail: url, thumbnailType: 'image' };
                } catch {
                    return { ...meta, thumbnail: null, thumbnailType: 'icon' };
                }
            }
            if (meta.contentType === 'application/pdf') {
                try {
                    const resp = await axios.get(`file/${meta.id}/download`, {
                        responseType: 'blob',
                        headers: token ? { Authorization: `Bearer ${token}` } : {}
                    });
                    const blob = resp.data;
                    const arrayBuffer = await blob.arrayBuffer();
                    pdfjsLib.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.mjs';
                    const pdf = await pdfjsLib.getDocument(arrayBuffer).promise;
                    const page = await pdf.getPage(1);
                    const canvas = document.createElement('canvas');
                    const viewport = page.getViewport({ scale: 0.15 });
                    canvas.width = viewport.width;
                    canvas.height = viewport.height;
                    await page.render({ canvasContext: canvas.getContext('2d'), viewport }).promise;
                    const dataUrl = canvas.toDataURL('image/png');
                    return { ...meta, thumbnail: dataUrl, thumbnailType: 'pdf' };
                } catch {
                    return { ...meta, thumbnail: null, thumbnailType: 'icon' };
                }
            }
            return { ...meta, thumbnail: null, thumbnailType: 'icon' };
        };

        (async () => {
            const metas = await Promise.all(ids.map(fetchMetadata));
            if (cancelled) return;
            const withThumbs = await Promise.all(metas.map(loadThumbnail));
            if (cancelled) return;
            itemsRef.current = withThumbs;
            setItems(withThumbs);
            setLoading(false);
        })();

        return () => {
            cancelled = true;
            itemsRef.current.forEach((p) => {
                if (p.thumbnailType === 'image' && p.thumbnail) URL.revokeObjectURL(p.thumbnail);
            });
            itemsRef.current = [];
        };
    }, [props.fileIds, parseIds]);

    if (loading) {
        return <div className="filethumbnailgallery-loading">...</div>;
    }

    if (items.length === 0) {
        return null;
    }

    return (
        <div className="filethumbnailgallery-grid">
            {items.map((item) => (
                <div
                    key={item.id}
                    className="filethumbnailgallery-item"
                    onClick={() => openInBrowser(item.id, item.name)}
                    role="button"
                    tabIndex={0}
                    onKeyDown={(e) => e.key === 'Enter' && openInBrowser(item.id, item.name)}
                    aria-label={`View ${item.name}`}
                >
                    {item.thumbnailType === 'image' && item.thumbnail && (
                        <img src={item.thumbnail} alt={item.name} className="filethumbnailgallery-thumb" />
                    )}
                    {item.thumbnailType === 'pdf' && item.thumbnail && (
                        <img src={item.thumbnail} alt={item.name} className="filethumbnailgallery-thumb" />
                    )}
                    {(item.thumbnailType === 'icon' || !item.thumbnail) && (
                        <div className="filethumbnailgallery-icon">
                            <Icon iconName="Page" />
                        </div>
                    )}
                    <div className="filethumbnailgallery-name" title={item.name}>
                        {item.name}
                    </div>
                </div>
            ))}
        </div>
    );
};

export default FileThumbnailGallery;
