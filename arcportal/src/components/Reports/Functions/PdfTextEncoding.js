/*
 * jsPDF writes the standard (non embedded) fonts with WinAnsiEncoding, which only
 * covers U+0000 - U+00FF. As soon as a single character above U+00FF appears in a
 * string, jsPDF re-encodes the *whole* string as UCS-2 BE, so every character gains
 * a leading NUL byte and the viewer draws the line with a blank between each letter
 * ("C o u n t   1 0 u   m l / c f m"). Superscripts are the usual culprit: 1, 2 and
 * 3 exist in Latin-1 but 0 and 4 - 9 do not, so a title such as "> 10^5 ml/cfm"
 * garbles the whole title.
 *
 * Superscript and subscript runs are therefore drawn by ReportPdf as raised/lowered
 * plain characters at a smaller font size, and anything else outside Latin-1 is
 * transliterated to a WinAnsi equivalent before it reaches jsPDF.
 */

const SUPERSCRIPTS = {
    '⁰': '0',
    '¹': '1',
    '²': '2',
    '³': '3',
    '⁴': '4',
    '⁵': '5',
    '⁶': '6',
    '⁷': '7',
    '⁸': '8',
    '⁹': '9',
    '⁺': '+',
    '⁻': '-',
    '⁼': '=',
    '⁽': '(',
    '⁾': ')',
    'ⁿ': 'n',
};

const SUBSCRIPTS = {
    '₀': '0',
    '₁': '1',
    '₂': '2',
    '₃': '3',
    '₄': '4',
    '₅': '5',
    '₆': '6',
    '₇': '7',
    '₈': '8',
    '₉': '9',
    '₊': '+',
    '₋': '-',
    '₌': '=',
    '₍': '(',
    '₎': ')',
    'ₙ': 'n',
};

// Characters above U+00FF that report authors paste in regularly, mapped onto
// something the standard fonts can actually draw.
const TRANSLITERATIONS = {
    'α': 'alpha',
    'β': 'beta',
    'γ': 'gamma',
    'δ': 'delta',
    'λ': 'lambda',
    'μ': 'µ', // greek small mu -> micro sign
    'ω': 'omega',
    'Ω': 'Omega', // greek capital omega
    'Ω': 'Omega', // ohm sign
    '‘': "'",
    '’': "'",
    '‚': "'",
    '“': '"',
    '”': '"',
    '„': '"',
    '‹': '<',
    '›': '>',
    '‐': '-',
    '‑': '-',
    '‒': '-',
    '–': '-', // en dash
    '—': '-', // em dash
    '―': '-',
    '−': '-', // minus sign
    '…': '...',
    '•': '·', // bullet -> middle dot
    '⁄': '/', // fraction slash
    '→': '->',
    '←': '<-',
    '≤': '<=',
    '≥': '>=',
    '≠': '!=',
    '≈': '~',
    '™': '(TM)',
    '℃': '°C',
    '℉': '°F',
    '‰': '%o',
    ' ': ' ', // figure space
    ' ': ' ', // thin space
    '​': '', // zero width space
    ' ': ' ', // narrow no-break space
    '﻿': '', // byte order mark
};

const SCRIPT_KINDS = {
    superscript: SUPERSCRIPTS,
    subscript: SUBSCRIPTS,
};

const scriptKindOf = (character) => {
    if (SUPERSCRIPTS[character]) {
        return 'superscript';
    }
    if (SUBSCRIPTS[character]) {
        return 'subscript';
    }
    return 'normal';
};

const containsScriptCharacters = (text) =>
    typeof text === 'string' &&
    Array.from(text).some((character) => scriptKindOf(character) !== 'normal');

const containsUnsupportedCharacters = (text) =>
    typeof text === 'string' &&
    Array.from(text).some((character) => character.charCodeAt(0) > 0xff);

/**
 * Replaces every character the standard fonts cannot encode. Superscripts and
 * subscripts fall back to their plain equivalent, so prefer SplitScriptSegments
 * where the caller is able to draw them raised - this is the last line of defence
 * for text jsPDF would otherwise garble (table cells, multi line text and so on).
 */
const toPdfSafeText = (text) => {
    if (typeof text !== 'string' || !containsUnsupportedCharacters(text)) {
        return text;
    }

    let safeText = '';
    for (const character of text) {
        if (character.charCodeAt(0) <= 0xff) {
            safeText += character;
        } else if (SUPERSCRIPTS[character] || SUBSCRIPTS[character]) {
            safeText += SUPERSCRIPTS[character] || SUBSCRIPTS[character];
        } else if (character in TRANSLITERATIONS) {
            safeText += TRANSLITERATIONS[character];
        } else {
            safeText += '?';
        }
    }

    return safeText;
};

/**
 * Breaks a string into consecutive runs of normal, superscript and subscript text,
 * with the script characters replaced by the plain characters drawn in their place.
 *
 * "Count > 10⁵ ml/cfm" =>
 *   [{ text: 'Count > 10', kind: 'normal' },
 *    { text: '5', kind: 'superscript' },
 *    { text: ' ml/cfm', kind: 'normal' }]
 */
const splitScriptSegments = (text) => {
    const segments = [];

    for (const character of String(text)) {
        const kind = scriptKindOf(character);
        const plainCharacter =
            kind === 'normal' ? character : SCRIPT_KINDS[kind][character];
        const currentSegment = segments[segments.length - 1];

        if (currentSegment && currentSegment.kind === kind) {
            currentSegment.text += plainCharacter;
        } else {
            segments.push({ text: plainCharacter, kind });
        }
    }

    return segments.map((segment) => ({
        ...segment,
        text:
            segment.kind === 'normal'
                ? toPdfSafeText(segment.text)
                : segment.text,
    }));
};

export {
    toPdfSafeText as ToPdfSafeText,
    splitScriptSegments as SplitScriptSegments,
    containsScriptCharacters as ContainsScriptCharacters,
    containsUnsupportedCharacters as ContainsUnsupportedCharacters,
};
