import React from 'react';
import { Spinner } from '@fluentui/react';
import { useDelayedVisibility } from '../../../Utils/General/useDelayedVisibility';
import './FormContentLoading.css';

const spinnerStyles = { circle: { width: '56px', height: '56px', borderWidth: '4px' } };

/**
 * Centered in-panel loading indicator; spinner appears only after a delay while `active` stays true.
 */
const FormContentLoading = ({ active, delayMs = 200 }) => {
    const showSpinner = useDelayedVisibility(!!active, delayMs);

    return (
        <div className="form-content-loading" role="status" aria-live="polite" aria-busy={!!active}>
            {showSpinner && <Spinner styles={spinnerStyles} />}
        </div>
    );
};

export default FormContentLoading;
