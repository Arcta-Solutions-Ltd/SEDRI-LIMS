/**
 * Parses a raw MIC measurement string into operator and numeric components.
 * Mirrors {@code MicMeasurementExtensions.TryParseMicMeasurement} on the backend.
 *
 * @param {string|number|null|undefined} value - Raw MIC input or stored value.
 * @returns {{ operator: string, numeric: string|number, numericString: string, operatorOnly: boolean, isBlank: boolean, hasNumericValue: boolean }}
 */
export function parseMicMeasurement(value) {
    const blank = { operator: '', numeric: '', numericString: '', operatorOnly: false, isBlank: true, hasNumericValue: false };
    if (value === '' || value === null || value === undefined) {
        return blank;
    }
    const trimmed = String(value).trim();
    if (trimmed === '' || trimmed === '-1') {
        return blank;
    }

    let operator = '';
    let numStr = trimmed;
    if (trimmed.startsWith('<=')) {
        operator = '<=';
        numStr = trimmed.slice(2).trim();
    } else if (trimmed.startsWith('<')) {
        operator = '<';
        numStr = trimmed.slice(1).trim();
    } else if (trimmed.startsWith('>=')) {
        operator = '>=';
        numStr = trimmed.slice(2).trim();
    } else if (trimmed.startsWith('>')) {
        operator = '>';
        numStr = trimmed.slice(1).trim();
    }

    if (numStr === '') {
        return {
            operator,
            numeric: '',
            numericString: '',
            operatorOnly: operator !== '',
            isBlank: false,
            hasNumericValue: false
        };
    }

    const num = parseFloat(numStr);
    if (Number.isNaN(num)) {
        return { operator, numeric: '', numericString: '', operatorOnly: false, isBlank: false, hasNumericValue: false };
    }

    return {
        operator,
        numeric: num,
        numericString: numStr,
        operatorOnly: false,
        isBlank: false,
        hasNumericValue: true
    };
}

/**
 * Parses MIC from row state where numeric {@code Mic} and {@code Operator} may be split (API load).
 *
 * @param {Object|undefined|null} row - Disk/MIC row
 * @returns {{ operator: string, numeric: string|number, operatorOnly: boolean, isBlank: boolean, hasNumericValue: boolean }}
 */
export function parseMicMeasurementFromRow(row) {
    if (!row) {
        return parseMicMeasurement('');
    }
    const m = row.Mic;
    if (m === '' || m === null || m === undefined) {
        const op = row.Operator;
        if (op === '>' || op === '<=' || op === '<') {
            return parseMicMeasurement(op);
        }
        return parseMicMeasurement('');
    }
    const s = typeof m === 'string' ? m.trim() : String(m);
    if (s.startsWith('<') || s.startsWith('>')) {
        return parseMicMeasurement(s);
    }
    const op = row.Operator;
    if (op != null && op !== '') {
        return parseMicMeasurement(String(op) + s);
    }
    return parseMicMeasurement(s);
}

/**
 * Display string for the MIC input: operator prefix plus numeric when split on the row.
 *
 * @param {Object|undefined|null} row - Disk/MIC row
 * @returns {string|number}
 */
export function formatMicDisplay(row) {
    const parts = parseMicMeasurementFromRow(row);
    if (parts.isBlank || (parts.operatorOnly && !parts.operator)) {
        return '';
    }
    if (parts.operatorOnly) {
        return parts.operator;
    }
    if (!parts.hasNumericValue) {
        return '';
    }
    return parts.operator ? parts.operator + String(parts.numeric) : parts.numeric;
}

/**
 * Normalises a MIC field edit into split {@code Mic} and {@code Operator} row properties.
 * Clearing the field clears both; numeric edits without a prefix clear any stale operator.
 *
 * @param {string|number|null|undefined} combinedValue - Value from ASTMicField on blur
 * @returns {{ Mic: string|number, Operator: string }}
 */
export function normalizeMicRowEdit(combinedValue) {
    if (combinedValue === '' || combinedValue === null || combinedValue === undefined || combinedValue === '<') {
        return { Mic: '', Operator: '' };
    }
    const parts = parseMicMeasurement(combinedValue);
    if (parts.isBlank) {
        return { Mic: '', Operator: '' };
    }
    if (parts.operatorOnly) {
        return { Mic: parts.operator, Operator: parts.operator };
    }
    if (!parts.hasNumericValue) {
        return { Mic: combinedValue, Operator: '' };
    }
    return { Mic: parts.numeric, Operator: parts.operator };
}

/**
 * Builds the combined measurement string for susceptibility POST and save payloads.
 *
 * @param {Object|undefined|null} row - Disk/MIC row
 * @returns {string|number} Combined value, or -1 when blank
 */
export function buildMicMeasurementForSave(row) {
    const parts = parseMicMeasurementFromRow(row);
    if (parts.isBlank) {
        return -1;
    }
    if (parts.operatorOnly) {
        return parts.operator;
    }
    if (!parts.hasNumericValue) {
        return -1;
    }
    return parts.operator ? parts.operator + String(parts.numeric) : parts.numeric;
}
