import React from 'react';
import { connect } from 'react-redux';
import { IconButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import LaboratoryList from '../../../../Classes/Laboratory/LaboratoryList';
import './ASTIsolateTests.css';

/**
 * Normalizes a test name to the corresponding form name.
 * Handles both "testname" and "testnameform" formats; appends "form" if missing.
 * @param {string} testName - Test name from CultureTypeTestOptions
 * @returns {string} Form name (e.g. "testnameform")
 */
const toFormName = (testName) => {
    if (!testName) return '';
    const lower = String(testName).toLowerCase();
    if (lower.endsWith('form')) return lower;
    return lower + 'form';
};

/**
 * Returns isolate test forms applicable for the current culture type and organism scope.
 * When astData.ApplicableIsolateTests is provided (from backend), uses that list directly.
 * Otherwise falls back to CultureTypeTestOptions (culture type filter only).
 *
 * @param {Array} allIsolateForms - All forms with formtype === 'culturetest'
 * @param {Object} cultureTypeTestOptions - Map of cultureTypeId to array of test names
 * @param {number|string} cultureTypeId - Current culture type ID
 * @param {Array} [applicableIsolateTests] - Optional backend-provided list of applicable form names
 * @returns {Array} Applicable form objects
 */
const getApplicableForms = (allIsolateForms, cultureTypeTestOptions, cultureTypeId, applicableIsolateTests) => {
    if (!allIsolateForms || allIsolateForms.length === 0) return [];
    // When the backend sends an array (including empty), use it — do not fall back to CultureTypeTestOptions.
    if (applicableIsolateTests !== undefined && applicableIsolateTests !== null && Array.isArray(applicableIsolateTests)) {
        const allowedNames = new Set(applicableIsolateTests.map((n) => toFormName(n)));
        return allIsolateForms.filter((f) => allowedNames.has((f.Name || '').toLowerCase()));
    }
    const key = cultureTypeTestOptions[cultureTypeId]
        ?? cultureTypeTestOptions[String(cultureTypeId)]
        ?? cultureTypeTestOptions[Number(cultureTypeId)];
    if (!key || !Array.isArray(key) || key.length === 0) {
        return allIsolateForms;
    }
    const allowedNames = new Set(key.map((n) => toFormName(n)));
    return allIsolateForms.filter((f) => allowedNames.has((f.Name || '').toLowerCase()));
};

/**
 * Isolate tests panel - shows applicable tests as panels with edit icon.
 * When no CultureTypeTestOptions exist for a culture type, shows all isolate test forms.
 * Green when completed (Status === "Complete" in culturetests table). Edit opens the test form.
 *
 * @param {Object} props
 * @param {number} props.cultureId - Culture ID
 * @param {number} props.cultureTypeId - Culture type ID for filtering applicable tests
 * @param {number} props.specimenTypeId - Specimen type ID
 * @param {number} props.laboratoryId - Laboratory ID
 * @param {Object} props.astData - { CultureTests: [{ Id, TestName, Status }] } for completion status
 * @param {Function} props.onEditTest - (formName, cultureId, cultureTestId?) => void - opens the isolate test form
 * @param {Object} props.laboratory - Laboratory config (from Redux)
 * @param {Array} props.forms - Forms config (from Redux)
 * @param {string} props.language - Current language
 */
const ASTIsolateTests = (props) => {
    const { cultureId, cultureTypeId, specimenTypeId, laboratoryId, astData, onEditTest, laboratory, forms, language } = props;

    if (!laboratory) return null;

    const labList = new LaboratoryList(laboratory, specimenTypeId);
    const lab = labList.getLaboratory(laboratoryId);
    const cultureTypeTestOptions = lab?.CultureTypeTestOptions || {};
    const applicableIsolateTests = astData?.ApplicableIsolateTests ?? astData?.applicableIsolateTests;

    const allIsolateForms = (forms || []).filter(
        (f) => (f.FormType || f.Formtype || f.formtype || '').toLowerCase() === 'culturetest'
    );
    // Deduplicate by Name: config may include the same form multiple times when referenced by multiple UI events
    const uniqueIsolateForms = allIsolateForms.filter(
        (f, i, arr) => arr.findIndex((x) => (x.Name || '').toLowerCase() === (f.Name || '').toLowerCase()) === i
    );
    const applicableForms = getApplicableForms(uniqueIsolateForms, cultureTypeTestOptions, cultureTypeId, applicableIsolateTests);

    const isCompleted = (formName) =>
        (astData?.CultureTests || []).some(
            (t) => (t.TestName || '').toLowerCase() === (formName || '').toLowerCase() && t.Status === 'Complete'
        );

    /**
     * Handles Edit click. Resolves culturetest Id from astData when the test exists; otherwise passes cultureId
     * so the parent can call GetOrCreateCultureTestId before opening the form.
     */
    const handleEditClick = (formName) => {
        const form = forms?.find((f) => (f.Name || '').toLowerCase() === (formName || '').toLowerCase());
        if (!form || !onEditTest) return;
        const cultureTest = (astData?.CultureTests || []).find(
            (t) => (t.TestName || '').toLowerCase() === (formName || '').toLowerCase()
        );
        const cultureTestId = cultureTest?.Id ?? cultureTest?.id;
        onEditTest(formName, cultureId, cultureTestId);
    };

    if (applicableForms.length === 0) return null;

    return (
        <div>
            <div className="astform-section-label-alt">
                {TranslateTag('@AstEnz@', props.language)}
            </div>
            <div className="astform-first-row">
                <div className="astform-antibiotic-level" />
                <div className="astform-isolate-tests">
                    {applicableForms.map((form) => {
                        const formName = form.Name || '';
                        const completed = isCompleted(formName);
                        const label = form.Title || formName;
                        return (
                            <div
                                key={formName}
                                className={`astform-isolate-test-item ${completed ? 'astform-isolate-test-completed' : ''}`}
                            >
                        <span className="astform-isolate-test-name">{label}</span>
                                <IconButton
                                    iconProps={{ iconName: 'Edit' }}
                                    title={TranslateTag('@GenEdiB@', language)}
                                    onClick={() => handleEditClick(formName)}
                                />
                            </div>
                        );
                    })}
                </div>
            </div>
        </div>
    );
};

const mapStateToProps = (state) => ({
    forms: state.config.forms
});

export default connect(mapStateToProps)(ASTIsolateTests);
