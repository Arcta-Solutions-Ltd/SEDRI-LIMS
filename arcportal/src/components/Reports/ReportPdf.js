import jsPDF from 'jspdf';
import {
    ContainsScriptCharacters,
    SplitScriptSegments,
    ToPdfSafeText,
} from './Functions/PdfTextEncoding';

// Superscripts/subscripts are drawn as plain characters at a reduced size, shifted
// off the baseline, because the standard PDF fonts have no glyph for most of them.
const SCRIPT_FONT_SIZE_RATIO = 0.65;
const SUPERSCRIPT_RAISE_RATIO = 0.33;
const SUBSCRIPT_DROP_RATIO = 0.15;

// jsPDF accepts both text(text, x, y, ...) and the legacy text(x, y, text, ...).
const resolveTextArguments = (args) => {
    const [first, second, third] = args;
    const legacyOrder =
        typeof first === 'number' &&
        typeof second === 'number' &&
        (typeof third === 'string' || Array.isArray(third));

    return legacyOrder
        ? { text: third, x: first, y: second, rest: args.slice(3), legacyOrder }
        : { text: first, x: second, y: third, rest: args.slice(3), legacyOrder };
};

const withReplacedText = (args, text, legacyOrder) => {
    const replacedArgs = [...args];
    replacedArgs[legacyOrder ? 2 : 0] = text;
    return replacedArgs;
};

// Anything that repositions or reflows the text itself (alignment, wrapping,
// rotation, transformation matrices) has to stay with jsPDF, so those calls only
// get the characters transliterated rather than split into segments.
const canDrawSegments = (rest) =>
    rest.length === 0 ||
    (rest.length === 1 &&
        (rest[0] === undefined ||
            rest[0] === null ||
            (Object.getPrototypeOf(rest[0]) === Object.prototype &&
                !rest[0].maxWidth &&
                !rest[0].angle &&
                (!rest[0].align || rest[0].align === 'left'))));

class ReportPdf {
    margins = {};
    header = {};
    headerHeight = 0;
    footerHeight = 0;
    contentMargin = {
        top: 0,
        bottom: 0,
        left: 0,
        right: 0
    }
    spacing = {
        sm: 5,
        md: 10,
        l: 15,
        xl: 20,
    };

    constructor(margins = {}, format = 'a4', watermarkText = '') {
        Object.assign(this, new jsPDF('p', 'pt', format));
        this.setMargins(margins, 1);
        this.bounds = {
            width: this.internal.pageSize.width,
            height: this.internal.pageSize.height,
        };
        this.x = this.margins.left;
        this.y = this.margins.top;
        this.watermarkText = watermarkText

        // Object.assign above copies jsPDF's own methods over any class field of the
        // same name, so the text wrapper has to be installed after it.
        this.baseJsPdfText = this.text;
        this.baseJsPdfGetTextWidth = this.getTextWidth;
        this.baseJsPdfGetStringUnitWidth = this.getStringUnitWidth;
        this.text = (...args) => this.writeText(args);
        this.getTextWidth = (text) =>
            this.baseJsPdfGetTextWidth(ToPdfSafeText(text));
        this.getStringUnitWidth = (text, options) =>
            this.baseJsPdfGetStringUnitWidth(ToPdfSafeText(text), options);

        this.setFont('Times');
        this.setFontSize(11);

        this.initialisePageState();
        // Cache watermark image data for efficient redrawing
        this.watermarkImageData = this.createWatermarkImageData(watermarkText);
        // IMPORTANT: Do not draw watermark here. We apply watermarks once per page
        // via redrawWatermarksOnAllPages() after all content is written, to avoid
        // double-drawing page 1 (which makes it appear darker than subsequent pages).

    }

    initialisePageState = () =>
        (this.state = {
            currentLine: 1,
            currentLinePos: this.getMargins(this.getCurrentPageNumber()).left,
        });

    getMargins = (pageNumber = 1) =>
        this.margins[pageNumber] || this.margins[1];
    setMargins = (margins, pageNumber = 1) =>
        (this.margins[pageNumber] = {
            left: this.spacing.xl,
            right: this.spacing.xl,
            top: this.spacing.xl,
            bottom: this.spacing.l,
            ...margins,
        });
    getState = () => this.state;
    setState = (state) => (this.state = state);

