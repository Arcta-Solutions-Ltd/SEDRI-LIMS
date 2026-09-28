import React, { useEffect, useState } from 'react';
import { useBarcode } from 'next-barcode';

const BarcodeLinear = (props) => {
    const [isRendered, setIsRendered] = useState(false);

    const { inputRef } = useBarcode({
        value: props.barcode || '0000000000',
        options: {
            text: props.barcodeLabel,
            format: "CODE128",
            margin: 0,
            height: props.barcodeHeight
        },
        onError: (error) => {
            console.error("Barcode generation error:", error);
        },
        onComplete: () => {
            setIsRendered(true);
        }
    });

    useEffect(() => {
        const timer = setTimeout(() => {
            if (inputRef.current) {
                const svgElement = inputRef.current;
                if (svgElement && !isRendered) {
                    svgElement.setAttribute('width', '100%');
                    svgElement.style.display = 'block';
                    setIsRendered(true);
                }
            }
        }, 100);

        return () => clearTimeout(timer);
    }, [inputRef, isRendered]);

    return (
        <div className="barcode-container">
            <svg ref={inputRef} />
        </div>
    );
};

export default BarcodeLinear;
