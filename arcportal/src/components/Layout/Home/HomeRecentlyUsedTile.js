import React, { useEffect, useMemo, useState } from 'react';
import Post from '../../../Data/Post';
import { buildRecentlyUsedParameters } from './buildDashboardQueryParameters';
import { HOMEDASHBOARD_RECENTLY_USED_QUERY } from './homeDashboardUtils';
import './HomeRecentlyUsedTile.css';

/**
 * Resolves list item id to display text for list-backed fields (e.g. specimen type).
 * @param {Array} lists
 * @param {string} listName
 * @param {string|number} key
 * @returns {string}
 */
function listItemText(lists, listName, key) {
    if (key == null || key === '') return '';
    const list = lists?.find((l) => (l.Name || l.name || '').toLowerCase() === listName.toLowerCase());
    const opts = list?.Options || list?.options || [];
    const k = String(key);
    const o = opts.find((x) => String(x.key ?? x.Key) === k);
    return o ? String(o.text ?? o.Text ?? '') : '';
}

/**
 * Maps tests.testname (form Name) to the user-facing Title from directtestconfiglist.
 */
function directTestDisplayLabel(lists, testName) {
    if (testName == null || testName === '') return '';
    const exact = listItemText(lists, 'directtestconfiglist', testName);
    if (exact) return exact;
    const list = lists?.find((l) => (l.Name || l.name || '').toLowerCase() === 'directtestconfiglist');
    const opts = list?.Options || list?.options || [];
    const k = String(testName).toLowerCase();
    const o = opts.find((x) => String(x.key ?? x.Key).toLowerCase() === k);
    return o ? String(o.text ?? o.Text ?? '') : '';
}

/**
 * @param {object} props
 * @param {object} props.section Recently Used section config.
 * @param {Array} props.lists Redux lists for display labels.
 * @param {function} [props.onItemClick] Navigate when a row is activated.
 * @param {string} [props.homeAuthScopeKey] Toolbar lab/org scope; triggers refetch when JWT scope changes on Home.
 */
const HomeRecentlyUsedTile = (props) => {
    const { section, lists, onItemClick, homeAuthScopeKey = '' } = props;
    const [rows, setRows] = useState([]);

    const requestKey = useMemo(
        () => JSON.stringify({ p: buildRecentlyUsedParameters(section), scope: homeAuthScopeKey ?? '' }),
        [section, homeAuthScopeKey]
    );

    useEffect(() => {
        const criteria = {
            Name: HOMEDASHBOARD_RECENTLY_USED_QUERY,
            Parameters: buildRecentlyUsedParameters(section),
        };
        Post(
            'query/filteredget',
            criteria,
            (res) => setRows(Array.isArray(res) ? res : []),
            () => setRows([])
        );
    }, [requestKey]);

    const patientName = (row) => {
        const fn = row.PatientFirstName || row.patientFirstName || '';
        const sn = row.PatientSurname || row.patientSurname || '';
        return `${fn} ${sn}`.trim();
    };

    return (
        <div className="home-dashboard-tile-inner home-recently-used-tile">
            <div className="homesection-fieldcontent home-recently-used-list">
                {rows.map((row, index) => {
                    const kind = (row.Kind || row.kind || '').toLowerCase();
                    const cls =
                        kind === 'test'
                            ? 'home-recent-card home-recent-card--test'
                            : kind === 'patient'
                              ? 'home-recent-card home-recent-card--patient'
                              : kind === 'culture'
                                ? 'home-recent-card home-recent-card--culture'
                                : 'home-recent-card home-recent-card--specimen';
                    const key = `${kind}-${row.TestId || row.testId || ''}-${row.CultureId || row.cultureId || ''}-${row.SpecimenId || row.specimenId || ''}-${row.PatientId || row.patientId || ''}-${index}`;

                    const handleClick = () => {
                        if (typeof onItemClick === 'function') {
                            onItemClick({
                                kind,
                                patientId: row.PatientId ?? row.patientId,
                                specimenId: row.SpecimenId ?? row.specimenId,
                                testId: row.TestId ?? row.testId,
                                testName: row.TestName ?? row.testName,
                                cultureId: row.CultureId ?? row.cultureId,
                            });
                        }
                    };

                    const typeId = row.SpecimenTypeId ?? row.specimenTypeId;
                    const specTypeLabel = listItemText(lists, 'specimentype', typeId);
                    const cultureTypeId = row.CultureTypeId ?? row.cultureTypeId;
                    const cultureTypeLabel = listItemText(lists, 'culturetype', cultureTypeId);
                    const rawTestName = row.TestName || row.testName || '';
                    const testLine = directTestDisplayLabel(lists, rawTestName) || rawTestName;

                    return (
                        <div
                            key={key}
                            role="button"
                            tabIndex={0}
                            className={cls}
                            onClick={handleClick}
                            onKeyDown={(e) => {
                                if (e.key === 'Enter' || e.key === ' ') {
                                    e.preventDefault();
                                    handleClick();
                                }
                            }}
                        >
                            {kind === 'specimen' && (
                                <>
                                    <div className="home-recent-line home-recent-line--primary">{patientName(row)}</div>
                                    <div className="home-recent-line">{row.AccessionNumber || row.accessionNumber || ''}</div>
                                    <div className="home-recent-line">{specTypeLabel}</div>
                                    <div className="home-recent-line">{row.PatientRef || row.patientRef || ''}</div>
                                </>
                            )}
                            {kind === 'patient' && (
                                <>
                                    <div className="home-recent-line home-recent-line--primary">{patientName(row)}</div>
                                    <div className="home-recent-line">{row.PatientRef || row.patientRef || ''}</div>
                                </>
                            )}
                            {kind === 'test' && (
                                <>
                                    <div className="home-recent-line home-recent-line--primary">{patientName(row)}</div>
                                    <div className="home-recent-line">{testLine}</div>
                                    <div className="home-recent-line">{row.AccessionNumber || row.accessionNumber || ''}</div>
                                    <div className="home-recent-line">{row.PatientRef || row.patientRef || ''}</div>
                                </>
                            )}
                            {kind === 'culture' && (
                                <>
                                    <div className="home-recent-line home-recent-line--primary">{patientName(row)}</div>
                                    <div className="home-recent-line">{cultureTypeLabel}</div>
                                    <div className="home-recent-line">{row.AccessionNumber || row.accessionNumber || ''}</div>
                                    <div className="home-recent-line">{row.CultureNumber ?? row.cultureNumber ?? ''}</div>
                                    <div className="home-recent-line">{row.PatientRef || row.patientRef || ''}</div>
                                </>
                            )}
                        </div>
                    );
                })}
            </div>
        </div>
    );
};

export default HomeRecentlyUsedTile;