    // True for the embedded unicode fonts (Lao uses Phetsarath OT), which carry
    // their own glyphs and must be handed the original text untouched.
    usesUnicodeFont = () => this.getFont().encoding === 'Identity-H';

    /**
     * Wrapper around jsPDF's text() that keeps characters the standard fonts cannot
     * encode from garbling the whole string - see Functions/PdfTextEncoding.
     */
    writeText = (args) => {
        const { text, x, y, rest, legacyOrder } = resolveTextArguments(args);

        if (this.usesUnicodeFont()) {
            return this.baseJsPdfText(...args);
        }

        if (Array.isArray(text)) {
            return this.baseJsPdfText(
                ...withReplacedText(args, text.map(ToPdfSafeText), legacyOrder)
            );
        }

        if (typeof text !== 'string') {
            return this.baseJsPdfText(...args);
        }

        if (ContainsScriptCharacters(text) && canDrawSegments(rest)) {
            return this.writeTextWithScriptSegments(text, x, y, rest);
        }

        return this.baseJsPdfText(
            ...withReplacedText(args, ToPdfSafeText(text), legacyOrder)
        );
    };

    writeTextWithScriptSegments = (text, x, y, rest) => {
        const baseFontSize = this.getFontSize();
        let segmentX = x;

        for (const segment of SplitScriptSegments(text)) {
            if (!segment.text) {
                continue;
            }

            const isScript = segment.kind !== 'normal';
            if (isScript) {
                this.setFontSize(baseFontSize * SCRIPT_FONT_SIZE_RATIO);
            }

            this.baseJsPdfText(
                segment.text,
                segmentX,
                this.scriptBaseline(y, segment.kind, baseFontSize),
                ...rest
            );
            segmentX += this.getTextWidth(segment.text);

            if (isScript) {
                this.setFontSize(baseFontSize);
            }
        }

        return this;
    };

    scriptBaseline = (y, kind, baseFontSize) => {
        switch (kind) {
            case 'superscript':
                return y - baseFontSize * SUPERSCRIPT_RAISE_RATIO;
            case 'subscript':
                return y + baseFontSize * SUBSCRIPT_DROP_RATIO;
            default:
                return y;
        }
    };

    // TODO: Rename these to getCurrentYPosition and setCurrentYPosition etc
    // since the line number has nothing to do with it
    getCurrentLinePosition = () => this.state.currentLinePos;
    setCurrentLinePosition = (currentLinePosition) =>
        (this.state.currentLinePos = currentLinePosition);
    incrementCurrentLinePositionBy = (amount) =>
        (this.state.currentLinePos += amount);
    decrementCurrentLinePositionBy = (amount) =>
        (this.state.currentLinePos -= amount);

    getCurrentLine = () => this.state.currentLine;
    setCurrentLine = (currentLine) => (this.state.currentLine = currentLine);
    incrementCurrentLineBy = (amount) => (this.state.currentLine += amount);
    decrementCurrentLineBy = (amount) => (this.state.currentLine -= amount);
    getSavedSectionHeading = () => this.state.savedSectionHeading;
    setSavedSectionHeading = (sectionHeading) =>
        (this.state.savedSectionHeading = sectionHeading);

    /** @returns {number} Grid ContentsConfigs still expected for the current deferred heading group. */
    getLinkedSectionsRemaining = () => this.state.linkedSectionsRemaining ?? 0;

    /** @param {number} count - Grid ContentsConfigs still expected for the current deferred heading group. */
    setLinkedSectionsRemaining = (count) =>
        (this.state.linkedSectionsRemaining = count);

    /**
     * Decrements the linked-grid countdown and returns the remaining count.
     * @returns {number} Grid ContentsConfigs still expected after this decrement.
     */
    decrementLinkedSectionsRemaining = () => {
        const remaining = Math.max(0, this.getLinkedSectionsRemaining() - 1);
        this.setLinkedSectionsRemaining(remaining);
        return remaining;
    };
    horizontalRule = () => {
        this.line(
            0,
            this.getCurrentLinePosition(),
            this.getPageWidth(),
            this.getCurrentLinePosition(),
            'S'
        );
    };

    baseJsPdfAddPage = this.addPage;
    addPage = (margins) => {
        this.baseJsPdfAddPage();
        this.setMargins(
            (this.margins = {
                ...this.getMargins(1),
                ...margins,
            }),
            this.getCurrentPageNumber()
        );
        // Do not draw watermark here; it will be inserted once per page in the final pass.
    };

