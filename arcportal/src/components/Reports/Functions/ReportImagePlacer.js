import { correctImageOrientation } from './ImageOrientationHandler';

const placeSectionColumnFieldImages = (section, reportPdf) => {
    for (const field of section.Column1.Fields) {
        if (field.Image) {
            reportPdf.addImage(
                (new Image().src = `data:image/jpeg;base64,${field.Value}`),
                field.Format,
                section.Column1.Left, // x
                reportPdf.getCurrentLinePosition(), // y
                field.Width,
                field.Height,
                ''
            );
            reportPdf.incrementCurrentLineBy(field.Height + 10);
        }
    }
};

const placeImages = async (reportImages, reportPdf) => {
    if (!reportImages || reportImages.length === 0) {
        return;
    }
    
    for (const image of reportImages) {
        await placeImage(image, reportPdf);
    }
};

const placeImage = async (reportImage, reportPdf) => {
    try {
        // Correct image orientation using browser's EXIF handling
        const base64Data = reportImage.Value.startsWith('data:')
            ? reportImage.Value
            : `data:image/${reportImage.Format.toLowerCase()};base64,${reportImage.Value}`;
        
        const correctedImage = await correctImageOrientation(base64Data, reportImage.Format);
        
        reportPdf.addImage(
            correctedImage, // Use corrected image (already a data URL)
            reportImage.Format,
            reportImage.X,
            reportImage.Y,
            reportImage.Width,
            reportImage.Height,
            ''
        );
    } catch (error) {
        console.warn('Failed to correct image orientation, using original:', error);
        // Fallback to original if correction fails
        const fallbackData = reportImage.Value.startsWith('data:')
            ? reportImage.Value
            : `data:image/${reportImage.Format.toLowerCase()};base64,${reportImage.Value}`;
        
        reportPdf.addImage(
            fallbackData,
            reportImage.Format,
            reportImage.X,
            reportImage.Y,
            reportImage.Width,
            reportImage.Height,
            ''
        );
    }
};

export {
    placeSectionColumnFieldImages as PlaceSectionColumnFieldImages,
    placeImages as PlaceImages,
};
