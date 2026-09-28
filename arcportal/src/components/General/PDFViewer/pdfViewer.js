import React, { useEffect, useRef, useState } from 'react';
import * as pdfjsLib from 'pdfjs-dist';
import 'pdfjs-dist/build/pdf.worker';
import './pdfViewer.css';

const PdfViewer = (props) => {
    const viewerRef = useRef(null);
    const [thumbnails, setThumbnails] = useState([]);
    const [pdf, setPdf] = useState(null);
    const [pageNum, setPageNum] = useState(1);

    useEffect(() => {
        const loadPdf = async (url) => {
            pdfjsLib.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.mjs';
            //pdfjsLib.GlobalWorkerOptions.workerSrc = '$\'pdfjs-dist/build/pdf.worker.min.mjs\'';

            const loadingTask = pdfjsLib.getDocument(url);
            const loadedPdf = await loadingTask.promise;
            setPdf(loadedPdf);

            const thumbnailsArray = [];
            if (loadedPdf.numPages > 1) {
                for (let pageNumber = 1; pageNumber <= loadedPdf.numPages; pageNumber++) {
                    const page = await loadedPdf.getPage(pageNumber);
                    const canvas = document.createElement('canvas');
                    const context = canvas.getContext('2d');
                    const viewport = page.getViewport({ scale: 0.1 }); // Reduced the scale for smaller thumbnails

                    canvas.width = viewport.width;
                    canvas.height = viewport.height;

                    const renderContext = {
                        canvasContext: context,
                        viewport: viewport
                    };
                    await page.render(renderContext).promise;

                    thumbnailsArray.push({ pageNumber, thumbnail: canvas.toDataURL() });
                }
            } else {
                setPageNum(1); // Directly load the single page
            }
            setThumbnails(thumbnailsArray);
        };

        if (props.data !== undefined) {
            const pdfBlob = props.data.output('blob');
            const pdfUrl = URL.createObjectURL(pdfBlob);
            loadPdf(pdfUrl);
        }
    }, [props.data]);

    useEffect(() => {
        const loadPage = async (pageNumber) => {
            const pdfViewer = viewerRef.current;
            if (!pdf || !pdfViewer) return;

            pdfViewer.innerHTML = '';  // Clear the viewer

            const displayPage = pdf.numPages < pageNumber ? pdf.numPages : pageNumber;
            const page = await pdf.getPage(displayPage);
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');
            const viewport = page.getViewport({ scale: 1 });

            while (pdfViewer.firstChild) {
                pdfViewer.removeChild(pdfViewer.firstChild);
            }
            pdfViewer.appendChild(canvas);

            const containerWidth = canvas.parentElement.clientWidth;
            const containerHeight = canvas.parentElement.clientHeight;

            const scale = Math.min(containerWidth / viewport.width, containerHeight / viewport.height) - 0.1;
            const scaledViewport = page.getViewport({ scale });

            canvas.width = scaledViewport.width;
            canvas.height = scaledViewport.height;

            const renderContext = {
                canvasContext: context,
                viewport: scaledViewport
            };
            await page.render(renderContext).promise;

            canvas.style.display = 'block';
            canvas.style.margin = '20px auto';  // Centers the canvas horizontally
            canvas.style.border = '1px solid #000';
        };

        if (pdf) {
            loadPage(pageNum);
        }
    }, [pageNum, pdf]);

    
    return (
        <div className="pdf-container">
            {thumbnails.length > 0 && (
                <div className="thumbnails">
                    {thumbnails.map(thumbnail => (
                        <img 
                            key={thumbnail.pageNumber} 
                            src={thumbnail.thumbnail} 
                            alt={`Page ${thumbnail.pageNumber}`} 
                            onClick={() => setPageNum(thumbnail.pageNumber)}
                            style={{ border: pageNum === thumbnail.pageNumber ? '2px solid blue' : '1px solid gray' }}
                        />
                    ))}
                </div>
            )}
            <div className="pdf-viewer" ref={viewerRef}></div>
        </div>
    );
};

export default PdfViewer;