    getCurrentPageNumber = () => this.internal.getCurrentPageInfo().pageNumber;

    getHeaderHeight = () => this.headerHeight || 0;
    setHeaderHeight = (height) => (this.headerHeight = height);

    getFooterHeight = () => this.footerHeight || 0;
    setFooterHeight = (height) => this.footerHeight = height;

    getFooterStartY = (pageNumber) => {
        const footerHeight = this.getFooterHeight(pageNumber);
        return (
            this.pageHeight - this.getMargins(pageNumber).bottom - footerHeight
        );
    };

    getContentStartY = () => this.getHeaderHeight(pageNumber) + this.contentMargin.top;
    getContentEndY = () => this.getPageHeight() - this.getFooterHeight() - this.contentMargin.bottom;

    createWatermarkImageData(text = 'NOT APPROVED') {
        if (!text) {
            return null;
        }

        const { width: w, height: h } = this.internal.pageSize;

        const canvas = document.createElement('canvas');
        canvas.width  = w;
        canvas.height = h;
        const ctx = canvas.getContext('2d');

        if (!ctx) {
            return null;
        }

        const angleRad = Math.atan(h / w);
        const diag     = Math.sqrt(w * w + h * h);
        const targetW  = diag * 0.8;   

        let fontSize = 60;              
        ctx.font = `${fontSize}px Times New Roman`;
        let textWidth = ctx.measureText(text).width;

        if (textWidth > 0) {
            fontSize = (targetW / textWidth) * fontSize;
            ctx.font = `${fontSize}px Times New Roman`;
            textWidth = ctx.measureText(text).width;
        }

        ctx.clearRect(0, 0, w, h);
        ctx.translate(w / 2, h / 2);            
        ctx.rotate(angleRad);                    
        ctx.textAlign    = 'center';             
        ctx.textBaseline = 'middle';             
        ctx.fillStyle    = 'rgba(200, 0, 0, 0.15)';
        ctx.fillText(text, 0, 0);                

        return canvas.toDataURL('image/png');
    }

    drawWatermark(text = 'NOT APPROVED') {
        if (!text) {
            return;
        }

        const { width: w, height: h } = this.internal.pageSize;
        const dataUrl = this.watermarkImageData || this.createWatermarkImageData(text);
        
        if (dataUrl) {
            this.addImage(dataUrl, 'PNG', 0, 0, w, h);
        }
    }

