/**
 * Corrects image orientation by using browser's automatic EXIF handling
 * Returns a Promise that resolves to a base64 string with correct orientation
 * @param {string} base64Image - Base64 encoded image data (with or without data URL prefix)
 * @param {string} format - Image format (JPEG, PNG, etc.)
 * @returns {Promise<string>} Base64 data URL of correctly oriented image
 */
export const correctImageOrientation = (base64Image, format = 'JPEG') => {
    return new Promise((resolve, reject) => {
        const img = new Image();
        
        img.onload = () => {
            // Create canvas with image's natural dimensions
            // (browser has already applied EXIF orientation)
            const canvas = document.createElement('canvas');
            canvas.width = img.naturalWidth;
            canvas.height = img.naturalHeight;
            
            const ctx = canvas.getContext('2d');
            
            // Draw image to canvas - browser has already applied orientation
            ctx.drawImage(img, 0, 0);
            
            // Convert to base64 data URL
            const correctedBase64 = canvas.toDataURL(`image/${format.toLowerCase()}`, 0.95);
            resolve(correctedBase64);
        };
        
        img.onerror = (error) => {
            reject(new Error('Failed to load image for orientation correction: ' + error));
        };
        
        // Load image - browser will automatically apply EXIF orientation
        img.src = base64Image.startsWith('data:') 
            ? base64Image 
            : `data:image/${format.toLowerCase()};base64,${base64Image}`;
    });
};