    redrawWatermarkOnCurrentPage() {
        if (!this.watermarkText || !this.watermarkImageData) {
            return;
        }

        const { width: w, height: h } = this.internal.pageSize;
        
        // Use jsPDF's internal API to insert watermark at the beginning of content stream
        // This ensures it renders behind all other content
        try {
            const currentPage = this.getCurrentPageNumber();
            
            // Access the page object
            if (this.internal.pages && this.internal.pages[currentPage]) {
                const page = this.internal.pages[currentPage];
                
                // Get or add the watermark image to the PDF
                let imgIndex;
                try {
                    const imgProps = this.internal.getImageProperties(this.watermarkImageData);
                    imgIndex = imgProps ? imgProps.index : null;
                } catch (e) {
                    imgIndex = null;
                }
                
                // Add image if not already in PDF
                if (imgIndex === null) {
                    // Add image without drawing it yet
                    this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h, '', 'FAST');
                    try {
                        const imgProps = this.internal.getImageProperties(this.watermarkImageData);
                        imgIndex = imgProps ? imgProps.index : null;
                    } catch (e) {
                        // Fallback to standard method
                        this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
                        return;
                    }
                }
                
                // Manipulate the page content array to prepend watermark
                // jsPDF stores page content as an array of PDF command strings.
                if (Array.isArray(page) && imgIndex !== null) {
                    // Create PDF commands to draw the watermark image
                    const watermarkOps = [
                        'q',  // Save graphics state
                        `${w} 0 0 ${h} 0 0 cm`,  // Transformation matrix (width, height, x, y)
                        `/I${imgIndex} Do`,  // Draw the image object
                        'Q'   // Restore graphics state
                    ];
                    
                    // Check if watermark already exists in content
                    const contentStr = page.join(' ');
                    const watermarkPattern = `/I${imgIndex} Do`;
                    
                    if (!contentStr.includes(watermarkPattern)) {
                        // Prepend watermark operations to ensure they execute first (behind content)
                        page.unshift(...watermarkOps);
                    }
                } else {
                    // Fallback: standard addImage
                    this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
                }
            } else {
                // Fallback: standard addImage
                this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
            }
        } catch (error) {
            // If internal manipulation fails, use standard method
            // This ensures watermark is at least redrawn, even if on top
            const { width: w, height: h } = this.internal.pageSize;
            this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
        }
    }

    redrawWatermarksOnAllPages() {
        if (!this.watermarkText || !this.watermarkImageData) {
            return;
        }

        const totalPages = this.getNumberOfPages();
        const currentPage = this.getCurrentPageNumber();
        const { width: w, height: h } = this.internal.pageSize;
        
        // Ensure watermark image is added to PDF resources
        let imgIndex = null;
        try {
            const imgProps = this.internal.getImageProperties(this.watermarkImageData);
            imgIndex = imgProps ? imgProps.index : null;
        } catch (e) {
            // Image not yet in PDF, will add it
        }
        
        if (imgIndex === null) {
            // Add image to first page to get it into PDF resources
            this.setPage(1);
            // addImage() appends drawing ops to the current page content stream.
            // We only want to register the image as a resource, NOT actually draw it here,
            // otherwise page 1 can end up with a second watermark and look darker.
            const page1 = this.internal.pages?.[1];
            const page1LenBefore = Array.isArray(page1) ? page1.length : null;

            this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h, '', 'FAST');

            // Remove any ops addImage appended to page 1 (resource registration stays).
            if (Array.isArray(page1) && page1LenBefore !== null && page1.length > page1LenBefore) {
                page1.splice(page1LenBefore);
            }
            try {
                const imgProps = this.internal.getImageProperties(this.watermarkImageData);
                imgIndex = imgProps ? imgProps.index : null;
            } catch (e) {
                imgIndex = null;
            }
        }
        
        // Iterate through all pages and insert watermark at beginning of content stream
        for (let pageNum = 1; pageNum <= totalPages; pageNum++) {
            this.setPage(pageNum);
            
            try {
                if (this.internal.pages && this.internal.pages[pageNum]) {
                    const page = this.internal.pages[pageNum];
                    
                    if (Array.isArray(page) && imgIndex !== null) {
                        // Create watermark drawing commands
                        const watermarkOps = [
                            'q',  // Save graphics state
                            `${w} 0 0 ${h} 0 0 cm`,  // Transformation matrix
                            `/I${imgIndex} Do`,  // Draw image
                            'Q'   // Restore graphics state
                        ];
                        
                        // Check if watermark already exists
                        const contentStr = page.join(' ');
                        const watermarkPattern = `/I${imgIndex} Do`;
                        
                        if (!contentStr.includes(watermarkPattern)) {
                            // Prepend watermark to content array
                            // This ensures it renders first (behind all other content)
                            page.unshift(...watermarkOps);
                        }
                    } else {
                        // Fallback: use standard addImage
                        this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
                    }
                } else {
                    // Fallback: standard addImage
                    this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
                }
            } catch (error) {
                // If manipulation fails, use standard method
                this.addImage(this.watermarkImageData, 'PNG', 0, 0, w, h);
            }
        }
        
        // Restore to original page
        this.setPage(currentPage);
    }

    /**
     * Replace page number placeholders for a specific page range (batch mode).
     * Replaces "docPageNum of {total_pages_count_string}" with "relativePageNum of reportPageCount"
     * on each page in [startPage, endPage], so each report in a batch shows correct per-report numbering.
     */
    putTotalPagesForPageRange(startPage, endPage, reportPageCount) {
        if (!this.internal.pages) {
            return;
        }
        const pattern = /\d+ of \{total_pages_count_string\}/g;
        for (let pageNum = startPage; pageNum <= endPage; pageNum++) {
            const relativePageNum = pageNum - startPage + 1;
            const replacement = `${relativePageNum} of ${reportPageCount}`;
            const page = this.internal.pages[pageNum];
            if (Array.isArray(page)) {
                for (let i = 0; i < page.length; i++) {
                    page[i] = page[i].replace(pattern, replacement);
                }
            }
        }
    }
}

export default ReportPdf;
