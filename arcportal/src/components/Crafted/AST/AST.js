import React, { useState, useEffect, useRef, useMemo } from 'react';
import { connect } from 'react-redux';
import './AST.css';
import { PrimaryButton, DefaultButton } from '@fluentui/react';
import { Dialog, DialogType, DialogFooter } from '@fluentui/react/lib/Dialog';
import SingleLineField from '../../Forms/SingleLineField/SingleLineField';
import { runTestPatternQuery } from './ASTQueries';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import AlertList from '../../General/AlertList/AlertList';
import Post from '../../../Data/Post';
import ASTHeader from './ASTHeader/ASTHeader';
import ASTDiskResults from './ASTDiskResults/ASTDiskResults';
import ASTMicResults from './ASTMicResults/ASTMicResults';
import ASTExpertRules from './ASTExpertRules/ASTExpertRules';
import ASTIsolateTests from './ASTIsolateTests/ASTIsolateTests';
import ASTComments from './ASTComments/ASTComments';
import FormHandler from '../../Containers/FormHandler/FormHandler';
import { roundToValidMIC } from './ASTMicField/micRounding';
import {
    buildMicMeasurementForSave,
    normalizeMicRowEdit,
    parseMicMeasurementFromRow
} from './ASTMicField/micMeasurementUtils';
import { buildExpertRuleColourMap, getExpertRuleColour } from './expertRuleColours';
import ExpertTriggerIcon from './ExpertTriggerIcon/ExpertTriggerIcon';
import ASTSusceptibilityOverridePanel from './ASTSusceptibilityOverridePanel/ASTSusceptibilityOverridePanel';
import getLoggedInUsername from '../../../Utils/General/getLoggedInUsername';
import {
    isRowManuallySetSusceptibility,
    mapSusceptibilityOverrideForSave,
} from './astSusceptibilityOverrideUtils';

    const testPatternConfig =
    {
        Id: 'testpattern', Type: 'dropdown', Label: '', Required: false, Placeholder: "", value: "", noTab: true,
        Options: []
    };

    const testPatternFullListConfig =
    {
        Id: 'testpatternfulllist', Type: 'dropdown', Label: '', Required: false, Placeholder: "", value: "", noTab: true,
        Options: []
    };

    const ASTCommentOneConfig =
    {
        Id: 'astcommentone', Type: 'combobox', Label: '', Required: false, value: "", Options: []
    };

    const ASTCommentTwoConfig =
    {
        Id: 'astcommenttwo', Type: 'combobox', Label: '', Required: false, value: "", Options: []
    };

    const ASTAdditionalNotesConfig =
    {
        Id: 'astadditionalnotes', Type: 'multiline', Label: '', Required: false, value: ""
    };

    const completedDateConfig =
    {
        Id: 'completeddate', Type: 'date', Label: '', Required: false, value: ""
    };

    const completedTimeConfig =
    {
        Id: 'completedtime', Type: 'time', Label: '', Required: false, value: ""
    };
    
    const deleteBlankRowsConfig =
    {
        Id: 'deleteblankrows', Type: 'toggle', Label: '', value: 'Yes'
    };

const BACKEND_MEASUREMENT_SENTINEL = -1;

/**
 * Clears Measurement sentinel (-1) from a disk/MIC row for UI display. The server stores -1 when no
 * measurement was entered; the craft save path mirrors that. Reload must show blank fields, not -1.
 * @param {Object} row - AST disk/MIC row or embedded row
 * @returns {Object} Shallow-cloned row with sentinels cleared
 */
function normalizeBackendMeasurementSentinelRow(row) {
    if (!row) {
        return row;
    }
    const next = { ...row };
    const zd = next.ZoneDiameter;
    if (zd === BACKEND_MEASUREMENT_SENTINEL || zd === '-1') {
        next.ZoneDiameter = '';
    }
    const m = next.Mic;
    let micIsSentinel = m === BACKEND_MEASUREMENT_SENTINEL || m === '-1';
    if (!micIsSentinel && m !== '' && m != null && (typeof m === 'number' || typeof m === 'string')) {
        const n = Number(m);
        if (!Number.isNaN(n) && n === BACKEND_MEASUREMENT_SENTINEL) {
            micIsSentinel = true;
        }
    }
    if (micIsSentinel) {
        next.Mic = '';
        next.Operator = '';
    }
    if (Array.isArray(next.EmbeddedASTRows) && next.EmbeddedASTRows.length > 0) {
        next.EmbeddedASTRows = next.EmbeddedASTRows.map(normalizeBackendMeasurementSentinelRow);
    }
    return next;
}

/**
 * Ensures at least one empty Disk and one empty Mic row when the server sends empty arrays (manual entry UX).
 * Normalizes `ExpertRuleGroups` and `ExpertRuleCommentAlerts` when present.
 * @param {Object} ASTData - Parsed AST craft payload
 * @returns {Object} Copy with DiskResults/MicResults populated when empty
 */
function ensureDefaultDiskMicRows(ASTData) {
    const disk = [...(ASTData.DiskResults || [])];
    const mic = [...(ASTData.MicResults || [])];
    if (disk.length === 0) {
        disk.push({
            Antibiotic: '',
            Dosage: '',
            DrugCategory: '',
            EmbeddedASTRows: [],
            ExpertRuleLine: false,
            ExpertRuleName: null,
            ExpertRuleText: null,
            Guidelines: '',
            IncludeOnReport: 'No',
            Mic: '',
            OrganismId: '',
            SpecialConsideration: null,
            SpecialConsiderationId: 973,
            TestMethod: 681,
            TestResult: '',
            ZoneDiameter: ''
        });
    }
    if (mic.length === 0) {
        mic.push({
            Antibiotic: '',
            Dosage: '',
            DrugCategory: '',
            EmbeddedASTRows: [],
            ExpertRuleLine: false,
            ExpertRuleName: null,
            ExpertRuleText: null,
            Guidelines: '',
            IncludeOnReport: 'No',
            Mic: '',
            OrganismId: '',
            SpecialConsideration: null,
            SpecialConsiderationId: 973,
            TestMethod: 680,
            TestResult: '',
            ZoneDiameter: ''
        });
    }
    const rawRm = ASTData.ResistanceMechanisms ?? ASTData.resistanceMechanisms;
    const resistanceMechanisms = Array.isArray(rawRm) ? [...rawRm] : [];
    const rawGroups = ASTData.ExpertRuleGroups ?? ASTData.expertRuleGroups;
    const expertRuleGroups = Array.isArray(rawGroups)
        ? rawGroups.map((g) => {
              const applyBool = g.ApplyRule ?? g.applyRule;
              const actions = g.Actions ?? g.actions ?? [];
              return {
                  ...g,
                  RuleName: g.RuleName ?? g.ruleName,
                  RuleText: g.RuleText ?? g.ruleText,
                  RuleId: g.RuleId ?? g.ruleId,
                  ApplyRule: applyBool,
                  Actions: actions.map((a) => ({
                      ...a,
                      ApplyRule: applyBool ? 'Yes' : 'No'
                  }))
              };
          })
        : rawGroups;
    const expertRuleCommentAlerts = normalizeExpertRuleCommentAlerts(ASTData);
    return {
        ...ASTData,
        DiskResults: disk.map(normalizeBackendMeasurementSentinelRow),
        MicResults: mic.map(normalizeBackendMeasurementSentinelRow),
        ResistanceMechanisms: resistanceMechanisms,
        ...(expertRuleGroups !== undefined ? { ExpertRuleGroups: expertRuleGroups } : {}),
        ...(expertRuleCommentAlerts !== undefined ? { ExpertRuleCommentAlerts: expertRuleCommentAlerts } : {})
    };
}

/**
 * Returns a copy of expert-rule comment alerts from the AST payload (PascalCase or camelCase API), or undefined if absent.
 * @param {Object} ASTData - Parsed AST craft payload
 * @returns {Array|undefined}
 */
function normalizeExpertRuleCommentAlerts(ASTData) {
    const raw = ASTData.ExpertRuleCommentAlerts ?? ASTData.expertRuleCommentAlerts;
    if (!Array.isArray(raw)) {
        return undefined;
    }
    return [...raw];
}

/**
 * Returns the trigger list (AST lines that fired the rule) from an expert rule group or comment alert,
 * tolerating PascalCase or camelCase from the API.
 * @param {Object} rule - Expert rule group or comment alert.
 * @returns {Array<Object>}
 */
function expertRuleTriggers(rule) {
    const triggers = rule?.Triggers ?? rule?.triggers;
    return Array.isArray(triggers) ? triggers : [];
}
/**
 * Normalizes a special consideration id for trigger-line matching: the &quot;no special consideration&quot; sentinel (973)
 * is treated as 0 so it lines up with a parent Disk/MIC line.
 * @param {number|string} scId
 * @returns {number}
 */
function normalizeTriggerSpecialConsiderationId(scId) {
    const n = Number(scId) || 0;
    return n === 973 ? 0 : n;
}

/**
 * Stable key identifying an AST line for expert-rule trigger matching. Uses ids only (test method, antibiotic id,
 * special consideration id) so it remains correct when tags/list items are translated.
 * @param {number|string} testMethod - 681 Disk or 680 MIC.
 * @param {number|string} antibioticId
 * @param {number|string} specialConsiderationId
 * @returns {string}
 */
function expertTriggerLineKey(testMethod, antibioticId, specialConsiderationId) {
    return `${Number(testMethod) || 0}:${Number(antibioticId) || 0}:${Number(specialConsiderationId) || 0}`;
}

/**
 * Builds a map from AST line identity to the expert rules linked to that line, used to draw the inline trigger icon
 * and colour-coded hover. The key is {@link expertTriggerLineKey}. Each value lists rules whose conditions
 * triggered the line and/or whose applied actions target that line (including group-expanded action antibiotics).
 * @param {Object} astData - AST UI state (ExpertRuleGroups + ExpertRuleCommentAlerts with Triggers).
 * @param {Object<string,string>} colourMap - From {@link buildExpertRuleColourMap}.
 * @returns {Object<string, Array<{ruleId:number, ruleName:string, ruleText:string, colour:string, hasActions:boolean}>>}
 */
function buildExpertLineTriggerMap(astData, colourMap) {
    const map = {};
    const addRuleToLineKey = (key, ruleId, ruleName, ruleText, colour, hasActions) => {
        if (!map[key]) {
            map[key] = [];
        }
        if (map[key].some((r) => r.ruleId === ruleId)) {
            return;
        }
        map[key].push({ ruleId, ruleName, ruleText, colour, hasActions });
    };
    const addConditionTriggers = (rule, hasActions) => {
        const ruleId = rule.RuleId ?? rule.ruleId;
        const ruleName = rule.RuleName ?? rule.ruleName ?? '';
        const ruleText = rule.RuleText ?? rule.ruleText ?? '';
        const colour = getExpertRuleColour(colourMap, ruleId);
        expertRuleTriggers(rule).forEach((t) => {
            const tm = t.TestMethod ?? t.testMethod ?? 0;
            const ab = t.AntibioticId ?? t.antibioticId ?? 0;
            const sc = normalizeTriggerSpecialConsiderationId(t.SpecialConsiderationId ?? t.specialConsiderationId ?? 0);
            addRuleToLineKey(expertTriggerLineKey(tm, ab, sc), ruleId, ruleName, ruleText, colour, hasActions);
        });
    };
    const addAppliedActionTargets = (g) => {
        const groupApplied = g.ApplyRule === true || g.ApplyRule === 'Yes';
        if (!groupApplied) {
            return;
        }
        const ruleId = g.RuleId ?? g.ruleId;
        const ruleName = g.RuleName ?? g.ruleName ?? '';
        const ruleText = g.RuleText ?? g.ruleText ?? '';
        const colour = getExpertRuleColour(colourMap, ruleId);
        const actions = g.Actions || g.actions || [];
        for (const a of actions) {
            if (isPrintOnReportOnlyExpertAction(a)) {
                continue;
            }
            const actionApplied =
                a.ApplyRule === 'Yes' ||
                a.ApplyRule === true ||
                (a.ApplyRule === undefined && groupApplied);
            if (!actionApplied) {
                continue;
            }
            const ab = a.Antibiotic ?? a.antibiotic;
            if (ab === undefined || ab === '' || ab === null) {
                continue;
            }
            const tm = a.TestMethod ?? a.testMethod;
            if (tm === DISK_TEST_METHOD || tm === MIC_TEST_METHOD) {
                addRuleToLineKey(expertTriggerLineKey(tm, ab, 0), ruleId, ruleName, ruleText, colour, true);
            } else {
                addRuleToLineKey(expertTriggerLineKey(DISK_TEST_METHOD, ab, 0), ruleId, ruleName, ruleText, colour, true);
                addRuleToLineKey(expertTriggerLineKey(MIC_TEST_METHOD, ab, 0), ruleId, ruleName, ruleText, colour, true);
            }
        }
    };
    (astData?.ExpertRuleGroups ?? astData?.expertRuleGroups ?? []).forEach((g) => {
        addConditionTriggers(g, true);
        addAppliedActionTargets(g);
    });
    (astData?.ExpertRuleCommentAlerts ?? astData?.expertRuleCommentAlerts ?? []).forEach((c) => addConditionTriggers(c, false));
    return map;
}

/**
 * Stable grouping key for an expert-rule AST row. Uses the rule id (`id:<ruleId>`) so two expert rules with the
 * same display name remain separate groups; falls back to the name (`name:<ruleName>`) only when the row carries
 * no usable rule id. Matching on id keeps grouping correct when tags/list items are translated.
 * @param {Object} row - AST row with ExpertRuleId / ExpertRuleName.
 * @param {string} ruleName - Resolved display name for the rule.
 * @returns {string}
 */
function expertGroupKeyForRow(row, ruleName) {
    const ruleId = row?.ExpertRuleId;
    return ruleId !== undefined && ruleId !== null && ruleId !== 0 ? `id:${ruleId}` : `name:${ruleName}`;
}

const DISK_TEST_METHOD = 681;
const MIC_TEST_METHOD = 680;

/**
 * Whether an expert rule action is print-on-report only (antibiotic set, no susceptibility).
 * Uses API flag when present; otherwise infers from TestResult === 0 (0 is not a valid susceptibility list id).
 *
 * @param {Object} a - Expert action row from ExpertRuleGroups.
 * @returns {boolean}
 */
function isPrintOnReportOnlyExpertAction(a) {
    if (!a) {
        return false;
    }
    if (a.isPrintOnReportOnlyExpertAction === true || a.IsPrintOnReportOnlyExpertAction === true) {
        return true;
    }
    if (a.isPrintOnReportOnlyExpertAction === false || a.IsPrintOnReportOnlyExpertAction === false) {
        return false;
    }
    const ab = a.Antibiotic ?? a.antibiotic;
    if (ab === undefined || ab === '' || ab === null || ab === 0) {
        return false;
    }
    const tr = a.TestResult ?? a.testResult;
    return tr === 0 || tr === '' || tr === null || tr === undefined;
}

/**
 * True when every action in the group is print-on-report only (used to merge ApplyRule on refresh from the server).
 *
 * @param {Object} group - Expert rule group from ExpertRuleGroups.
 * @returns {boolean}
 */
function expertRuleGroupIsPrintOnlyOnly(group) {
    const actions = group?.Actions ?? group?.actions ?? [];
    if (actions.length === 0) {
        return false;
    }
    return actions.every((a) => isPrintOnReportOnlyExpertAction(a));
}

/**
 * Normalizes display-on-report / include-on-report to Yes or No for manual AST rows.
 * Accepts Yes/No, True/False (strings from toggles or legacy payloads), and 1/0.
 *
 * @param {*} v - Raw toggle or string from expert action.
 * @returns {'Yes'|'No'}
 */
function normalizeIncludeOnReportValue(v) {
    if (v === true || v === 1) {
        return 'Yes';
    }
    if (v === false || v === 0) {
        return 'No';
    }
    if (v === null || v === undefined || v === '') {
        return 'No';
    }
    const s = String(v).trim();
    if (!s) {
        return 'No';
    }
    const lower = s.toLowerCase();
    if (lower === 'yes' || lower === 'true' || lower === '1') {
        return 'Yes';
    }
    if (lower === 'no' || lower === 'false' || lower === '0') {
        return 'No';
    }
    return 'No';
}

/** List item id for no special consideration (matches backend sentinel). */
const NO_SPECIAL_CONSIDERATION_ID = 973;

/**
 * @param {number|undefined|null} specialConsiderationId
 * @returns {boolean}
 */
function isValidSpecialConsiderationId(specialConsiderationId) {
    return (
        specialConsiderationId !== undefined &&
        specialConsiderationId !== null &&
        specialConsiderationId !== 0 &&
        specialConsiderationId !== NO_SPECIAL_CONSIDERATION_ID
    );
}

/**
 * @param {Object|undefined|null} row
 * @returns {number}
 */
function resolveSpecialConsiderationId(row) {
    return row?.SpecialConsiderationId ?? row?.specialConsiderationId ?? 0;
}

/**
 * @param {Object|undefined|null} row
 * @returns {string}
 */
function resolveRowIncludeOnReport(row) {
    return normalizeIncludeOnReportValue(row?.IncludeOnReport ?? row?.includeOnReport ?? 'No');
}

/**
 * @param {Object|undefined} embed
 * @returns {boolean}
 */
function embedIncludeOnReportNeedsInherit(embed) {
    const value = embed?.IncludeOnReport ?? embed?.includeOnReport;
    return value === undefined || value === null || value === '';
}

/**
 * When special-consideration embeds are first attached to a parent row, copy the parent's
 * IncludeOnReport into each new embed that has no stored value yet (inherit on appear).
 * Does not overwrite embeds that already have an explicit value from storage or user edit.
 *
 * @param {Object} parentRow - Parent disk/MIC row with EmbeddedASTRows.
 * @param {Array|undefined|null} previousEmbeds - Prior EmbeddedASTRows before merge; when omitted, only unset embeds inherit.
 * @returns {Object} Parent row clone with updated embed IncludeOnReport values.
 */
function inheritIncludeOnReportOnNewEmbeds(parentRow, previousEmbeds) {
    if (!parentRow || !Array.isArray(parentRow.EmbeddedASTRows) || parentRow.EmbeddedASTRows.length === 0) {
        return parentRow;
    }
    const parentInclude = normalizeIncludeOnReportValue(parentRow.IncludeOnReport ?? 'No');
    const hasPrevious = previousEmbeds != null;
    const previousIds = new Set(
        (previousEmbeds || [])
            .filter((embed) => isValidSpecialConsiderationId(resolveSpecialConsiderationId(embed)))
            .map((embed) => resolveSpecialConsiderationId(embed))
    );
    const embedded = parentRow.EmbeddedASTRows.map((embed) => {
        const specialId = resolveSpecialConsiderationId(embed);
        if (!isValidSpecialConsiderationId(specialId)) {
            return embed;
        }
        const isNew = !hasPrevious || !previousIds.has(specialId);
        if (isNew || embedIncludeOnReportNeedsInherit(embed)) {
            return { ...embed, IncludeOnReport: parentInclude };
        }
        return embed;
    });
    return { ...parentRow, EmbeddedASTRows: embedded };
}

/**
 * After susceptibility lookup refreshes embed rows from the server, keep each embed's IncludeOnReport
 * when that special consideration was already on screen (independent toggle; server defaults to No).
 *
 * @param {Object} parentRow - Parent row after server merge and inherit-on-appear.
 * @param {Array|undefined|null} previousEmbeds - EmbeddedASTRows before the susceptibility response.
 * @returns {Object} Parent row with embed IncludeOnReport preserved where applicable.
 */
function preserveEmbedIncludeOnReportAfterSusceptibilityMerge(parentRow, previousEmbeds) {
    if (!parentRow || !Array.isArray(parentRow.EmbeddedASTRows) || !previousEmbeds?.length) {
        return parentRow;
    }
    const previousBySpecialId = new Map();
    previousEmbeds.forEach((embed) => {
        const specialId = resolveSpecialConsiderationId(embed);
        if (isValidSpecialConsiderationId(specialId)) {
            previousBySpecialId.set(specialId, embed);
        }
    });
    if (previousBySpecialId.size === 0) {
        return parentRow;
    }
    const embedded = parentRow.EmbeddedASTRows.map((embed) => {
        const specialId = resolveSpecialConsiderationId(embed);
        const previous = previousBySpecialId.get(specialId);
        if (!previous) {
            return embed;
        }
        const previousInclude = previous.IncludeOnReport ?? previous.includeOnReport;
        if (previousInclude === undefined || previousInclude === null || previousInclude === '') {
            return embed;
        }
        return { ...embed, IncludeOnReport: normalizeIncludeOnReportValue(previousInclude) };
    });
    return { ...parentRow, EmbeddedASTRows: embedded };
}

/**
 * Applies inherit-on-appear for all disk/MIC rows, comparing each row to optional previous AST state.
 * Parent rows are matched by id-based line key, not array index.
 *
 * @param {Object} astData - AST craft payload with DiskResults and MicResults.
 * @param {Object|undefined} previousAstData - Prior AST state for embed id comparison (e.g. on test pattern change).
 * @returns {Object} Updated AST payload.
 */
function applyInheritIncludeOnReportToAstData(astData, previousAstData) {
    if (!astData) {
        return astData;
    }
    const prevDisk = previousAstData?.DiskResults || [];
    const prevMic = previousAstData?.MicResults || [];
    const prevDiskByKey = new Map(prevDisk.map((row) => [buildAstRowLineKey(row), row]));
    const prevMicByKey = new Map(prevMic.map((row) => [buildAstRowLineKey(row), row]));
    return {
        ...astData,
        DiskResults: (astData.DiskResults || []).map((row) => {
            const prevRow = prevDiskByKey.get(buildAstRowLineKey(row));
            return inheritIncludeOnReportOnNewEmbeds(row, prevRow?.EmbeddedASTRows);
        }),
        MicResults: (astData.MicResults || []).map((row) => {
            const prevRow = prevMicByKey.get(buildAstRowLineKey(row));
            return inheritIncludeOnReportOnNewEmbeds(row, prevRow?.EmbeddedASTRows);
        })
    };
}

/**
 * Disk (681) and/or MIC (680) test methods targeted by an expert action row.
 *
 * @param {Object} a - Expert action row with TestMethod.
 * @returns {number[]} 681, 680, or both when TestMethod is unknown.
 */
function resolveExpertActionTestMethods(a) {
    const tm = a.TestMethod ?? a.testMethod;
    if (tm === DISK_TEST_METHOD || tm === MIC_TEST_METHOD) {
        return [tm];
    }
    return [DISK_TEST_METHOD, MIC_TEST_METHOD];
}

/**
 * Undo map key for print-only expert IncludeOnReport overrides.
 *
 * @param {number} ruleId
 * @param {number} testMethod - 681 or 680
 * @param {string|number} antibioticId
 * @returns {string}
 */
function printOnReportExpertUndoKey(ruleId, testMethod, antibioticId) {
    return `por|${ruleId}|${testMethod}|${String(antibioticId)}`;
}

/**
 * Parses an applied-rule undo map key from {@link handleApplyRuleChange} (`${type}-${parentIndex}-${specialIndex}`).
 *
 * @param {string} key
 * @returns {{ type: string, parentIndex: number, specialIndex: number }|null}
 */
function parseAppliedRuleUndoKey(key) {
    const match = /^(\w+)-(\d+)-(\d+)$/.exec(key);
    if (!match) {
        return null;
    }
    return {
        type: match[1],
        parentIndex: parseInt(match[2], 10),
        specialIndex: parseInt(match[3], 10)
    };
}

/**
 * Removes expert apply undo entries and print-only IncludeOnReport undo after a Disk/MIC or embedded row delete.
 *
 * @param {'disk'|'mic'} type
 * @param {number} index - Parent row index in DiskResults or MicResults
 * @param {number|undefined} specialIndex - EmbeddedASTRows index when deleting a special-consideration embed
 * @param {Object|null|undefined} deletedRow - Row snapshot captured before splice (for antibiotic / undo cleanup)
 * @param {{ current: Map<string, Object> }} appliedRuleUndoRef
 * @param {{ current: Map<string, { originalIncludeOnReport: string }> }} printOnReportExpertUndoRef
 */
function pruneExpertRuleUndoStateAfterRowRemoval(
    type,
    index,
    specialIndex,
    deletedRow,
    appliedRuleUndoRef,
    printOnReportExpertUndoRef
) {
    const nextApplied = new Map();
    for (const [key, value] of appliedRuleUndoRef.current.entries()) {
        const parsed = parseAppliedRuleUndoKey(key);
        if (!parsed || parsed.type !== type) {
            nextApplied.set(key, value);
            continue;
        }
        if (specialIndex === undefined) {
            if (parsed.parentIndex === index) {
                continue;
            }
            if (parsed.parentIndex > index) {
                nextApplied.set(
                    `${type}-${parsed.parentIndex - 1}-${parsed.specialIndex}`,
                    value
                );
                continue;
            }
        } else if (parsed.parentIndex === index) {
            if (parsed.specialIndex === specialIndex) {
                continue;
            }
            if (parsed.specialIndex > specialIndex) {
                nextApplied.set(
                    `${type}-${index}-${parsed.specialIndex - 1}`,
                    value
                );
                continue;
            }
        }
        nextApplied.set(key, value);
    }
    appliedRuleUndoRef.current = nextApplied;

    const ab = deletedRow?.Antibiotic;
    if (ab !== undefined && ab !== '' && ab !== null) {
        const tm = type === 'disk' ? DISK_TEST_METHOD : MIC_TEST_METHOD;
        const suffix = `|${tm}|${String(ab)}`;
        for (const key of [...printOnReportExpertUndoRef.current.keys()]) {
            if (key.endsWith(suffix)) {
                printOnReportExpertUndoRef.current.delete(key);
            }
        }
    }
}

/**
 * Removes manual rows added by expert apply/unapply when the parent rule key prefix is invalidated.
 *
 * @param {Object} newData - AST state (DiskResults / MicResults mutated in place)
 * @param {string} appliedFromRulePrefix - e.g. `disk-2-`
 */
function removeRowsWithAppliedFromRulePrefix(newData, appliedFromRulePrefix) {
    for (const arrName of ['DiskResults', 'MicResults']) {
        const arr = newData[arrName];
        if (!Array.isArray(arr)) {
            continue;
        }
        for (let i = arr.length - 1; i >= 0; i--) {
            const row = arr[i];
            if (row?.AppliedFromRule && String(row.AppliedFromRule).startsWith(appliedFromRulePrefix)) {
                arr.splice(i, 1);
            }
        }
    }
}

/**
 * Merges server expert groups with previous client state: (1) print-only-only groups keep Apply Rule when
 * the server sends false but the user had applied; (2) if the user explicitly turned Apply off, keep off
 * when the server sends true (default-on) so debounced refresh does not re-enable.
 *
 * @param {Array|undefined} prevGroups - Previous ExpertRuleGroups from client state.
 * @param {Array} serverGroups - Groups from POST AST/getexpertrulesforpage.
 * @returns {Array} Merged groups with consistent ApplyRule on actions.
 */
function expertActionMatchesById(a, b) {
    const idOf = (x, ...keys) => {
        for (const k of keys) {
            if (x[k] !== undefined && x[k] !== null) {
                return String(x[k]);
            }
        }
        return '';
    };
    return (
        idOf(a, 'AntibioticId', 'antibioticId', 'Antibiotic', 'antibiotic') ===
            idOf(b, 'AntibioticId', 'antibioticId', 'Antibiotic', 'antibiotic') &&
        idOf(a, 'AntibioticGroupId', 'antibioticGroupId') === idOf(b, 'AntibioticGroupId', 'antibioticGroupId') &&
        idOf(a, 'SusceptibilityId', 'susceptibilityId', 'TestResult', 'testResult') ===
            idOf(b, 'SusceptibilityId', 'susceptibilityId', 'TestResult', 'testResult')
    );
}

/**
 * Maps a server expert action onto the merged group: applies the group ApplyRule, stamps the editable
 * Include-on-report flag when the server value is unset, and preserves the user's prior toggle choice
 * (matched by ids, never translated values) so a debounced refresh does not discard it.
 *
 * @param {Object} serverAction - Action from POST AST/getexpertrulesforpage.
 * @param {string} applyStr - 'Yes' or 'No' for the merged group.
 * @param {Array} prevActions - Previous client actions for the same rule (by RuleId).
 * @returns {Object} Merged action.
 */
function mergeExpertActionPreserveIncludeOnReport(serverAction, applyStr, prevActions) {
    const merged = { ...serverAction, ApplyRule: applyStr };
    const serverIor = serverAction.IncludeOnReport ?? serverAction.includeOnReport;
    const serverUnset = serverIor === null || serverIor === undefined || serverIor === '';
    if (serverUnset) {
        merged.IncludeOnReportEditable = true;
        const prevMatch = (prevActions || []).find(
            (pa) => pa.IncludeOnReportEditable === true && expertActionMatchesById(pa, serverAction)
        );
        const prevIor = prevMatch ? prevMatch.IncludeOnReport ?? prevMatch.includeOnReport : undefined;
        if (prevIor !== null && prevIor !== undefined && prevIor !== '') {
            merged.IncludeOnReport = prevIor;
        }
    }
    return merged;
}

function mergeExpertRuleGroupsPreservePrintOnlyApply(prevGroups, serverGroups) {
    if (!Array.isArray(serverGroups)) {
        return serverGroups;
    }
    const prevByRuleId = new Map((prevGroups || []).map((g) => [g.RuleId ?? g.ruleId, g]));
    return serverGroups.map((sg) => {
        const rid = sg.RuleId ?? sg.ruleId;
        const prev = prevByRuleId.get(rid);
        const prevActions = prev ? prev.Actions ?? prev.actions ?? [] : [];
        const serverApply = sg.ApplyRule === true || sg.ApplyRule === 'Yes';
        const prevAr = prev ? prev.ApplyRule ?? prev.applyRule : undefined;
        const prevExplicitlyOff =
            prev &&
            (prevAr === false ||
                prevAr === 'No' ||
                (typeof prevAr === 'string' && prevAr.trim().toLowerCase() === 'no'));
        if (prevExplicitlyOff) {
            const applyStr = 'No';
            const actions = sg.Actions ?? sg.actions ?? [];
            const mergedActions = actions.map((a) =>
                mergeExpertActionPreserveIncludeOnReport(a, applyStr, prevActions)
            );
            return {
                ...sg,
                ApplyRule: false,
                Actions: mergedActions
            };
        }
        const prevApply = prev && (prev.ApplyRule === true || prev.ApplyRule === 'Yes');
        const finalGroupApply =
            serverApply || (!serverApply && expertRuleGroupIsPrintOnlyOnly(sg) && prevApply);
        const applyStr = finalGroupApply ? 'Yes' : 'No';
        const actions = sg.Actions ?? sg.actions ?? [];
        const mergedActions = actions.map((a) =>
            mergeExpertActionPreserveIncludeOnReport(a, applyStr, prevActions)
        );
        return {
            ...sg,
            ApplyRule: finalGroupApply,
            Actions: mergedActions
        };
    });
}

/**
 * Applies or reverts print-only expert rule effects on manual Disk/MIC rows (IncludeOnReport sync and undo).
 *
 * @param {Object} newData - AST state clone to mutate (DiskResults / MicResults replaced).
 * @param {Object} grp - Expert rule group before ApplyRule mutation.
 * @param {number} ruleId
 * @param {boolean} applyYes - True when applying the rule, false when unapplying.
 * @param {{ current: Map<string, { originalIncludeOnReport: string }> }} undoRef - Ref whose map is mutated for undo.
 */
function applyPrintOnlyExpertGroupToManualRows(newData, grp, ruleId, applyYes, undoRef) {
    const disk = [...(newData.DiskResults || [])].map((r) => ({ ...r }));
    const mic = [...(newData.MicResults || [])].map((r) => ({ ...r }));
    const actions = grp.Actions || grp.actions || [];

    if (applyYes) {
        for (const a of actions) {
            if (!isPrintOnReportOnlyExpertAction(a)) {
                continue;
            }
            const rawTarget = a.IncludeOnReport ?? a.includeOnReport ?? a.DisplayOnReport ?? a.displayOnReport;
            // Unset print-on-report defaults to Yes (matching the AST include-on-report toggle default) rather than No.
            const target =
                rawTarget === null || rawTarget === undefined || rawTarget === ''
                    ? 'Yes'
                    : normalizeIncludeOnReportValue(rawTarget);
            const ab = a.Antibiotic ?? a.antibiotic;
            if (ab === undefined || ab === '' || ab === null) {
                continue;
            }
            const tms = resolveExpertActionTestMethods(a);
            for (const tm of tms) {
                const arr = tm === DISK_TEST_METHOD ? disk : mic;
                for (let i = 0; i < arr.length; i++) {
                    const row = arr[i];
                    if (row.ExpertRuleLine) {
                        continue;
                    }
                    if (String(row.Antibiotic) !== String(ab)) {
                        continue;
                    }
                    const key = printOnReportExpertUndoKey(ruleId, tm, ab);
                    if (!undoRef.current.has(key)) {
                        undoRef.current.set(key, { originalIncludeOnReport: row.IncludeOnReport ?? 'No' });
                    }
                    arr[i] = { ...row, IncludeOnReport: target };
                }
            }
        }
    } else {
        const prefix = `por|${ruleId}|`;
        const keysToRemove = [];
        for (const key of undoRef.current.keys()) {
            if (!key.startsWith(prefix)) {
                continue;
            }
            const parts = key.split('|');
            if (parts.length < 4) {
                continue;
            }
            const tm = parseInt(parts[2], 10);
            const ab = parts[3];
            const undo = undoRef.current.get(key);
            if (!undo) {
                keysToRemove.push(key);
                continue;
            }
            const arr = tm === DISK_TEST_METHOD ? disk : mic;
            for (let i = 0; i < arr.length; i++) {
                const row = arr[i];
                if (row.ExpertRuleLine) {
                    continue;
                }
                if (String(row.Antibiotic) !== ab) {
                    continue;
                }
                arr[i] = { ...row, IncludeOnReport: undo.originalIncludeOnReport };
            }
            keysToRemove.push(key);
        }
        keysToRemove.forEach((k) => undoRef.current.delete(k));
    }
    newData.DiskResults = disk;
    newData.MicResults = mic;
}

/**
 * Whether the manual row's Include on report toggle is locked by an applied print-only expert rule.
 *
 * @param {Array} rows - DiskResults or MicResults
 * @param {'disk'|'mic'} section
 * @param {number} rowIndex
 * @param {Set<string>} printOnlyKeys - From {@link collectAppliedPrintOnlyExpertKeys}.
 * @returns {boolean}
 */
function isRowIncludeOnReportLockedByPrintOnlyExpert(rows, section, rowIndex, printOnlyKeys) {
    const tm = section === 'disk' ? DISK_TEST_METHOD : MIC_TEST_METHOD;
    const row = rows[rowIndex];
    if (!row || row.ExpertRuleLine) {
        return false;
    }
    if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
        return false;
    }
    return printOnlyKeys.has(`${tm}:${String(row.Antibiotic)}`);
}

/**
 * Collects (TestMethod, Antibiotic) pairs for applied print-only expert actions (Include on report lock).
 *
 * @param {Object} astData - Current AST UI state.
 * @returns {Set<string>} Keys `${testMethod}:${antibioticId}`.
 */
function collectAppliedPrintOnlyExpertKeys(astData) {
    const set = new Set();
    if (!astData) {
        return set;
    }
    const erg = astData.ExpertRuleGroups ?? astData.expertRuleGroups ?? [];
    for (const g of erg) {
        const groupApplied = g.ApplyRule === true || g.ApplyRule === 'Yes';
        if (!groupApplied) {
            continue;
        }
        const actions = g.Actions || g.actions || [];
        for (const a of actions) {
            if (!isPrintOnReportOnlyExpertAction(a)) {
                continue;
            }
            const actionApplied =
                a.ApplyRule === 'Yes' ||
                a.ApplyRule === true ||
                (a.ApplyRule === undefined && groupApplied);
            if (!actionApplied) {
                continue;
            }
            const ab = a.Antibiotic ?? a.antibiotic;
            if (ab === undefined || ab === '' || ab === null) {
                continue;
            }
            const tms = resolveExpertActionTestMethods(a);
            for (const tm of tms) {
                set.add(`${tm}:${String(ab)}`);
            }
        }
    }
    return set;
}

/**
 * Collects (TestMethod, Antibiotic) pairs for expert rule actions that are applied (Apply Rule = Yes).
 * Used to grey out / omit matching manual Disk or MIC rows.
 *
 * @param {Object} astData - Current AST UI state (DiskResults, MicResults, ExpertRuleGroups).
 * @param {Array} [diskRowsOverride] - Optional disk rows when patternRowsRef overrides astData.DiskResults.
 * @param {Array} [micRowsOverride] - Optional mic rows when patternRowsRef overrides astData.MicResults.
 * @returns {Set<string>} Keys formatted `${testMethod}:${antibioticId}`.
 * When an `ExpertRuleGroups` action has no 681/680 TestMethod, both Disk and MIC keys are added for that antibiotic (defence in depth).
 */
function collectAppliedExpertAntibioticKeys(astData, diskRowsOverride, micRowsOverride) {
    const set = new Set();
    if (!astData) {
        return set;
    }
    const organismId = Number(astData.OrganismId ?? astData.organismId ?? 0);
    if (organismId === 0) {
        return set;
    }
    const erg = astData.ExpertRuleGroups ?? astData.expertRuleGroups ?? [];
    for (const g of erg) {
        const groupApplied = g.ApplyRule === true || g.ApplyRule === 'Yes';
        if (!groupApplied) {
            continue;
        }
        const actions = g.Actions || g.actions || [];
        for (const a of actions) {
            if (isPrintOnReportOnlyExpertAction(a)) {
                continue;
            }
            const actionApplied =
                a.ApplyRule === 'Yes' ||
                a.ApplyRule === true ||
                (a.ApplyRule === undefined && groupApplied);
            if (!actionApplied) {
                continue;
            }
            const ab = a.Antibiotic;
            if (ab === undefined || ab === '' || ab === null) {
                continue;
            }
            const tm = a.TestMethod;
            if (tm === DISK_TEST_METHOD || tm === MIC_TEST_METHOD) {
                set.add(`${tm}:${String(ab)}`);
            } else {
                // Defence: expert actions may still have TestMethod 0 when conditions/actions omit it; match both sections.
                set.add(`${DISK_TEST_METHOD}:${String(ab)}`);
                set.add(`${MIC_TEST_METHOD}:${String(ab)}`);
            }
        }
    }
    return set;
}

/**
 * @param {Array} rows - DiskResults or MicResults array being displayed/saved.
 * @param {'disk'|'mic'} section
 * @param {number} rowIndex
 * @param {Set<string>} appliedKeys - From {@link collectAppliedExpertAntibioticKeys}.
 * @returns {boolean}
 */
function isManualRowSuppressedByExpertRule(rows, section, rowIndex, appliedKeys) {
    const tm = section === 'disk' ? DISK_TEST_METHOD : MIC_TEST_METHOD;
    const row = rows[rowIndex];
    if (!row || row.ExpertRuleLine) {
        return false;
    }
    if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
        return false;
    }
    return appliedKeys.has(`${tm}:${String(row.Antibiotic)}`);
}

/**
 * Dedupes expert-rule AST result rows by test method, antibiotic, and rule id.
 * @param {number} testMethod
 * @param {string|number} antibioticId
 * @param {number} expertRuleId
 */
function expertAstResultDedupeKey(testMethod, antibioticId, expertRuleId) {
    return `${testMethod}:${String(antibioticId)}:${String(expertRuleId)}`;
}

/**
 * Appends `ASTResults` rows for applied {@link ExpertRuleGroups} actions. Expert applications are not persisted from
 * `EmbeddedASTRows` after the page-scoped expert endpoint; this is the save path for those rows.
 *
 * @param {Array<Object>} results - Mutable list from disk/MIC walk.
 * @param {Object} effectiveAst - AST state including ExpertRuleGroups / expertRuleGroups.
 */
function appendAstResultsFromExpertRuleGroups(results, effectiveAst) {
    const erg = effectiveAst?.ExpertRuleGroups ?? effectiveAst?.expertRuleGroups;
    if (!Array.isArray(erg) || erg.length === 0) {
        return;
    }
    const existing = new Set();
    for (const r of results) {
        if (r.ExpertRuleLine && r.ExpertRuleId) {
            existing.add(expertAstResultDedupeKey(Number(r.TestMethod), r.Antibiotic, r.ExpertRuleId));
        }
    }

    const tryPush = (tm, ruleId, ab, a) => {
        const k = expertAstResultDedupeKey(tm, ab, ruleId);
        if (existing.has(k)) {
            return;
        }
        existing.add(k);
        const isDisk = tm === DISK_TEST_METHOD;
        results.push({
            TestType: isDisk ? 'Disk' : 'Strip',
            TestMethod: String(tm),
            EntryType: 'manual',
            Antibiotic: ab,
            Dosage: isDisk ? (a.Dosage === '' || a.Dosage === undefined ? '0' : a.Dosage) : '0',
            Guidelines: a.Guidelines ?? a.guidelines ?? 0,
            Measurement: isDisk
                ? a.ZoneDiameter === '' || a.ZoneDiameter === null || a.ZoneDiameter === undefined
                    ? -1
                    : a.ZoneDiameter
                : a.Mic === '' || a.Mic === null || a.Mic === undefined
                  ? -1
                  : a.Mic,
            Susceptibility: a.TestResult ?? a.testResult,
            Category: a.DrugCategory ?? a.drugCategory,
            IncludeInReport: a.IncludeOnReport ?? a.includeOnReport ?? 'Yes',
            ExpertRuleLine: true,
            ExpertRuleId: ruleId,
            SpecialRows: null
        });
    };

    for (const g of erg) {
        const groupApplied = g.ApplyRule === true || g.ApplyRule === 'Yes';
        if (!groupApplied) {
            continue;
        }
        const ruleId = g.RuleId ?? g.ruleId;
        const actions = g.Actions || g.actions || [];
        for (const a of actions) {
            const actionApplied =
                a.ApplyRule === 'Yes' ||
                a.ApplyRule === true ||
                (a.ApplyRule === undefined && groupApplied);
            if (!actionApplied) {
                continue;
            }
            if (isPrintOnReportOnlyExpertAction(a)) {
                continue;
            }
            const ab = a.Antibiotic ?? a.antibiotic;
            if (ab === undefined || ab === '' || ab === null) {
                continue;
            }
            const tm = a.TestMethod ?? a.testMethod;
            if (tm === DISK_TEST_METHOD || tm === MIC_TEST_METHOD) {
                tryPush(tm, ruleId, ab, a);
            } else {
                // Ambiguous test method: persist one AST row only; default to Disk until MIC can be inferred reliably.
                tryPush(DISK_TEST_METHOD, ruleId, ab, a);
            }
        }
    }
}

/**
 * Removes expert-rule lines from embedded rows (recursively) before POSTing disk/MIC state for expert-rule evaluation.
 * @param {Array<Object>} rows
 * @returns {Array<Object>}
 */
function stripExpertRuleLinesFromEmbedded(rows) {
    if (!Array.isArray(rows)) {
        return [];
    }
    return rows.map((row) => {
        const raw = row.EmbeddedASTRows;
        if (!Array.isArray(raw)) {
            return row;
        }
        return {
            ...row,
            EmbeddedASTRows: raw
                .filter((e) => !e.ExpertRuleLine)
                .map((e) => ({
                    ...e,
                    EmbeddedASTRows: stripExpertRuleLinesFromEmbedded(e.EmbeddedASTRows || [])
                }))
        };
    });
}

/**
 * True when a MIC row holds an operator prefix without a numeric measurement.
 * @param {Object} row - MIC row
 * @returns {boolean}
 */
function isOperatorOnlyMicRow(row) {
    if (!row) {
        return false;
    }
    const m = row.Mic;
    if (m === '' || m === null || m === undefined) {
        const op = row.Operator;
        return op === '>' || op === '<=' || op === '<';
    }
    const s = typeof m === 'string' ? m.trim() : String(m);
    if (s === '>' || s === '<=' || s === '<') {
        return true;
    }
    if (s.startsWith('<=')) {
        const numStr = s.slice(2).trim();
        return numStr === '' || Number.isNaN(parseFloat(numStr));
    }
    if (s.startsWith('>')) {
        const numStr = s.slice(1).trim();
        return numStr === '' || Number.isNaN(parseFloat(numStr));
    }
    if (s.startsWith('<')) {
        const numStr = s.slice(1).trim();
        return numStr === '' || Number.isNaN(parseFloat(numStr));
    }
    return false;
}

/**
 * Clears operator-only MIC values from a row snapshot used in craft save payloads.
 * @param {Object} row - MIC row
 * @returns {Object}
 */
function normalizeOperatorOnlyMicRow(row) {
    if (!isOperatorOnlyMicRow(row)) {
        return row;
    }
    return { ...row, Mic: '', Operator: '' };
}

/**
 * Builds Measurement for MIC save: combines Operator + numeric Mic when the API split them on load.
 * Operator-only values are sent through so server validation can return @AstMicMea@.
 * @param {Object} row - MIC row
 * @returns {string|number} Full value for server MicComparison/Measurement parsing (e.g. "<=2") or -1 if blank
 */
function measurementMicForSave(row) {
    return buildMicMeasurementForSave(row);
}

/**
 * Row for breakpoint lookup after a line edit (uses state already merged in newData).
 *
 * @param {'disk'|'mic'} type
 * @param {number} row
 * @param {number|undefined} specialrow
 * @param {Object} newData
 * @param {Object} value - Fallback row from the editor
 * @returns {Object}
 */
function resolveAstRowForBreakpointLookup(type, row, specialrow, newData, value) {
    if (specialrow !== undefined) {
        const parentArray = type === 'disk' ? newData.DiskResults : newData.MicResults;
        const parent = parentArray?.[row];
        return parent?.EmbeddedASTRows?.[specialrow] ?? value;
    }
    if (type === 'mic') {
        return newData.MicResults?.[row] ?? value;
    }
    if (type === 'disk') {
        return newData.DiskResults?.[row] ?? value;
    }
    return value;
}

/**
 * Applies breakpoint lookup response while keeping client-owned measurements (MIC, zone, operator).
 *
 * @param {Object|undefined} prevRow - Row state before the server response
 * @param {Object} serverRow - Parsed getsusceptibility row payload
 * @returns {Object}
 */
/** @param {*} testResult */
function astRowHasResolvedTestResult(testResult) {
    return testResult != null && testResult !== '' && testResult !== 0;
}

/**
 * Keeps susceptibility only on criteria-only POSTs (no zone/MIC on the request). When a measurement
 * was sent, an empty server TestResult means no matching band and must blank the combo.
 *
 * @param {Object|undefined} prevRow
 * @param {Object} mergedRow
 * @param {boolean} hadMeasurementOnRequest - Zone diameter or MIC was on the POST body
 * @returns {Object}
 */
function preserveTestResultIfServerUnmatched(prevRow, mergedRow, hadMeasurementOnRequest) {
    if (!prevRow || !mergedRow || hadMeasurementOnRequest) {
        return mergedRow;
    }
    if (astRowHasResolvedTestResult(mergedRow.TestResult)) {
        return mergedRow;
    }
    if (astRowHasResolvedTestResult(prevRow.TestResult)) {
        return { ...mergedRow, TestResult: prevRow.TestResult };
    }
    return mergedRow;
}

/** @param {'disk'|'mic'} type @param {number} index @param {number|undefined} specialIndex */
function getSusceptibilityRowKey(type, index, specialIndex) {
    return `${type}-${index}-${specialIndex ?? 'p'}`;
}

/**
 * Id-based metadata captured when {@link POST} AST/getsusceptibility is issued (never translated labels).
 *
 * @param {Object} row - Row payload sent to the server (parent line or special-consideration embed).
 * @param {string} parentRequestLineKey - {@link buildAstRowLineKey} for the parent when the request targets an embed.
 * @returns {{ requestLineKey: string, parentRequestLineKey: string, antibioticId: number, testMethodId: number, guidelinesId: number, specialConsiderationId: number }}
 */
function buildSusceptibilityRequestMeta(row, parentRequestLineKey = '') {
    return {
        requestLineKey: buildAstRowLineKey(row),
        parentRequestLineKey: parentRequestLineKey || '',
        antibioticId: Number(row?.Antibiotic) || 0,
        testMethodId: Number(row?.TestMethod) || 0,
        guidelinesId: Number(row?.Guidelines) || 0,
        specialConsiderationId: resolveSpecialConsiderationId(row),
    };
}

/**
 * @param {Object|undefined|null} row
 * @param {{ requestLineKey?: string, antibioticId?: number, testMethodId?: number, guidelinesId?: number }} requestMeta
 * @returns {boolean}
 */
function parentRowMatchesSusceptibilityRequest(row, requestMeta) {
    if (!row || row.ExpertRuleLine || !requestMeta) {
        return false;
    }
    if (requestMeta.requestLineKey) {
        return buildAstRowLineKey(row) === requestMeta.requestLineKey;
    }
    const antibioticId = Number(row.Antibiotic) || 0;
    if (requestMeta.antibioticId !== 0 && antibioticId !== requestMeta.antibioticId) {
        return false;
    }
    const testMethodId = Number(row.TestMethod) || 0;
    if (requestMeta.testMethodId !== 0 && testMethodId !== requestMeta.testMethodId) {
        return false;
    }
    const guidelinesId = Number(row.Guidelines) || 0;
    if (requestMeta.guidelinesId !== 0 && guidelinesId !== requestMeta.guidelinesId) {
        return false;
    }
    return antibioticId !== 0;
}

/**
 * @param {Object|undefined|null} embedRow
 * @param {{ specialConsiderationId?: number, antibioticId?: number }} requestMeta
 * @returns {boolean}
 */
function embedRowMatchesSusceptibilityRequest(embedRow, requestMeta) {
    if (!embedRow || !requestMeta) {
        return false;
    }
    const specialId = resolveSpecialConsiderationId(embedRow);
    const reqSpecialId = requestMeta.specialConsiderationId ?? 0;
    if (reqSpecialId !== 0 && specialId !== reqSpecialId) {
        return false;
    }
    const antibioticId = Number(embedRow.Antibiotic) || 0;
    if (requestMeta.antibioticId !== 0 && antibioticId !== requestMeta.antibioticId) {
        return false;
    }
    return reqSpecialId !== 0 || antibioticId !== 0;
}

/**
 * Resolves where a getsusceptibility response should be applied after row deletion or index shift.
 * Returns null when the target row no longer exists (stale response — discard).
 *
 * @param {Object} astData - Current AST craft state.
 * @param {'disk'|'mic'} type
 * @param {{ index: number, specialIndex?: number, requestMeta?: Object }} extraInfo
 * @returns {{ parentIndex: number, specialIndex: number|undefined, prevRow: Object } | null}
 */
function resolveSusceptibilityTargetLocation(astData, type, extraInfo) {
    const isDisk = type === 'disk';
    const rows = isDisk ? (astData.DiskResults || []) : (astData.MicResults || []);
    const requestMeta = extraInfo.requestMeta;
    const isEmbedded = extraInfo.specialIndex !== undefined && extraInfo.specialIndex !== null;

    if (isEmbedded) {
        const tryParentAt = (parentIndex) => {
            const parent = rows[parentIndex];
            if (!parent || parent.ExpertRuleLine) {
                return null;
            }
            if (requestMeta?.parentRequestLineKey && buildAstRowLineKey(parent) !== requestMeta.parentRequestLineKey) {
                return null;
            }
            const embeds = parent.EmbeddedASTRows || [];
            const embed = embeds[extraInfo.specialIndex];
            if (embed && embedRowMatchesSusceptibilityRequest(embed, requestMeta)) {
                return { parentIndex, specialIndex: extraInfo.specialIndex, prevRow: embed };
            }
            for (let ei = 0; ei < embeds.length; ei++) {
                if (embedRowMatchesSusceptibilityRequest(embeds[ei], requestMeta)) {
                    return { parentIndex, specialIndex: ei, prevRow: embeds[ei] };
                }
            }
            return null;
        };

        if (extraInfo.index < rows.length) {
            const atIndex = tryParentAt(extraInfo.index);
            if (atIndex) {
                return atIndex;
            }
        }
        for (let pi = 0; pi < rows.length; pi++) {
            if (pi === extraInfo.index) {
                continue;
            }
            const parent = rows[pi];
            if (!parent || parent.ExpertRuleLine) {
                continue;
            }
            if (requestMeta?.parentRequestLineKey && buildAstRowLineKey(parent) !== requestMeta.parentRequestLineKey) {
                continue;
            }
            const embeds = parent.EmbeddedASTRows || [];
            for (let ei = 0; ei < embeds.length; ei++) {
                if (embedRowMatchesSusceptibilityRequest(embeds[ei], requestMeta)) {
                    return { parentIndex: pi, specialIndex: ei, prevRow: embeds[ei] };
                }
            }
        }
        return null;
    }

    if (extraInfo.index < rows.length) {
        const row = rows[extraInfo.index];
        if (parentRowMatchesSusceptibilityRequest(row, requestMeta)) {
            return { parentIndex: extraInfo.index, specialIndex: undefined, prevRow: row };
        }
    }

    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        if (parentRowMatchesSusceptibilityRequest(row, requestMeta)) {
            return { parentIndex: i, specialIndex: undefined, prevRow: row };
        }
    }
    return null;
}

/**
 * Bumps susceptibility generation counters so in-flight getsusceptibility responses are discarded after row removal.
 *
 * @param {'disk'|'mic'} type
 * @param {number} removedIndex - Parent row index that was spliced out (or parent of deleted embed).
 * @param {number|undefined} specialIndex - When set, only the embed key at removedIndex is invalidated.
 * @param {{ current: Object<string, number> }} susceptibilitySeqRef
 */
function invalidateSusceptibilityRequestsAfterRowRemoval(type, removedIndex, specialIndex, susceptibilitySeqRef) {
    if (specialIndex !== undefined && specialIndex !== null) {
        const key = getSusceptibilityRowKey(type, removedIndex, specialIndex);
        susceptibilitySeqRef.current[key] = (susceptibilitySeqRef.current[key] ?? 0) + 1;
        return;
    }
    for (const key of Object.keys(susceptibilitySeqRef.current)) {
        const parts = key.split('-');
        if (parts.length < 3 || parts[0] !== type) {
            continue;
        }
        const idx = parseInt(parts[1], 10);
        if (!Number.isNaN(idx) && idx >= removedIndex) {
            susceptibilitySeqRef.current[key] = (susceptibilitySeqRef.current[key] ?? 0) + 1;
        }
    }
}

/**
 * Re-rounds a MIC row when the guideline changes if the target guideline requires rounding
 * (CLSI, EUCAST, or missing). Custom/non-standard guidelines leave the displayed MIC unchanged.
 *
 * @param {Object} row
 * @returns {Object}
 */
function reroundMicRowForGuideline(row) {
    if (!row || row.Mic == null || row.Mic === '') {
        return row;
    }
    const parts = parseMicMeasurementFromRow(row);
    if (!parts.hasNumericValue) {
        return row;
    }
    const numericValue = Number(parts.numeric);
    if (Number.isNaN(numericValue) || numericValue <= 0) {
        return row;
    }
    const guidelinesId = Number(row.Guidelines) || 0;
    const rounded = roundToValidMIC(numericValue, guidelinesId, parts.numericString);
    return { ...row, Mic: rounded, Operator: parts.operator || '' };
}

/**
 * Removes invalid expert-rule placeholder embeds from a parent row's EmbeddedASTRows.
 *
 * @param {Object} result - Parent disk/MIC row
 * @param {Array} newAlertMessages - Alert messages to append to
 * @param {number} parentIndex
 */
function filterInvalidEmbeddedAstRows(result, newAlertMessages, parentIndex) {
    const validRows = [];
    (result.EmbeddedASTRows || []).forEach((embedRow) => {
        const isInvalid = (embedRow.Antibiotic == null || embedRow.Antibiotic === 0) &&
            (embedRow.TestResult == null || embedRow.TestResult === 0);
        if (isInvalid) {
            if (embedRow.ExpertRuleLine && embedRow.ExpertRuleName) {
                newAlertMessages.push({
                    alertTypeId: 1,
                    colour: '#708090',
                    message: `${embedRow.ExpertRuleName}: ${embedRow.ExpertRuleText}`,
                    positionId: 998,
                    reportPositionId: 999,
                    specimenOrganism: '',
                    parentIndex: parentIndex
                });
            }
        } else {
            validRows.push(embedRow);
        }
    });
    result.EmbeddedASTRows = validRows;
}

function mergeEmbeddedAstRowsIncludeOnReport(prevEmbeds, serverEmbeds) {
    if (!Array.isArray(serverEmbeds)) {
        return prevEmbeds ?? serverEmbeds;
    }
    if (!Array.isArray(prevEmbeds) || prevEmbeds.length === 0) {
        return serverEmbeds;
    }
    return serverEmbeds.map((serverEmbed) => {
        const specialId = resolveSpecialConsiderationId(serverEmbed);
        if (!isValidSpecialConsiderationId(specialId)) {
            return serverEmbed;
        }
        const prevEmbed = prevEmbeds.find((e) => resolveSpecialConsiderationId(e) === specialId);
        if (!prevEmbed) {
            return serverEmbed;
        }
        const prevInclude = prevEmbed.IncludeOnReport ?? prevEmbed.includeOnReport;
        if (prevInclude === undefined || prevInclude === null || prevInclude === '') {
            return serverEmbed;
        }
        return { ...serverEmbed, IncludeOnReport: normalizeIncludeOnReportValue(prevInclude) };
    });
}

function mergeAstLineFromServerResponse(prevRow, serverRow) {
    if (!serverRow) {
        return prevRow ?? serverRow;
    }
    const merged = { ...serverRow };
    if (!prevRow) {
        return merged;
    }
    const prevMicEmpty = prevRow.Mic === '' || prevRow.Mic == null || prevRow.Mic === undefined;
    if (!prevMicEmpty) {
        merged.Mic = prevRow.Mic;
        if (prevRow.Operator != null && prevRow.Operator !== '') {
            merged.Operator = prevRow.Operator;
        }
    } else {
        merged.Mic = '';
        merged.Operator = '';
    }
    if (prevRow.ZoneDiameter != null && prevRow.ZoneDiameter !== '') {
        merged.ZoneDiameter = prevRow.ZoneDiameter;
    }
    const prevInclude = prevRow.IncludeOnReport ?? prevRow.includeOnReport;
    if (prevInclude !== undefined && prevInclude !== null && prevInclude !== '') {
        merged.IncludeOnReport = normalizeIncludeOnReportValue(prevInclude);
    }
    merged.EmbeddedASTRows = mergeEmbeddedAstRowsIncludeOnReport(
        prevRow.EmbeddedASTRows,
        merged.EmbeddedASTRows ?? prevRow.EmbeddedASTRows
    );
    if (prevRow.SusceptibilityOverride && prevRow.SusceptibilityOverride.ClearOverride !== true) {
        merged.SusceptibilityOverride = prevRow.SusceptibilityOverride;
    }
    return merged;
}

/**
 * Stable id-based key for matching a manual AST line to a test pattern line.
 * Disk (681) includes dosage; MIC (680) ignores dosage.
 *
 * @param {Object|undefined|null} row - Disk or MIC row from craft state.
 * @returns {string} Match key, or empty string when the row cannot be matched.
 */
function buildAstRowLineKey(row) {
    if (!row || row.ExpertRuleLine) {
        return '';
    }
    const antibioticId = Number(row.Antibiotic) || 0;
    const guidelinesId = Number(row.Guidelines) || 0;
    const testMethodId = Number(row.TestMethod) || 0;
    if (antibioticId === 0 || guidelinesId === 0) {
        return '';
    }
    if (testMethodId !== DISK_TEST_METHOD && testMethodId !== MIC_TEST_METHOD) {
        return '';
    }
    const dosage = testMethodId === DISK_TEST_METHOD ? (Number(row.Dosage) || 0) : 0;
    return `${testMethodId}|${antibioticId}|${guidelinesId}|${dosage}`;
}

/**
 * Merges special-consideration embed results from an existing row onto pattern embeds by special consideration id.
 *
 * @param {Array|undefined|null} patternEmbeds - Embeds from the new pattern line.
 * @param {Array|undefined|null} existingEmbeds - Embeds from the prior AST state.
 * @returns {Array|undefined|null}
 */
function mergeEmbeddedAstRowsFromExisting(patternEmbeds, existingEmbeds) {
    const withInclude = mergeEmbeddedAstRowsIncludeOnReport(existingEmbeds, patternEmbeds);
    if (!Array.isArray(withInclude) || !Array.isArray(existingEmbeds) || existingEmbeds.length === 0) {
        return withInclude;
    }
    const existingBySpecialId = new Map();
    existingEmbeds.forEach((embed) => {
        const specialId = resolveSpecialConsiderationId(embed);
        if (isValidSpecialConsiderationId(specialId)) {
            existingBySpecialId.set(specialId, embed);
        }
    });
    return withInclude.map((embed) => {
        const specialId = resolveSpecialConsiderationId(embed);
        const existing = existingBySpecialId.get(specialId);
        if (!existing) {
            return embed;
        }
        const merged = { ...embed };
        if (existing.TestResult != null && existing.TestResult !== '' && existing.TestResult !== 0) {
            merged.TestResult = existing.TestResult;
        }
        if (existing.ZoneDiameter != null && existing.ZoneDiameter !== '') {
            merged.ZoneDiameter = existing.ZoneDiameter;
        }
        if (existing.Mic != null && existing.Mic !== '') {
            merged.Mic = existing.Mic;
            if (existing.Operator != null && existing.Operator !== '') {
                merged.Operator = existing.Operator;
            }
        } else {
            merged.Operator = '';
        }
        if (existing.AppliedBreakpointId != null && existing.AppliedBreakpointId !== 0) {
            merged.AppliedBreakpointId = existing.AppliedBreakpointId;
        }
        return merged;
    });
}

/**
 * Copies entered result fields from an existing row onto a pattern line; pattern structure wins.
 *
 * @param {Object} patternRow - Line from the newly selected test pattern.
 * @param {Object} existingRow - Prior manual AST row with the same id-based line key.
 * @returns {Object} Merged row.
 */
function mergeExistingResultOntoPatternRow(patternRow, existingRow) {
    let merged = mergeAstLineFromServerResponse(existingRow, patternRow);
    if (existingRow.TestResult != null && existingRow.TestResult !== '' && existingRow.TestResult !== 0) {
        merged = { ...merged, TestResult: existingRow.TestResult };
    }
    if (existingRow.AppliedBreakpointId != null && existingRow.AppliedBreakpointId !== 0) {
        merged = { ...merged, AppliedBreakpointId: existingRow.AppliedBreakpointId };
    }
    merged.EmbeddedASTRows = mergeEmbeddedAstRowsFromExisting(
        merged.EmbeddedASTRows ?? patternRow.EmbeddedASTRows,
        existingRow.EmbeddedASTRows
    );
    return merged;
}

/**
 * For each pattern row, preserve entered results from matching existing manual rows (ids only).
 *
 * @param {Array} patternRows - Disk or MIC rows from the test pattern query response.
 * @param {Array} existingRows - Prior disk or MIC rows from craft state.
 * @returns {Array} Pattern rows with overlapping results merged in.
 */
function mergeExistingAstResultsOntoPatternRows(patternRows, existingRows) {
    if (!Array.isArray(patternRows)) {
        return patternRows;
    }
    const existingByKey = new Map();
    (existingRows || []).forEach((row) => {
        if (row?.ExpertRuleLine) {
            return;
        }
        const key = buildAstRowLineKey(row);
        if (key && !existingByKey.has(key)) {
            existingByKey.set(key, row);
        }
    });
    return patternRows.map((patternRow) => {
        const key = buildAstRowLineKey(patternRow);
        const existing = key ? existingByKey.get(key) : null;
        return existing ? mergeExistingResultOntoPatternRow(patternRow, existing) : patternRow;
    });
}

/**
 * Builds the flat `ASTResults` list for the craft save payload from Disk/MIC row trees.
 * Omits manual rows suppressed by an applied expert rule. Persists expert applications via {@link appendAstResultsFromExpertRuleGroups} only.
 *
 * @param {Array} localDiskResults
 * @param {Array} localMicResults
 * @param {Object} effectiveAst - Full ast object for ExpertRuleGroups and key collection overrides.
 * @returns {Array<Object>} ASTResults entries matching server expectations.
 */
/** Coerces AST save payload id fields to strings expected by legacy server parsing. */
function stringifyAstSaveField(value) {
    if (value === undefined || value === null || value === '') {
        return '';
    }
    return String(value);
}

function buildAstResultsFromAstData(localDiskResults, localMicResults, effectiveAst) {
    const appliedKeys = collectAppliedExpertAntibioticKeys(effectiveAst, localDiskResults, localMicResults);
    const results = [];

    for (let rowIndex = 0; rowIndex < localDiskResults.length; rowIndex++) {
        const row = localDiskResults[rowIndex];
        if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
            continue;
        }

        const suppressed =
            !row.ExpertRuleLine && isManualRowSuppressedByExpertRule(localDiskResults, 'disk', rowIndex, appliedKeys);

        if (!row.ExpertRuleLine && !suppressed) {
            let specialRows = null;
            if (row.EmbeddedASTRows !== undefined && row.EmbeddedASTRows !== null) {
                const rowsToInclude = [];
                for (const item of row.EmbeddedASTRows) {
                    const specialId = resolveSpecialConsiderationId(item);
                    if (isValidSpecialConsiderationId(specialId) && !item.ExpertRuleLine) {
                        rowsToInclude.push({
                            SpecialTypeId: specialId,
                            Susceptibility: stringifyAstSaveField(item.TestResult ?? item.testResult),
                            IncludeInReport: resolveRowIncludeOnReport(item),
                            BreakpointId: item.AppliedBreakpointId ?? item.appliedBreakpointId ?? item.BreakpointId ?? item.breakpointId ?? 0,
                            ExpertRuleId: 0,
                            SusceptibilityOverride: mapSusceptibilityOverrideForSave(item),
                        });
                    }
                }
                specialRows = rowsToInclude;
            }
            results.push({
                TestType: 'Disk',
                TestMethod: '681',
                EntryType: 'manual',
                Antibiotic: stringifyAstSaveField(row.Antibiotic),
                Dosage: row.Dosage === '' ? '0' : row.Dosage,
                Guidelines: stringifyAstSaveField(row.Guidelines),
                Measurement: row.ZoneDiameter === '' || row.ZoneDiameter === null ? -1 : row.ZoneDiameter,
                Susceptibility: stringifyAstSaveField(row.TestResult),
                Category: stringifyAstSaveField(row.DrugCategory),
                IncludeInReport: row.IncludeOnReport,
                AppliedBreakpointId: row.AppliedBreakpointId ?? 0,
                ExpertRuleLine: row.ExpertRuleLine,
                ExpertRuleId: 0,
                SpecialRows: specialRows,
                SusceptibilityOverride: mapSusceptibilityOverrideForSave(row),
            });
        }
    }

    for (let rowIndex = 0; rowIndex < localMicResults.length; rowIndex++) {
        const row = localMicResults[rowIndex];
        if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
            continue;
        }

        const suppressed =
            !row.ExpertRuleLine && isManualRowSuppressedByExpertRule(localMicResults, 'mic', rowIndex, appliedKeys);

        if (!row.ExpertRuleLine && !suppressed) {
            let specialRows = null;
            if (row.EmbeddedASTRows !== undefined && row.EmbeddedASTRows !== null) {
                const rowsToInclude = [];
                for (const item of row.EmbeddedASTRows) {
                    const specialId = resolveSpecialConsiderationId(item);
                    if (isValidSpecialConsiderationId(specialId) && !item.ExpertRuleLine) {
                        rowsToInclude.push({
                            SpecialTypeId: specialId,
                            Susceptibility: item.TestResult ?? item.testResult,
                            IncludeInReport: resolveRowIncludeOnReport(item),
                            BreakpointId: item.AppliedBreakpointId ?? item.appliedBreakpointId ?? item.BreakpointId ?? item.breakpointId ?? 0,
                            SusceptibilityOverride: mapSusceptibilityOverrideForSave(item),
                        });
                    }
                }
                specialRows = rowsToInclude;
            }
            results.push({
                TestType: 'Strip',
                TestMethod: '680',
                EntryType: 'manual',
                Antibiotic: stringifyAstSaveField(row.Antibiotic),
                Dosage: '0',
                Guidelines: stringifyAstSaveField(row.Guidelines),
                Measurement: measurementMicForSave(row),
                Susceptibility: stringifyAstSaveField(row.TestResult),
                Category: stringifyAstSaveField(row.DrugCategory),
                IncludeInReport: row.IncludeOnReport,
                AppliedBreakpointId: row.AppliedBreakpointId ?? 0,
                ExpertRuleLine: false,
                ExpertRuleId: 0,
                SpecialRows: specialRows,
                SusceptibilityOverride: mapSusceptibilityOverrideForSave(row),
            });
        }
    }

    appendAstResultsFromExpertRuleGroups(results, effectiveAst);
    return results;
}

/**
 * Deep-clones a disk/MIC row for {@link buildExpertRuleEvalContextFromAstData} (sidecar JSON, not AST table).
 * @param {Object} row
 * @returns {Object}
 */
function cloneRowForExpertRuleEvalContext(row) {
    return JSON.parse(JSON.stringify(normalizeOperatorOnlyMicRow(row)));
}

/**
 * Collects manual rows suppressed from {@link buildAstResultsFromAstData} by applied expert rules; persisted server-side
 * in `cultureastexpertruleevalcontext` (culture-scoped JSON) so expert conditions can be re-evaluated on reload.
 *
 * @param {Array} localDiskResults
 * @param {Array} localMicResults
 * @param {Object} effectiveAst
 * @returns {{ DiskResults: Array, MicResults: Array }|null} Null when nothing to persist.
 */
function buildExpertRuleEvalContextFromAstData(localDiskResults, localMicResults, effectiveAst) {
    const appliedKeys = collectAppliedExpertAntibioticKeys(effectiveAst, localDiskResults, localMicResults);
    const diskRows = [];
    const micRows = [];
    for (let rowIndex = 0; rowIndex < localDiskResults.length; rowIndex++) {
        const row = localDiskResults[rowIndex];
        if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
            continue;
        }
        const suppressed =
            !row.ExpertRuleLine && isManualRowSuppressedByExpertRule(localDiskResults, 'disk', rowIndex, appliedKeys);
        if (suppressed) {
            diskRows.push(cloneRowForExpertRuleEvalContext(row));
        }
    }
    for (let rowIndex = 0; rowIndex < localMicResults.length; rowIndex++) {
        const row = localMicResults[rowIndex];
        if (row.Antibiotic === undefined || row.Antibiotic === '' || row.Antibiotic === null) {
            continue;
        }
        const suppressed =
            !row.ExpertRuleLine && isManualRowSuppressedByExpertRule(localMicResults, 'mic', rowIndex, appliedKeys);
        if (suppressed) {
            micRows.push(cloneRowForExpertRuleEvalContext(row));
        }
    }
    if (diskRows.length === 0 && micRows.length === 0) {
        return null;
    }
    return { DiskResults: diskRows, MicResults: micRows };
}

    /**
     * AST (Antimicrobial Susceptibility Testing) form. Composes sub-components for header,
     * disk/mic results, expert rules, isolate tests, and comments.
     *
     * @param {Object} props
     * @param {Object} props.config - Page config (PageTitle, Text)
     * @param {Array} props.data - Crafted data
     * @param {Function} props.changeHandler - (key, changes) => void
     * @param {string} props.language - Current language
     * @param {Object} props.startConfig - Form start config (selectedRecord, setFormStartConfig, etc.)
     */
    const AST = (props) => {

    const modalPropsStyles = { main: { maxWidth: 450 } };
    const dialogContentProps = {
        type: DialogType.normal,
        title: TranslateTag("@GenRem@", props.language),
        subText: TranslateTag("@GenRemA@", props.language)
    };
    const ASTRowRemovalConfirmation = props.preferences !== undefined && props.preferences.ASTRowRemovalConfirmation === "Yes";
    //const calloutProps = { gapSpace: 10 };
    const categoryColours = [ 'black', 'red', 'blue', 'green' ];

    const [cultureId, setCultureId] = useState(0);
    const [organismId, setOrganismId] = useState(0);
    const [specimenTypeId, setSpecimenTypeId] = useState(0);
    const [testPattern, setTestPattern] = useState(null);
    const [testPatternFullList, setTestPatternFullList] = useState(null);
    const [deleteBlankRows, setDeleteBlankRows] = useState(null);
    const [ASTCommentOne, setASTCommentOne] = useState(null);
    const [ASTCommentTwo, setASTCommentTwo] = useState(null);
    const [ASTAdditionalNotes, setASTAdditionalNotes] = useState(null);
    const [CompletedDate, setCompletedDate] = useState(null);
    const [CompletedTime, setCompletedTime] = useState(null);
    const [showDialog, _setShowDialog] = useState(false);
    const [rowToDelete, setRowToDelete] = useState({ special: false, type: "", index: 0 });
    const [alertMessages, setAlertMessages] = useState([]);
    const [astData, setASTData] = useState({ DiskResults: [], MicResults: [] });
    /** Per parent row index: manual row suppressed because an expert rule with the same antibiotic is applied. */
    const diskRowSuppressedByExpert = useMemo(() => {
        const rows = astData.DiskResults || [];
        if (!organismId) {
            return rows.map(() => false);
        }
        const keys = collectAppliedExpertAntibioticKeys(astData);
        return rows.map((_, i) => isManualRowSuppressedByExpertRule(rows, 'disk', i, keys));
    }, [astData, organismId]);
    const micRowSuppressedByExpert = useMemo(() => {
        const rows = astData.MicResults || [];
        if (!organismId) {
            return rows.map(() => false);
        }
        const keys = collectAppliedExpertAntibioticKeys(astData);
        return rows.map((_, i) => isManualRowSuppressedByExpertRule(rows, 'mic', i, keys));
    }, [astData, organismId]);
    const printOnlyExpertKeys = useMemo(() => collectAppliedPrintOnlyExpertKeys(astData), [astData]);
    const diskRowIncludeOnReportLockedByExpert = useMemo(() => {
        const rows = astData.DiskResults || [];
        return rows.map((_, i) =>
            isRowIncludeOnReportLockedByPrintOnlyExpert(rows, 'disk', i, printOnlyExpertKeys)
        );
    }, [astData, printOnlyExpertKeys]);
    const micRowIncludeOnReportLockedByExpert = useMemo(() => {
        const rows = astData.MicResults || [];
        return rows.map((_, i) =>
            isRowIncludeOnReportLockedByPrintOnlyExpert(rows, 'mic', i, printOnlyExpertKeys)
        );
    }, [astData, printOnlyExpertKeys]);
    /**
     * Top alert strip: culture alerts only (position 998). Guidance expert rules no longer appear here —
     * triggered guidance rules show inline next to their AST line, and intrinsic guidance rules (no triggering
     * line) show in the hover next to the organism name at the top of the page.
     */
    const topAlertMessages = useMemo(() => {
        return alertMessages.filter((m) => m.positionId === 998);
    }, [alertMessages]);
    /** Deterministic colour per expert rule id (groups and guidance rules share the same palette). */
    const expertRuleColourMap = useMemo(() => {
        const groups = astData.ExpertRuleGroups ?? astData.expertRuleGroups ?? [];
        const comments = astData.ExpertRuleCommentAlerts ?? astData.expertRuleCommentAlerts ?? [];
        const ids = [
            ...groups.map((g) => g.RuleId ?? g.ruleId),
            ...comments.map((c) => c.RuleId ?? c.ruleId)
        ];
        return buildExpertRuleColourMap(ids);
    }, [astData]);
    /** Map of AST line identity -> expert rules it triggered, for inline trigger icons and colour-coded hover. */
    const expertLineTriggerMap = useMemo(
        () => buildExpertLineTriggerMap(astData, expertRuleColourMap),
        [astData, expertRuleColourMap]
    );
    /**
     * Intrinsic guidance expert rules: guidance (no-action) rules with no triggering AST line. Shown only in the
     * colour-coded hover next to the organism name at the top of the page. Shaped for {@link ExpertTriggerIcon}.
     */
    const intrinsicGuidanceRules = useMemo(() => {
        const comments = astData.ExpertRuleCommentAlerts ?? astData.expertRuleCommentAlerts ?? [];
        return comments
            .filter((c) => expertRuleTriggers(c).length === 0)
            .map((c) => {
                const ruleId = c.RuleId ?? c.ruleId;
                return {
                    ruleId,
                    ruleName: c.RuleName ?? c.ruleName ?? '',
                    ruleText: c.RuleText ?? c.ruleText ?? '',
                    colour: getExpertRuleColour(expertRuleColourMap, ruleId),
                    hasActions: false
                };
            });
    }, [astData, expertRuleColourMap]);
    /** Organism display name (from the AST payload) for the top-of-page heading. */
    const organismName = astData.OrganismName ?? astData.organismName ?? '';
    const [highlightedParentIds, setHighlightedParentIds] = useState(null); // Set of "type-index" when hovering group icon
    /** Undo metadata for expert rule apply/unapply; not server truth. Key = `${type}-${parentIndex}-${specialIndex}`. */
    const appliedRuleUndoRef = useRef(new Map());
    /** Undo for print-only expert IncludeOnReport overrides. Keys from {@link printOnReportExpertUndoKey}. */
    const printOnReportExpertUndoRef = useRef(new Map());
    /** When set, test pattern selection overrides Disk/Mic arrays until next server populate. */
    const patternRowsRef = useRef({ disk: null, mic: null });
    /** Debounce timer for POST AST/getexpertrulesforpage. */
    const expertRulesDebounceRef = useRef(null);
    /** Monotonic sequence so stale expert-rule responses are ignored. */
    const expertRulesSeqRef = useRef(0);
    /** Per-row generation so stale getsusceptibility responses are ignored. Key from {@link getSusceptibilityRowKey}. */
    const susceptibilitySeqRef = useRef({});
    const [formStartConfig, setFormStartConfig] = useState({});
    const [overridePanel, setOverridePanel] = useState(null);
    const overrideCannedOptions = useMemo(() => {
        const list = props.lists?.find(
            (l) => (l.Name || '').toLowerCase() === 'astsusceptibilityoverridecannedcommentslist',
        );
        return (list?.Options ?? []).map((item) => ({
            key: String(item.Key),
            text: item.Text,
        }));
    }, [props.lists]);


    const showDialogRef = useRef(showDialog);

    // Latest committed astData, mirrored every render so event handlers never read a stale closure.
    // Rapid successive edits (e.g. select antibiotic -> blur -> select guideline) would otherwise build
    // new state from a pre-render snapshot and drop the earlier field, intermittently clearing the row.
    const astDataRef = useRef(astData);
    astDataRef.current = astData;

    // Globals initialised on every render.
    var changedTestPattern = null;
    var changedTestPatternFullList = null;
    var changedDeleteBlankRows = null;
    var changedASTCommentOne = null;
    var changedASTCommentTwo = null;
    var changedASTAdditionalNotes = null;
    var changedCompletedDate = null;
    var changedCompletedTime = null;

    const craftAstValue = useMemo(() => props.data?.[0]?.Value, [props.data?.[0]?.Value]);

    useEffect(() => {
        if (craftAstValue === undefined || craftAstValue === null) {
            return;
        }
        initialSetUp();
        populateWithRetrievedData();
    }, [craftAstValue, props.language]);

    useEffect(() => {
        if (cultureId > 0) {
            resetAlerts(cultureId);
        }
    }, [cultureId]);

    useEffect(() => {
        if (expertRulesDebounceRef.current) {
            clearTimeout(expertRulesDebounceRef.current);
        }
    }, []);

    const GetOptionsFromList = (control, listname) => {
        const list = props.lists?.find(
            (l) => (l.Name || '').toLowerCase() === (listname || '').toLowerCase(),
        );
        control.Options = (list?.Options ?? []).map(item => ({
            key: item.Key,
            text: item.Text,
        }));
    }

    /**
     * Initialises AST form configs, labels, and state. Sets default date/time to current date and time.
     * populateWithRetrievedData may overwrite these with backend values when data is available.
     */
    const initialSetUp = () => {

        GetOptionsFromList(testPatternFullListConfig, 'testpatternnameslist');
        GetOptionsFromList(ASTCommentOneConfig, 'cannedcomments');
        GetOptionsFromList(ASTCommentTwoConfig, 'cannedcomments');

        deleteBlankRowsConfig.Label = TranslateTag("@AstRem@", props.language);
        ASTCommentOneConfig.Label = TranslateTag("@AstCom3@", props.language);
        ASTCommentTwoConfig.Label = TranslateTag("@AstCom4@", props.language);
        ASTAdditionalNotesConfig.Label = TranslateTag("@GenAdd@", props.language);
        completedDateConfig.Label = TranslateTag("@GenDatE@", props.language);
        completedTimeConfig.Label = TranslateTag("@GenTim@", props.language);

        const now = new Date();
        completedDateConfig.value = now.toISOString().split('T')[0];
        completedTimeConfig.value = now.toTimeString().slice(0, 5);

        setTestPatternFullList(testPatternFullListConfig);
        setDeleteBlankRows(deleteBlankRowsConfig);
        setASTCommentOne(ASTCommentOneConfig);
        setASTCommentTwo(ASTCommentTwoConfig);
        setASTAdditionalNotes(ASTAdditionalNotesConfig);
        setCompletedDate(completedDateConfig);
        setCompletedTime(completedTimeConfig);

        changedTestPatternFullList = testPatternFullListConfig;
        changedTestPattern = testPatternConfig;
        changedDeleteBlankRows = deleteBlankRowsConfig;
        changedASTCommentOne = ASTCommentOneConfig;
        changedASTCommentTwo = ASTCommentTwoConfig;
        changedASTAdditionalNotes = ASTAdditionalNotesConfig;
        changedCompletedDate = completedDateConfig;
        changedCompletedTime = completedTimeConfig;
    };

    /**
     * Populates AST form with data from the backend. Overwrites date/time with backend values when present.
     * When both CompletedDate and CompletedTime are empty from the backend, applies default current date/time.
     */
    const populateWithRetrievedData = () => {

        // We should always get data back at the culture level, so this is defensive.
        if (props.data === undefined || props.data === null) {
            return;
        }

       var ASTData = applyInheritIncludeOnReportToAstData(ensureDefaultDiskMicRows(JSON.parse(props.data[0].Value)));
        patternRowsRef.current = { disk: null, mic: null };
       setASTData(ASTData);

        // Save cultureId for in-form queries.
        setCultureId(ASTData.CultureId);

        // Save organismId for in-form queries.
        setOrganismId(ASTData.OrganismId);

        // Save specimenTypeId for in-form queries.
        setSpecimenTypeId(ASTData.SpecimenTypeId);

        // Populate test-pattern drop-down with valid test-pattern options (from the b'end).
        testPatternConfig.Options = [];
        if (ASTData.TestPatternOptions !== null && ASTData.TestPatternOptions !== undefined && ASTData.TestPatternOptions.length > 0) {
            for (var option of ASTData.TestPatternOptions) {
                let mappedOption = { key: option.Id.toString(), text: option.TestPatternName };
                testPatternConfig.Options.push(mappedOption);
           }
        }
        testPatternConfig.Label = TranslateTag("@AstTesB@", props.language);
        testPatternConfig.Placeholder = TranslateTag("@AstTesC@", props.language);
        testPatternFullListConfig.Label = TranslateTag("@AstTesE@", props.language);
        testPatternFullListConfig.Placeholder = TranslateTag("@AstTesF@", props.language);

        changedTestPattern.value = ASTData.SelectedTestPatternId !== null && ASTData.SelectedTestPatternId !== undefined ? ASTData.SelectedTestPatternId.toString() : "";

        setTestPattern(changedTestPattern);

        changedASTCommentOne.value = ASTData.ASTCommentOne;
        changedASTCommentTwo.value = ASTData.ASTCommentTwo;
        changedASTAdditionalNotes.value = ASTData.ASTAdditionalNotes;
        setASTCommentOne(changedASTCommentOne);
        setASTCommentTwo(changedASTCommentTwo);
        setASTAdditionalNotes(changedASTAdditionalNotes);

        const needsDefaultDateTime = (!ASTData.CompletedDate || ASTData.CompletedDate === '') &&
            (!ASTData.CompletedTime || ASTData.CompletedTime === '');
        if (needsDefaultDateTime) {
            const now = new Date();
            const today = now.toISOString().split('T')[0];
            const time = now.toTimeString().slice(0, 5);
            changedCompletedDate.value = today;
            changedCompletedTime.value = time;
        } else {
            changedCompletedDate.value = ASTData.CompletedDate ?? '';
            changedCompletedTime.value = ASTData.CompletedTime ?? '';
        }
        setCompletedDate(changedCompletedDate);
        setCompletedTime(changedCompletedTime);

        queueMicrotask(() => scheduleExpertRulesForPageRefresh(ASTData));
    }

    // const changeWrapper =  (id, value, changeFunction) => {

    //     let presentationModel = {...StoredPresentationModel};
    //     changeFunction(presentationModel, id, value);
    //     setStoredPresentationModel = presentationModel;
    // }

    // enzymeChangeHandler = (id, value) => {
    //     changeWrapper(id, value, (id, value) => { presentationModel.Enzymes[id] = value })
    // }

    // commentChangeHandler = (id, value) => {
    //     changeWrapper(id, value, (id, value) => { presentationModel.Comments[id] = value })
    // }

    /**
     * Handles test pattern selection change. Updates state, fetches antibiotics from the pattern,
     * merges overlapping entered results onto new pattern lines by id-based line key, and updates the save payload.
     * @param {string} value - Selected test pattern ID
     * @param {boolean} isMainPattern - If true, updates main test pattern; if false, updates full list selection
     */
    const testPatternChange = (value, isMainPattern) => {
        if (!value || !cultureId) {
            return;
        }
        if (isMainPattern) {
            const newTestPattern = { ...testPatternConfig, value };
            changedTestPattern = newTestPattern;
            setTestPattern(newTestPattern);
        } else {
            const newTestPatternFullList = { ...testPatternFullListConfig, value };
            changedTestPatternFullList = newTestPatternFullList;
            setTestPatternFullList(newTestPatternFullList);
        }
        runTestPatternQuery(
            cultureId,
            value,
            (data) => {
                const parsed = JSON.parse(data.Crafted[0].Contents);
                const value = JSON.parse(parsed[0].Value);
                setASTData((prev) => {
                    const merged = { ...prev, ...value };
                    if (merged.ApplicableIsolateTests === undefined && merged.applicableIsolateTests === undefined) {
                        merged.ApplicableIsolateTests =
                            prev?.ApplicableIsolateTests ?? prev?.applicableIsolateTests;
                    }
                    const hasGroups =
                        (Array.isArray(merged.ExpertRuleGroups) && merged.ExpertRuleGroups.length > 0) ||
                        (Array.isArray(merged.expertRuleGroups) && merged.expertRuleGroups.length > 0);
                    if (!hasGroups && prev?.ExpertRuleGroups?.length) {
                        merged.ExpertRuleGroups = prev.ExpertRuleGroups;
                    }
                    const hasCommentAlerts =
                        (Array.isArray(merged.ExpertRuleCommentAlerts) && merged.ExpertRuleCommentAlerts.length > 0) ||
                        (Array.isArray(merged.expertRuleCommentAlerts) && merged.expertRuleCommentAlerts.length > 0);
                    if (!hasCommentAlerts && prev?.ExpertRuleCommentAlerts?.length) {
                        merged.ExpertRuleCommentAlerts = prev.ExpertRuleCommentAlerts;
                    }
                    merged.DiskResults = mergeExistingAstResultsOntoPatternRows(
                        merged.DiskResults || [],
                        prev?.DiskResults || []
                    );
                    merged.MicResults = mergeExistingAstResultsOntoPatternRows(
                        merged.MicResults || [],
                        prev?.MicResults || []
                    );
                    const result = applyInheritIncludeOnReportToAstData(ensureDefaultDiskMicRows(merged), prev);
                    queueMicrotask(() => updateSaveableData(result));
                    return result;
                });
            },
            errorWhenRetrievingData
        );
    };

    const changeHandler = (id, value) => {
        if (id === 'testpatternfulllist') {
            testPatternChange(value, false);
        } else if (id === 'deleteblankrows') {
            let newDeleteBlankRows = {...deleteBlankRows};
            newDeleteBlankRows.value = value;
            setDeleteBlankRows(newDeleteBlankRows);
            changedDeleteBlankRows = newDeleteBlankRows;
            updateSaveableData();
        } else if (id === 'astcommentone') {
            let newASTCommentOne = {...ASTCommentOne};
            newASTCommentOne.value = value;
            setASTCommentOne(newASTCommentOne)
            changedASTCommentOne = newASTCommentOne;
        } else if (id === 'astcommenttwo') {
            let newASTCommentTwo = {...ASTCommentTwo};
            newASTCommentTwo.value = value;
            setASTCommentTwo(newASTCommentTwo)
            changedASTCommentTwo = newASTCommentTwo;
        } else if (id === 'astadditionalnotes') {
            let newASTAdditionalNotes = {...ASTAdditionalNotes};
            newASTAdditionalNotes.value = value;
            setASTAdditionalNotes(newASTAdditionalNotes)
            changedASTAdditionalNotes = newASTAdditionalNotes;
        } else if (id === 'completeddate') {
            let newCompletedDate = {...CompletedDate};
            newCompletedDate.value = value;
            setCompletedDate(newCompletedDate)
            changedCompletedDate = newCompletedDate;
        } else if (id === 'completedtime') {
            let newCompletedTime = {...CompletedTime};
            newCompletedTime.value = value;
            setCompletedTime(newCompletedTime)
            changedCompletedTime = newCompletedTime;
        } else if (id === 'testpattern') {
            testPatternChange(value, true);
        } else {
            updateAntibioticsForm(id, value);
        }
        if (id !== 'testpattern' && id !== 'testpatternfulllist') {
            updateSaveableData();
        }
    }

    /**
     * Pushes current AST presentation state to the parent craft handler. Pass `astDataOverride` when React state
     * has not yet committed (e.g. immediately after setASTData).
     * @param {Object} [astDataOverride] - Full ast payload to serialize; defaults to current `astData`.
     */
    const updateSaveableData = (astDataOverride) => {
        const effectiveAst = astDataOverride ?? astData;
        let localTestPatternFullList = changedTestPatternFullList === null ? testPatternFullList : changedTestPatternFullList;
        let localDeleteBlankRows = changedDeleteBlankRows == null ? deleteBlankRows : changedDeleteBlankRows;
        let localASTCommentOne = changedASTCommentOne === null ? ASTCommentOne : changedASTCommentOne;
        let localASTCommentTwo = changedASTCommentTwo === null ? ASTCommentTwo : changedASTCommentTwo;
        let localASTAdditionalNotes = changedASTAdditionalNotes === null ? ASTAdditionalNotes : changedASTAdditionalNotes;
        let localCompletedDate = changedCompletedDate === null ? CompletedDate : changedCompletedDate;
        let localCompletedTime = changedCompletedTime === null ?  CompletedTime : changedCompletedTime;
        let localTestPattern = changedTestPattern === null ? testPattern : changedTestPattern;
        const pr = patternRowsRef.current;
        let localDiskResults;
        let localMicResults;
        if (pr.disk != null && pr.mic != null) {
            localDiskResults = [...pr.disk];
            localMicResults = [...pr.mic];
        } else {
            localDiskResults = [...(effectiveAst.DiskResults || [])];
            localMicResults = [...(effectiveAst.MicResults || [])];
        }

        let dataToSave = {
            testPatternFullList: localTestPatternFullList.value,
            deleteBlankRows: localDeleteBlankRows.value,
            ASTCommentOne: localASTCommentOne.value,
            ASTCommentTwo: localASTCommentTwo.value,
            ASTAdditionalNotes: localASTAdditionalNotes.value,
            CompletedDate: localCompletedDate.value,
            CompletedTime: localCompletedTime.value,
            testPattern: localTestPattern.value,
            ASTResults: buildAstResultsFromAstData(localDiskResults, localMicResults, effectiveAst)
        };

        const evalCtx = buildExpertRuleEvalContextFromAstData(localDiskResults, localMicResults, effectiveAst);
        if (evalCtx) {
            dataToSave.ExpertRuleEvalContext = evalCtx;
        }

        const erg = effectiveAst.ExpertRuleGroups ?? effectiveAst.expertRuleGroups;
        if (Array.isArray(erg) && erg.length > 0) {
            dataToSave.ExpertRuleGroups = erg.map((g) => ({
                RuleId: g.RuleId ?? g.ruleId,
                RuleName: g.RuleName ?? g.ruleName,
                RuleText: g.RuleText ?? g.ruleText,
                ApplyRule: !!(g.ApplyRule === true || g.ApplyRule === 'Yes'),
                Actions: (g.Actions || g.actions || []).map((a) => ({ ...a }))
            }));
        }

        const changes = { Key: "ast", value: dataToSave };
        props.changeHandler('ast', changes);
    }

    const errorWhenRetrievingData = (errorMessage) => {
        console.log('Failed to retrieve data from backend with error:', errorMessage);
    };

    /**
     * Debounced refresh of expert rule groups from the server using current disk/MIC trees (page-scoped evaluation).
     * @param {Object} astSnapshot - Full ast state including CultureId, DiskResults, MicResults.
     */
    const scheduleExpertRulesForPageRefresh = (astSnapshot) => {
        const cid = astSnapshot.CultureId ?? astSnapshot.cultureId;
        if (!cid || cid <= 0) {
            return;
        }
        if (expertRulesDebounceRef.current) {
            clearTimeout(expertRulesDebounceRef.current);
        }
        expertRulesDebounceRef.current = setTimeout(() => {
            expertRulesDebounceRef.current = null;
            const seq = ++expertRulesSeqRef.current;
            const disk = stripExpertRuleLinesFromEmbedded(astSnapshot.DiskResults || []);
            const mic = stripExpertRuleLinesFromEmbedded(astSnapshot.MicResults || []);
            Post(
                'AST/getexpertrulesforpage',
                { CultureId: cid, DiskResults: disk, MicResults: mic },
                (raw) => {
                    if (seq !== expertRulesSeqRef.current) {
                        return;
                    }
                    const parsed = typeof raw === 'string' ? JSON.parse(raw) : raw;
                    const groups = parsed.ExpertRuleGroups ?? parsed.expertRuleGroups ?? [];
                    const flat = parsed.ExpertRuleResults ?? parsed.expertRuleResults ?? [];
                    const commentAlerts = parsed.ExpertRuleCommentAlerts ?? parsed.expertRuleCommentAlerts ?? [];
                    setASTData((prev) => {
                        const mergedGroups = mergeExpertRuleGroupsPreservePrintOnlyApply(
                            prev.ExpertRuleGroups ?? prev.expertRuleGroups,
                            groups
                        );
                        const next = {
                            ...prev,
                            ExpertRuleGroups: mergedGroups,
                            ExpertRuleResults: flat,
                            ExpertRuleCommentAlerts: Array.isArray(commentAlerts) ? commentAlerts : []
                        };
                        for (const g of mergedGroups) {
                            const applied = g.ApplyRule === true || g.ApplyRule === 'Yes';
                            if (!applied) {
                                continue;
                            }
                            const act = g.Actions || g.actions || [];
                            if (!act.some((a) => isPrintOnReportOnlyExpertAction(a))) {
                                continue;
                            }
                            const ruleId = g.RuleId ?? g.ruleId;
                            applyPrintOnlyExpertGroupToManualRows(next, g, ruleId, true, printOnReportExpertUndoRef);
                        }
                        queueMicrotask(() => updateSaveableData(next));
                        return next;
                    });
                },
                errorWhenRetrievingData
            );
        }, 280);
    };

    const clearAllRows = () => {

        changedDiskManualAntibioticsForm = [];
        changedStripManualAntibioticsForm = [];
    }

    const recordSusceptibilityChangeAudit =
        astData.RecordSusceptibilityChangeAudit ?? astData.recordSusceptibilityChangeAudit ?? 'No';

    const resolveSusceptibilityLabel = (susceptibilityId) => {
        const id = Number(susceptibilityId) || 0;
        if (id === 0) {
            return '';
        }
        const list = props.lists?.find((l) => l.Name === 'testresult');
        const found = list?.Options?.find((o) => Number(o.Key) === id);
        return found?.Text ?? '';
    };

    const getAstRowAt = (data, type, rowIndex, specialIndex) => {
        if (specialIndex === undefined || specialIndex === null) {
            return type === 'disk' ? data.DiskResults?.[rowIndex] : data.MicResults?.[rowIndex];
        }
        const parent = type === 'disk' ? data.DiskResults?.[rowIndex] : data.MicResults?.[rowIndex];
        return parent?.EmbeddedASTRows?.[specialIndex];
    };

    const setAstRowAt = (data, type, rowIndex, specialIndex, rowValue) => {
        if (specialIndex === undefined || specialIndex === null) {
            if (type === 'disk') {
                data.DiskResults = [...(data.DiskResults || [])];
                data.DiskResults[rowIndex] = rowValue;
            } else {
                data.MicResults = [...(data.MicResults || [])];
                data.MicResults[rowIndex] = rowValue;
            }
            return;
        }
        if (type === 'disk') {
            data.DiskResults = [...(data.DiskResults || [])];
            const parent = { ...data.DiskResults[rowIndex], EmbeddedASTRows: [...(data.DiskResults[rowIndex].EmbeddedASTRows || [])] };
            parent.EmbeddedASTRows[specialIndex] = rowValue;
            data.DiskResults[rowIndex] = parent;
        } else {
            data.MicResults = [...(data.MicResults || [])];
            const parent = { ...data.MicResults[rowIndex], EmbeddedASTRows: [...(data.MicResults[rowIndex].EmbeddedASTRows || [])] };
            parent.EmbeddedASTRows[specialIndex] = rowValue;
            data.MicResults[rowIndex] = parent;
        }
    };

    const openOverridePanel = (type, rowIndex, specialIndex, mode, pendingTestResult) => {
        const currentRow = getAstRowAt(astDataRef.current, type, rowIndex, specialIndex);
        const previousTestResult = mode === 'edit'
            ? (currentRow?.SusceptibilityOverride?.OverriddenFromSusceptibilityId ?? 0)
            : (currentRow?.TestResult ?? 0);
        setOverridePanel({
            type,
            rowIndex,
            specialIndex,
            mode,
            pendingTestResult: pendingTestResult ?? currentRow?.TestResult,
            previousTestResult,
            initialCannedCommentId: currentRow?.SusceptibilityOverride?.CannedCommentId ?? 0,
            initialFreeText: currentRow?.SusceptibilityOverride?.FreeTextComment ?? '',
        });
    };

    const applyManualOverrideToRow = (type, rowIndex, specialIndex, testResultId, auditFields) => {
        const newData = { ...astDataRef.current };
        const currentRow = getAstRowAt(newData, type, rowIndex, specialIndex) ?? {};
        const previousResult = overridePanel?.previousTestResult ?? currentRow.TestResult;
        const setByUsername =
            getLoggedInUsername() ||
            currentRow.SusceptibilityOverride?.SetByUsername ||
            '';
        const updatedRow = {
            ...currentRow,
            TestResult: testResultId ?? currentRow.TestResult,
            SusceptibilityOverride: {
                IsManuallySet: true,
                SetAt: new Date().toISOString(),
                SetByUsername: setByUsername,
                CannedCommentId: auditFields?.CannedCommentId ?? currentRow.SusceptibilityOverride?.CannedCommentId ?? 0,
                FreeTextComment: auditFields?.FreeTextComment ?? currentRow.SusceptibilityOverride?.FreeTextComment ?? '',
                OverriddenFromSusceptibilityId: Number(previousResult) || 0,
                ClearOverride: false,
            },
        };
        setAstRowAt(newData, type, rowIndex, specialIndex, updatedRow);
        patternRowsRef.current = { disk: null, mic: null };
        astDataRef.current = newData;
        setASTData(newData);
        updateSaveableData(newData);
        queueMicrotask(() => scheduleExpertRulesForPageRefresh(newData));
    };

    const handleOverridePanelSave = (auditFields) => {
        if (!overridePanel) {
            return;
        }
        const { type, rowIndex, specialIndex, mode, pendingTestResult } = overridePanel;
        if (mode === 'edit') {
            const newData = { ...astDataRef.current };
            const currentRow = getAstRowAt(newData, type, rowIndex, specialIndex) ?? {};
            setAstRowAt(newData, type, rowIndex, specialIndex, {
                ...currentRow,
                SusceptibilityOverride: {
                    ...(currentRow.SusceptibilityOverride ?? {}),
                    IsManuallySet: true,
                    CannedCommentId: auditFields.CannedCommentId ?? 0,
                    FreeTextComment: auditFields.FreeTextComment ?? '',
                    ClearOverride: false,
                },
            });
            patternRowsRef.current = { disk: null, mic: null };
            astDataRef.current = newData;
            setASTData(newData);
            updateSaveableData(newData);
        } else {
            applyManualOverrideToRow(type, rowIndex, specialIndex, pendingTestResult, auditFields);
        }
        setOverridePanel(null);
    };

    const revertManualSusceptibilityAtRow = (type, rowIndex, specialIndex) => {
        const newData = { ...astDataRef.current };
        const currentRow = getAstRowAt(newData, type, rowIndex, specialIndex) ?? {};
        const revertedSusceptibilityId =
            Number(currentRow.SusceptibilityOverride?.OverriddenFromSusceptibilityId) ||
            0;
        const clearedRow = {
            ...currentRow,
            TestResult: revertedSusceptibilityId,
            SusceptibilityOverride: { ClearOverride: true, IsManuallySet: false },
            ForceRecalculateSusceptibility: true,
        };
        setAstRowAt(newData, type, rowIndex, specialIndex, clearedRow);
        patternRowsRef.current = { disk: null, mic: null };
        astDataRef.current = newData;
        setASTData(newData);
        const rowForLookup = resolveAstRowForBreakpointLookup(type, rowIndex, specialIndex, newData, clearedRow);
        getBreakpointsAndExpertRules(rowForLookup, rowIndex, specialIndex, type, true);
        updateSaveableData(newData);
    };

    const handleManualOverrideIconClick = (type, rowIndex, specialIndex) => {
        openOverridePanel(type, rowIndex, specialIndex, 'edit');
    };

    const handleManualOverrideRevert = (type, rowIndex, specialIndex) => {
        revertManualSusceptibilityAtRow(type, rowIndex, specialIndex);
    };

    /**
     * Handles disk/MIC/expert row edits. MIC or zone-diameter changes always refresh susceptibility
     * from breakpoints; direct susceptibility (TestResult) and Include on report (IncludeOnReport)
     * edits skip that lookup.
     *
     * @param {'disk'|'mic'|'expert'} type
     * @param {number} row
     * @param {number|undefined} specialrow
     * @param {Object} value - Updated row fields from the line editor
     * @param {string|undefined} changedFieldKey - Field the user edited (e.g. Mic, TestResult, Guidelines)
     */
    const ASTChangeHandler = (type, row, specialrow, value, changedFieldKey) => {

        const newData = { ...astDataRef.current };

        if (type === 'expert') {
            const groups = [...(newData.ExpertRuleGroups || [])];
            const grp = groups[row];
            if (grp) {
                const actions = [...(grp.Actions || grp.actions || [])];
                actions[specialrow] = { ...actions[specialrow], ...value };
                groups[row] = { ...grp, Actions: actions };
                newData.ExpertRuleGroups = groups;
            }
            patternRowsRef.current = { disk: null, mic: null };
            astDataRef.current = newData;
            setASTData(newData);
            updateSaveableData(newData);
            return;
        }

        if (changedFieldKey === 'TestResult') {
            const prevRow = getAstRowAt(astDataRef.current, type, row, specialrow);
            if (recordSusceptibilityChangeAudit === 'Yes') {
                openOverridePanel(type, row, specialrow, 'new', value.TestResult);
                return;
            }
            value = {
                ...value,
                SusceptibilityOverride: {
                    IsManuallySet: true,
                    SetAt: new Date().toISOString(),
                    SetByUsername: getLoggedInUsername() || prevRow?.SusceptibilityOverride?.SetByUsername || '',
                    CannedCommentId: prevRow?.SusceptibilityOverride?.CannedCommentId ?? 0,
                    FreeTextComment: prevRow?.SusceptibilityOverride?.FreeTextComment ?? '',
                    OverriddenFromSusceptibilityId: Number(prevRow?.TestResult) || 0,
                    ClearOverride: false,
                },
            };
        }

        if (specialrow === undefined) {
            if (type === 'disk') {
                newData.DiskResults = [...newData.DiskResults];
                newData.DiskResults[row] = { ...newData.DiskResults[row], ...value };
            } else {
                newData.MicResults = [...newData.MicResults];
                const micUpdate =
                    changedFieldKey === 'Mic' ? { ...value, ...normalizeMicRowEdit(value.Mic) } : value;
                newData.MicResults[row] = { ...newData.MicResults[row], ...micUpdate };
            }
        } else {
            const parentArray = type === 'disk' ? newData.DiskResults : newData.MicResults;
            const parentRow = parentArray[row];
            if (parentRow) {
                const clonedEmbedded = [...(parentRow.EmbeddedASTRows || [])];
                const embedUpdate =
                    type === 'mic' && changedFieldKey === 'Mic'
                        ? { ...value, ...normalizeMicRowEdit(value.Mic) }
                        : value;
                clonedEmbedded[specialrow] = { ...clonedEmbedded[specialrow], ...embedUpdate };
                if (type === 'disk') {
                    newData.DiskResults = [...newData.DiskResults];
                    newData.DiskResults[row] = { ...parentRow, EmbeddedASTRows: clonedEmbedded };
                } else {
                    newData.MicResults = [...newData.MicResults];
                    newData.MicResults[row] = { ...parentRow, EmbeddedASTRows: clonedEmbedded };
                }
            }
        }

        patternRowsRef.current = { disk: null, mic: null };
        astDataRef.current = newData;
        setASTData(newData);
        let rowForLookup = resolveAstRowForBreakpointLookup(type, row, specialrow, newData, value);
        if (changedFieldKey === 'Guidelines' && type === 'mic' && specialrow === undefined) {
            const rerounded = reroundMicRowForGuideline(rowForLookup);
            if (rerounded !== rowForLookup) {
                rowForLookup = rerounded;
                newData.MicResults = [...newData.MicResults];
                newData.MicResults[row] = rerounded;
                astDataRef.current = newData;
                setASTData(newData);
            }
        }
        if (changedFieldKey === 'TestResult') {
            queueMicrotask(() => scheduleExpertRulesForPageRefresh(newData));
        } else if (changedFieldKey !== 'IncludeOnReport') {
            getBreakpointsAndExpertRules(rowForLookup, row, specialrow, type);
        }
        updateSaveableData(newData);
    };

    const handleApplyRuleChange = (type, parentIndex, specialIndex, applyRuleValue, expertRuleRow) => {
        const newData = { ...astData };
        const ruleKey = `${type}-${parentIndex}-${specialIndex}`;
        const appliedRulesCopy = new Map(appliedRuleUndoRef.current);
        
        // Get parent row
        const parentArray = type === 'disk' ? newData.DiskResults : newData.MicResults;
        const parentRow = parentArray[parentIndex];
        
        if (!parentRow) {
            return; // Parent row doesn't exist
        }

        if (applyRuleValue === 'Yes') {
            // Apply the rule
            const expertAntibiotic = expertRuleRow.Antibiotic;
            const parentAntibiotic = parentRow.Antibiotic;
            
            if (expertAntibiotic === parentAntibiotic) {
                // Same antibiotic: update parent TestResult
                const originalTestResult = parentRow.TestResult;
                parentRow.TestResult = expertRuleRow.TestResult;
                
                // Store original value for undo
                appliedRulesCopy.set(ruleKey, {
                    originalTestResult: originalTestResult,
                    addedRowIdentifier: null,
                    addedRowType: null
                });
            } else {
                // Different antibiotic: add as new row
                const newRow = {
                    ...expertRuleRow,
                    ExpertRuleLine: false, // Remove expert rule flag
                    ApplyRule: 'Yes', // Mark as applied
                    AppliedFromRule: ruleKey // Track where it came from - use this to find and remove
                };
                
                // Determine which array to add to based on type
                const targetArray = type === 'disk' ? newData.DiskResults : newData.MicResults;
                targetArray.push(newRow);
                
                // Store identifier for undo (use ruleKey to find the row)
                appliedRulesCopy.set(ruleKey, {
                    originalTestResult: null,
                    addedRowIdentifier: ruleKey,
                    addedRowType: type
                });
            }
            
            // Update expert rule row ApplyRule state
            if (parentRow.EmbeddedASTRows && parentRow.EmbeddedASTRows[specialIndex]) {
                parentRow.EmbeddedASTRows[specialIndex].ApplyRule = 'Yes';
            }
        } else {
            // Unapply the rule
            const appliedInfo = appliedRulesCopy.get(ruleKey);
            
            if (appliedInfo) {
                if (appliedInfo.addedRowIdentifier !== null) {
                    // Remove added row by finding it using AppliedFromRule
                    const targetArray = appliedInfo.addedRowType === 'disk' ? newData.DiskResults : newData.MicResults;
                    const rowIndex = targetArray.findIndex(row => row.AppliedFromRule === ruleKey);
                    if (rowIndex !== -1) {
                        targetArray.splice(rowIndex, 1);
                    }
                } else if (appliedInfo.originalTestResult !== null && appliedInfo.originalTestResult !== undefined) {
                    // Restore original TestResult
                    parentRow.TestResult = appliedInfo.originalTestResult;
                }
                
                // Remove from tracking
                appliedRulesCopy.delete(ruleKey);
            }
            
            // Update expert rule row ApplyRule state
            if (parentRow.EmbeddedASTRows && parentRow.EmbeddedASTRows[specialIndex]) {
                parentRow.EmbeddedASTRows[specialIndex].ApplyRule = 'No';
            }
        }
        
        appliedRuleUndoRef.current = appliedRulesCopy;
        patternRowsRef.current = { disk: null, mic: null };
        setASTData(newData);
        updateSaveableData(newData);
        const parentAfter = type === 'disk' ? newData.DiskResults[parentIndex] : newData.MicResults[parentIndex];
        if (parentAfter) {
            refreshParentRowExpertRules(type, parentIndex, parentAfter);
        }
    }

    const setShowDialog = data => {
        showDialogRef.current = data;
        _setShowDialog(data);
    };

    const deleteRow = () => {
        setShowDialog(false);
        removeAntibioticClickHandler(rowToDelete.type, rowToDelete.index, rowToDelete.specialIndex);
    }

    const dontDeleteRow = () => {
        setShowDialog(false);
    }

    const removeAntibioticConfirmation = (special, type, index, specialIndex) => {
        if (ASTRowRemovalConfirmation) {
            setRowToDelete({ special: special, type: type, index: index, specialIndex: specialIndex });
            setShowDialog(true);
        } 
        else 
        {
            removeAntibioticClickHandler(type, index, specialIndex);
        }
    }

    const addAntibioticClickHandler = (type) => {

        const newData = {...astData};
        let methodId = type == 'Disk' ? 681 : 680;

        let newEntry = 
        {
            Antibiotic: '',
            Dosage: '',
            DrugCategory: '',
            EmbeddedASTRows: [],
            ExpertRuleLine: false,
            ExpertRuleName: null,
            ExpertRuleText: null,
            Guidelines: '',
            IncludeOnReport: 'No',
            Mic: '',
            OrganismId: '',
            SpecialConsideration: null,
            SpecialConsiderationId: 973,
            TestMethod: methodId,
            TestResult: '',
            ZoneDiameter: ''
        }

        if (type === 'Disk') 
        {
            newData.DiskResults.push(newEntry);
        } 
        else if (type === 'Strip') 
        {
            newData.MicResults.push(newEntry);
        }

        patternRowsRef.current = { disk: null, mic: null };
        setASTData(newData);
        queueMicrotask(() => updateSaveableData(newData));
    }

    /**
     * Removes a manual Disk/MIC line or embedded special-consideration row, updates save payload,
     * prunes expert apply undo state, invalidates in-flight susceptibility requests, and re-evaluates expert rules for the page.
     *
     * @param {'disk'|'mic'} type
     * @param {number} index - Parent row index
     * @param {number|undefined} specialIndex - Embedded row index when deleting a special consideration
     */
    const removeAntibioticClickHandler = (type, index, specialIndex) => {
        const source = astDataRef.current ?? astData;
        const newData = { ...source };
        let deletedRow = null;

        if (type === 'disk') {
            if (specialIndex !== undefined) {
                deletedRow = source.DiskResults?.[index]?.EmbeddedASTRows?.[specialIndex];
                newData.DiskResults = [...(source.DiskResults || [])];
                const parent = {
                    ...newData.DiskResults[index],
                    EmbeddedASTRows: [...(newData.DiskResults[index]?.EmbeddedASTRows || [])]
                };
                parent.EmbeddedASTRows.splice(specialIndex, 1);
                newData.DiskResults[index] = parent;
            } else {
                deletedRow = source.DiskResults?.[index];
                newData.DiskResults = [...(source.DiskResults || [])];
                newData.DiskResults.splice(index, 1);
                removeRowsWithAppliedFromRulePrefix(newData, `${type}-${index}-`);
            }
        } else if (type === 'mic') {
            if (specialIndex !== undefined) {
                deletedRow = source.MicResults?.[index]?.EmbeddedASTRows?.[specialIndex];
                newData.MicResults = [...(source.MicResults || [])];
                const parent = {
                    ...newData.MicResults[index],
                    EmbeddedASTRows: [...(newData.MicResults[index]?.EmbeddedASTRows || [])]
                };
                parent.EmbeddedASTRows.splice(specialIndex, 1);
                newData.MicResults[index] = parent;
            } else {
                deletedRow = source.MicResults?.[index];
                newData.MicResults = [...(source.MicResults || [])];
                newData.MicResults.splice(index, 1);
                removeRowsWithAppliedFromRulePrefix(newData, `${type}-${index}-`);
            }
        } else {
            return;
        }

        pruneExpertRuleUndoStateAfterRowRemoval(
            type,
            index,
            specialIndex,
            deletedRow,
            appliedRuleUndoRef,
            printOnReportExpertUndoRef
        );

        invalidateSusceptibilityRequestsAfterRowRemoval(type, index, specialIndex, susceptibilitySeqRef);

        patternRowsRef.current = { disk: null, mic: null };
        astDataRef.current = newData;
        setASTData(newData);
        queueMicrotask(() => updateSaveableData(newData));
        queueMicrotask(() => scheduleExpertRulesForPageRefresh(newData));
    };

    /**
     * Applies AST/getsusceptibility response to disk/MIC state. Discards stale responses when the target
     * index is out of bounds or no row matches the request's id-based line key (e.g. after manual row delete).
     *
     * @param {*} data - Server susceptibility payload.
     * @param {Object} extraInfo - Callback context including generation, index, type, and requestMeta.
     */
    const susceptibilityDataRetrievedSuccessfully = (data, extraInfo) => {
        const rowKey = getSusceptibilityRowKey(extraInfo.type, extraInfo.index, extraInfo.specialIndex);
        if (extraInfo.generation != null && extraInfo.generation !== susceptibilitySeqRef.current[rowKey]) {
            return;
        }

        let parsedData = typeof data === 'string' ? JSON.parse(data) : data;
        if (parsedData.Mic != null && parsedData.Mic !== '' && parsedData.Operator != null && parsedData.Operator !== '') {
            const operator = parsedData.Operator;
            const micTemp = parsedData.Mic.toString();
            parsedData = { ...parsedData, Mic: operator + micTemp };
        }
        parsedData = normalizeBackendMeasurementSentinelRow(parsedData);
        const newAlertMessages = [];

        setASTData(prev => {
            const target = resolveSusceptibilityTargetLocation(prev, extraInfo.type, extraInfo);
            if (!target) {
                // #region agent log
                console.log('DEBUG', {
                    location: 'AST.js:susceptibilityDataRetrievedSuccessfully',
                    message: 'Discarded stale getsusceptibility response',
                    data: {
                        type: extraInfo.type,
                        index: extraInfo.index,
                        requestLineKey: extraInfo.requestMeta?.requestLineKey,
                    },
                    timestamp: Date.now(),
                    sessionId: 'ast-row-removal',
                    runId: 'susceptibility-guard',
                    hypothesisId: 'stale-response',
                });
                // #endregion
                return prev;
            }

            patternRowsRef.current = { disk: null, mic: null };
            const newData = { ...prev };
            const { parentIndex, specialIndex, prevRow } = target;
            let mergedRow = mergeAstLineFromServerResponse(prevRow, parsedData);
            if (isRowManuallySetSusceptibility(prevRow) && !extraInfo.forceRecalculate) {
                mergedRow = {
                    ...mergedRow,
                    TestResult: prevRow.TestResult,
                    AppliedBreakpointId: prevRow.AppliedBreakpointId,
                    SusceptibilityOverride: prevRow.SusceptibilityOverride,
                };
            } else {
                mergedRow = preserveTestResultIfServerUnmatched(
                    prevRow,
                    mergedRow,
                    extraInfo.hadMeasurementOnRequest === true
                );
                if (prevRow?.SusceptibilityOverride?.ClearOverride === true) {
                    mergedRow = {
                        ...mergedRow,
                        SusceptibilityOverride: { ClearOverride: true, IsManuallySet: false },
                        ForceRecalculateSusceptibility: false,
                    };
                    if (
                        extraInfo.forceRecalculate &&
                        !astRowHasResolvedTestResult(mergedRow.TestResult) &&
                        astRowHasResolvedTestResult(prevRow.TestResult)
                    ) {
                        mergedRow = { ...mergedRow, TestResult: prevRow.TestResult };
                    }
                }
            }
            if (specialIndex === undefined) {
                const previousEmbeds = prevRow?.EmbeddedASTRows;
                mergedRow = inheritIncludeOnReportOnNewEmbeds(mergedRow, previousEmbeds);
                mergedRow = preserveEmbedIncludeOnReportAfterSusceptibilityMerge(mergedRow, previousEmbeds);
                if (extraInfo.type === 'disk') {
                    newData.DiskResults = [...(newData.DiskResults || [])];
                    newData.DiskResults[parentIndex] = mergedRow;
                } else {
                    newData.MicResults = [...(newData.MicResults || [])];
                    newData.MicResults[parentIndex] = mergedRow;
                }
            } else if (extraInfo.type === 'disk') {
                newData.DiskResults = [...(newData.DiskResults || [])];
                const parent = {
                    ...newData.DiskResults[parentIndex],
                    EmbeddedASTRows: [...(newData.DiskResults[parentIndex].EmbeddedASTRows || [])],
                };
                parent.EmbeddedASTRows[specialIndex] = mergedRow;
                newData.DiskResults[parentIndex] = parent;
            } else {
                newData.MicResults = [...(newData.MicResults || [])];
                const parent = {
                    ...newData.MicResults[parentIndex],
                    EmbeddedASTRows: [...(newData.MicResults[parentIndex].EmbeddedASTRows || [])],
                };
                parent.EmbeddedASTRows[specialIndex] = mergedRow;
                newData.MicResults[parentIndex] = parent;
            }

            (newData.DiskResults || []).forEach((result, parentIndex) => {
                filterInvalidEmbeddedAstRows(result, newAlertMessages, parentIndex);
            });
            (newData.MicResults || []).forEach((result, parentIndex) => {
                filterInvalidEmbeddedAstRows(result, newAlertMessages, parentIndex);
            });

            queueMicrotask(() => updateSaveableData(newData));
            queueMicrotask(() => scheduleExpertRulesForPageRefresh(newData));
            return newData;
        });

        if (newAlertMessages.length > 0) {
            setAlertMessages(prev => [...prev.filter(a => a.parentIndex !== extraInfo.index), ...newAlertMessages]);
        }
    };
    
    /**
     * Refreshes special-consideration embeds and breakpoint susceptibilities for a disk/MIC line via POST AST/getsusceptibility.
     * Runs when line criteria are complete (antibiotic + guideline + dosage for disk) or when a measurement is present.
     * Attaches id-based {@link buildSusceptibilityRequestMeta} on the Post callback so stale responses can be discarded after row delete.
     */
    const getBreakpointsAndExpertRules = (data, row, specialrow, type, forceRecalculate = false) => {

        const updatedAlertMessages = alertMessages.filter(alert => alert.parentIndex !== row);
        setAlertMessages(updatedAlertMessages);

        if (data.EmbeddedASTRows) {
            data.EmbeddedASTRows = data.EmbeddedASTRows.filter(row => !row.ExpertRuleLine);
        }
    
        const nullableNumericFields = ['Antibiotic', 'ZoneDiameter', 'Mic'];
        const nonNullableNumericFields = ['DrugCategory', 'Dosage', 'Guidelines', 'TestMethod', 'TestResult', 'SpecialConsiderationId', 'OrganismId', 'SpecimenTypeId'];
        const normalized = { ...data };
        nullableNumericFields.forEach(key => {
            if (normalized[key] === '' || normalized[key] === undefined) {
                normalized[key] = null;
            }
        });
        nonNullableNumericFields.forEach(key => {
            if (normalized[key] === '' || normalized[key] === undefined || normalized[key] === null) {
                normalized[key] = 0;
            }
        });
        const resolvedLaboratoryId = props.startConfig?.selectedRecord?.laboratoryid ?? props.startConfig?.selectedRecord?.LaboratoryId ??
            astData?.LaboratoryId ?? astData?.laboratoryid;
        const resolvedOrganismId = organismId ?? 0;
        const updatedData = {
            ...normalized,
            OrganismId: resolvedOrganismId,
            organismId: resolvedOrganismId,
            specimenTypeId: specimenTypeId ?? 0,
            laboratoryId: resolvedLaboratoryId ?? 0,
            ForceRecalculateSusceptibility: forceRecalculate || data.ForceRecalculateSusceptibility === true,
            SusceptibilityOverride: forceRecalculate ? undefined : data.SusceptibilityOverride,
        };

        const hasZoneDiameter = data.ZoneDiameter !== "" && data.ZoneDiameter != null && data.ZoneDiameter !== undefined;
        const hasMic = data.Mic !== "" && data.Mic != null && data.Mic !== undefined;

        if (hasMic)
        {
            const parts = parseMicMeasurementFromRow(data);
            if (parts.hasNumericValue) {
                const numericValue = Number(parts.numeric);
                if (!Number.isNaN(numericValue) && numericValue > 0) {
                    const guidelinesId = Number(data.Guidelines) || 0;
                    const roundedMic = parseFloat(roundToValidMIC(numericValue, guidelinesId, parts.numericString));
                    updatedData.Mic = Number.isNaN(roundedMic) ? numericValue : roundedMic;
                } else if (!Number.isNaN(numericValue)) {
                    updatedData.Mic = numericValue;
                }
                updatedData.Operator = parts.operator || null;
            } else if (parts.operatorOnly) {
                updatedData.Mic = parts.operator;
                updatedData.Operator = parts.operator;
            }
        }

        const hasAntibiotic = data.Antibiotic !== '' && data.Antibiotic != null && data.Antibiotic !== undefined;
        const hasGuidelines = data.Guidelines !== '' && data.Guidelines != null && data.Guidelines !== undefined && data.Guidelines !== 0;
        const hasDosageForDisk =
            data.Dosage !== '' && data.Dosage != null && data.Dosage !== undefined && Number(data.Dosage) !== 0;
        const hasLineCriteriaForDisk =
            specialrow === undefined && type === 'disk' && hasAntibiotic && hasGuidelines && hasDosageForDisk;
        const hasLineCriteriaForMic =
            specialrow === undefined && type === 'mic' && hasAntibiotic && hasGuidelines;
        const hasRequiredForDisk = type === 'disk' && hasZoneDiameter && hasAntibiotic;
        const hasRequiredForMic = type === 'mic' && hasMic && hasAntibiotic;
        if (hasRequiredForDisk || hasRequiredForMic || hasLineCriteriaForDisk || hasLineCriteriaForMic) {
            const rowKey = getSusceptibilityRowKey(type, row, specialrow);
            const generation = (susceptibilitySeqRef.current[rowKey] ?? 0) + 1;
            susceptibilitySeqRef.current[rowKey] = generation;
            const parentRow =
                specialrow !== undefined && specialrow !== null
                    ? (type === 'disk' ? astData?.DiskResults?.[row] : astData?.MicResults?.[row])
                    : null;
            const parentRequestLineKey = parentRow ? buildAstRowLineKey(parentRow) : '';
            const requestMeta = buildSusceptibilityRequestMeta(data, parentRequestLineKey);
            Post(
                'AST/getsusceptibility',
                updatedData,
                susceptibilityDataRetrievedSuccessfully,
                errorWhenRetrievingData,
                {
                    index: row,
                    specialIndex: specialrow,
                    type: type,
                    generation: generation,
                    hadMeasurementOnRequest: hasZoneDiameter || hasMic,
                    forceRecalculate: forceRecalculate || data.ForceRecalculateSusceptibility === true,
                    requestMeta,
                }
            );
        }
    }

    /**
     * Re-fetches expert rule lines for a parent row from the server after apply/unapply (always POSTs, even without measurement).
     * @param {'disk'|'mic'} type
     * @param {number} parentIndex
     * @param {Object} parentRow - Full parent row (Disk/Mic line)
     */
    const refreshParentRowExpertRules = (type, parentIndex, parentRow) => {
        if (!parentRow) {
            return;
        }
        const data = { ...parentRow };
        if (data.EmbeddedASTRows) {
            data.EmbeddedASTRows = data.EmbeddedASTRows.filter((r) => !r.ExpertRuleLine);
        }
        getBreakpointsAndExpertRules(data, parentIndex, undefined, type);
    };

    const alertDataRetrievedSuccessfully = (data) => {
        setAlertMessages(data);
    }

    const resetAlerts = (culture_id) => {
        const criteria = { Parameters: [{ Key: 'id', Value: culture_id }, { Key: 'view', Value: "cultures" }] };
        Post('alert/get', criteria, alertDataRetrievedSuccessfully, errorWhenRetrievingData);
    }

    const dataRetrievedSuccessfully = (data) => {
        
        setSourceData(data);
    }

    const cultureTypeId = props.startConfig?.selectedRecord?.typeid ?? props.startConfig?.selectedRecord?.TypeId ??
        props.startConfig?.selectedRecord?.culturetypeid ?? props.startConfig?.selectedRecord?.CultureTypeId ??
        astData?.CultureTypeId ?? astData?.culturetypeid;

    const laboratoryId = props.startConfig?.selectedRecord?.laboratoryid ?? props.startConfig?.selectedRecord?.LaboratoryId ??
        astData?.LaboratoryId ?? astData?.laboratoryid;

    /**
     * Refetches CultureTests from the backend and updates astData.
     * Called when an isolate test form saves successfully so the panel turns green.
     * Only updates CultureTests; other AST data (DiskResults, MicResults, etc.) is preserved.
     */
    const refetchCultureTests = () => {
        if (!cultureId) return;
        const criteria = { Name: 'getculturetestsforculture', Parameters: [{ Key: 'id', Value: String(cultureId) }] };
        Post('query/filteredget', criteria, (data) => {
            try {
                const parsed = typeof data === 'string' ? JSON.parse(data) : data;
                const newCultureTests = parsed?.CultureTests ?? [];
                setASTData((prev) => {
                    const next = { ...prev, CultureTests: newCultureTests };
                    queueMicrotask(() => scheduleExpertRulesForPageRefresh(next));
                    return next;
                });
            } catch (e) {
                errorWhenRetrievingData(e?.message ?? 'Failed to parse CultureTests response');
            }
        }, errorWhenRetrievingData);
    };

    /**
     * Opens the isolate test form in a nested FormHandler overlay.
     * Uses deferSave: false so tests save when the user clicks Save on the test form.
     * Closing the form returns to AST (does not navigate away).
     * When cultureTestId is available (test exists in CultureTests), uses it directly.
     * When not, calls GetOrCreateCultureTestId to create the record before opening.
     * On save: onSave triggers refetchCultureTests so the panel turns green.
     * On cancel: no refresh occurs; the panel does not change.
     * @param {string} formName - Form name (e.g. from CultureTypeTestOptions)
     * @param {number} [cultureIdToUse] - Culture ID; defaults to current cultureId
     * @param {number} [cultureTestId] - Culturetest ID when test exists; when absent, resolved via GetOrCreateCultureTestId
     */
    const onEditTest = (formName, cultureIdToUse, cultureTestId) => {
        const rec = props.startConfig?.selectedRecord ?? props.startConfig?.currentRecord;
        const cultureIdToUseResolved = cultureIdToUse ?? cultureId;

        const openFormWithId = (idToUse) => {
            setFormStartConfig({
                button: { UIEvent: undefined, OnFinish: "embeddedrefresh" },
                formName: formName,
                id: idToUse,
                view: props.startConfig?.view,
                selectedRecord: rec,
                refresh: (finishAction, idToUseRefresh, extraInfo) => {
                    props.startConfig?.refresh?.(finishAction || "embeddedrefresh", idToUseRefresh, extraInfo);
                },
                onSave: refetchCultureTests,
                deferSave: false,
                setFormStartConfig: setFormStartConfig
            });
        };

        if (cultureTestId != null && cultureTestId !== 0) {
            openFormWithId(cultureTestId);
            return;
        }

        const criteria = {
            Name: 'GetOrCreateCultureTestId',
            Parameters: [
                { Key: 'cultureid', Value: String(cultureIdToUseResolved) },
                { Key: 'testname', Value: formName }
            ]
        };
        Post('query/filteredget', criteria, (data) => {
            const raw = typeof data === 'string' ? JSON.parse(data) : data;
            const id = raw?.Id ?? raw?.id;
            if (id != null && id !== 0) {
                openFormWithId(id);
            } else {
                errorWhenRetrievingData('GetOrCreateCultureTestId returned no id');
            }
        }, errorWhenRetrievingData);
    };

    const deleteRowHandler = (index, specialIndex, type) => {
        removeAntibioticConfirmation(false, type, index, specialIndex);
    }

    const diskTests = (
        <ASTDiskResults
            results={astData.DiskResults}
            changeHandler={ASTChangeHandler}
            deleteHandler={deleteRowHandler}
            addHandler={() => addAntibioticClickHandler('Disk')}
            lists={props.lists}
            highlightedParentIds={highlightedParentIds}
            language={props.language}
            manualRowSuppressedFlags={diskRowSuppressedByExpert}
            includeOnReportLockedByExpertFlags={diskRowIncludeOnReportLockedByExpert}
            expertTriggerMap={expertLineTriggerMap}
            recordSusceptibilityChangeAudit={recordSusceptibilityChangeAudit}
            overrideCannedOptions={overrideCannedOptions}
            onManualOverrideIconClick={handleManualOverrideIconClick}
            onManualOverrideRevert={handleManualOverrideRevert}
            resolveSusceptibilityLabel={resolveSusceptibilityLabel}
        />
    );

    const stripTests = (
        <ASTMicResults
            results={astData.MicResults}
            changeHandler={ASTChangeHandler}
            deleteHandler={deleteRowHandler}
            addHandler={() => addAntibioticClickHandler('Strip')}
            lists={props.lists}
            highlightedParentIds={highlightedParentIds}
            language={props.language}
            manualRowSuppressedFlags={micRowSuppressedByExpert}
            includeOnReportLockedByExpertFlags={micRowIncludeOnReportLockedByExpert}
            expertTriggerMap={expertLineTriggerMap}
            recordSusceptibilityChangeAudit={recordSusceptibilityChangeAudit}
            overrideCannedOptions={overrideCannedOptions}
            onManualOverrideIconClick={handleManualOverrideIconClick}
            onManualOverrideRevert={handleManualOverrideRevert}
            resolveSusceptibilityLabel={resolveSusceptibilityLabel}
        />
    );

    const hasValidData = (entry) => {
        const hasAntibiotic = entry.Antibiotic !== undefined && entry.Antibiotic !== null && entry.Antibiotic !== "" && entry.Antibiotic !== 0;
        const hasGuidelines = entry.Guidelines !== undefined && entry.Guidelines !== null && entry.Guidelines !== "" && entry.Guidelines !== 0;
        const hasTestResult = entry.TestResult !== undefined && entry.TestResult !== null && entry.TestResult !== 0;
        return hasAntibiotic || hasGuidelines || hasTestResult;
    };

    const groupExpertRules = () => {
        const fromApi = astData.ExpertRuleGroups ?? astData.expertRuleGroups;
        if (Array.isArray(fromApi) && fromApi.length > 0) {
            const grouped = {};
            fromApi.forEach((g, expertGroupIndex) => {
                const ruleName = g.RuleName || g.ruleName || 'Unknown';
                const ruleId = g.RuleId ?? g.ruleId;
                const groupKey = ruleId !== undefined && ruleId !== null ? `id:${ruleId}` : `name:${ruleName}`;
                const applyBool = g.ApplyRule ?? g.applyRule;
                const actions = g.Actions || g.actions || [];
                grouped[groupKey] = {
                    ruleName,
                    ruleText: g.RuleText || g.ruleText || '',
                    applyRule: applyBool,
                    ruleId,
                    expertGroupIndex,
                    entries: actions.map((entry) => ({
                        entry: {
                            ...entry,
                            ApplyRule: applyBool ? 'Yes' : 'No',
                            IncludeOnReportEditable:
                                entry.IncludeOnReportEditable === true ||
                                entry.IncludeOnReport === null ||
                                entry.IncludeOnReport === undefined ||
                                entry.IncludeOnReport === ''
                        },
                        type: 'expert',
                        expertGroupIndex,
                        isTopLevel: true
                    }))
                };
            });
            return grouped;
        }

        const grouped = {};
        
        // Process MicResults (top-level and embedded)
        astData.MicResults.forEach((entry, index) => {
            if (entry.ExpertRuleLine && hasValidData(entry)) {
                const ruleName = entry.ExpertRuleName || 'Unknown';
                const groupKey = expertGroupKeyForRow(entry, ruleName);
                if (!grouped[groupKey]) {
                    grouped[groupKey] = {
                        ruleName: ruleName,
                        ruleText: entry.ExpertRuleText || '',
                        ruleId: entry.ExpertRuleId,
                        entries: []
                    };
                }
                grouped[groupKey].entries.push({
                    entry: entry,
                    type: 'mic',
                    index: index,
                    specialIndex: undefined,
                    isTopLevel: true
                });
            }
            
            // Process embedded rows
            if (Array.isArray(entry.EmbeddedASTRows)) {
                entry.EmbeddedASTRows.forEach((embeddedEntry, embeddedIndex) => {
                    if (embeddedEntry.ExpertRuleLine && hasValidData(embeddedEntry)) {
                        const ruleName = embeddedEntry.ExpertRuleName || 'Unknown';
                        const groupKey = expertGroupKeyForRow(embeddedEntry, ruleName);
                        if (!grouped[groupKey]) {
                            grouped[groupKey] = {
                                ruleName: ruleName,
                                ruleText: embeddedEntry.ExpertRuleText || '',
                                ruleId: embeddedEntry.ExpertRuleId,
                                entries: []
                            };
                        }
                        grouped[groupKey].entries.push({
                            entry: embeddedEntry,
                            type: 'mic',
                            index: index,
                            specialIndex: embeddedIndex,
                            isTopLevel: false
                        });
                    }
                });
            }
        });
        
        // Process DiskResults (top-level and embedded)
        astData.DiskResults.forEach((entry, index) => {
            if (entry.ExpertRuleLine && hasValidData(entry)) {
                const ruleName = entry.ExpertRuleName || 'Unknown';
                const groupKey = expertGroupKeyForRow(entry, ruleName);
                if (!grouped[groupKey]) {
                    grouped[groupKey] = {
                        ruleName: ruleName,
                        ruleText: entry.ExpertRuleText || '',
                        ruleId: entry.ExpertRuleId,
                        entries: []
                    };
                }
                grouped[groupKey].entries.push({
                    entry: entry,
                    type: 'disk',
                    index: index,
                    specialIndex: undefined,
                    isTopLevel: true
                });
            }
            
            // Process embedded rows
            if (Array.isArray(entry.EmbeddedASTRows)) {
                entry.EmbeddedASTRows.forEach((embeddedEntry, embeddedIndex) => {
                    if (embeddedEntry.ExpertRuleLine && hasValidData(embeddedEntry)) {
                        const ruleName = embeddedEntry.ExpertRuleName || 'Unknown';
                        const groupKey = expertGroupKeyForRow(embeddedEntry, ruleName);
                        if (!grouped[groupKey]) {
                            grouped[groupKey] = {
                                ruleName: ruleName,
                                ruleText: embeddedEntry.ExpertRuleText || '',
                                ruleId: embeddedEntry.ExpertRuleId,
                                entries: []
                            };
                        }
                        grouped[groupKey].entries.push({
                            entry: embeddedEntry,
                            type: 'disk',
                            index: index,
                            specialIndex: embeddedIndex,
                            isTopLevel: false
                        });
                    }
                });
            }
        });
        
        return grouped;
    };

    const handleGroupApplyRuleChange = (ruleName, applyRuleValue, ruleGroup) => {
        const newData = { ...astData };
        const appliedRulesCopy = new Map(appliedRuleUndoRef.current);

        if (ruleGroup.expertGroupIndex !== undefined) {
            const groups = [...(newData.ExpertRuleGroups || [])];
            const grp = groups[ruleGroup.expertGroupIndex];
            if (grp) {
                const ruleId = grp.RuleId ?? grp.ruleId;
                const actions = grp.Actions || grp.actions || [];
                if (actions.some((a) => isPrintOnReportOnlyExpertAction(a))) {
                    applyPrintOnlyExpertGroupToManualRows(
                        newData,
                        grp,
                        ruleId,
                        applyRuleValue === 'Yes',
                        printOnReportExpertUndoRef
                    );
                }
                const applyStr = applyRuleValue === 'Yes' ? 'Yes' : 'No';
                const nextActions = actions.map((a) => ({
                    ...a,
                    ApplyRule: applyStr
                }));
                groups[ruleGroup.expertGroupIndex] = {
                    ...grp,
                    ApplyRule: applyRuleValue === 'Yes',
                    Actions: nextActions
                };
                newData.ExpertRuleGroups = groups;
            }
            patternRowsRef.current = { disk: null, mic: null };
            setASTData(newData);
            updateSaveableData(newData);
            return;
        }
        
        ruleGroup.entries.forEach(({ entry, type, index, specialIndex, isTopLevel }) => {
            if (isTopLevel) {
                // For top-level entries, just track the ApplyRule state
                // They're already in the results, so we don't need to add/remove them
                const targetArray = type === 'mic' ? newData.MicResults : newData.DiskResults;
                const targetEntry = targetArray[index];
                if (targetEntry) {
                    targetEntry.ApplyRule = applyRuleValue;
                }
            } else {
                // For embedded entries, use the same logic as handleApplyRuleChange
                const ruleKey = `${type}-${index}-${specialIndex}`;
                const parentArray = type === 'mic' ? newData.MicResults : newData.DiskResults;
                const parentRow = parentArray[index];
                
                if (!parentRow) {
                    return; // Parent row doesn't exist
                }

                if (applyRuleValue === 'Yes') {
                    // Apply the rule
                    const expertAntibiotic = entry.Antibiotic;
                    const parentAntibiotic = parentRow.Antibiotic;
                    
                    if (expertAntibiotic === parentAntibiotic) {
                        // Same antibiotic: update parent TestResult
                        const originalTestResult = parentRow.TestResult;
                        parentRow.TestResult = entry.TestResult;
                        
                        // Store original value for undo
                        appliedRulesCopy.set(ruleKey, {
                            originalTestResult: originalTestResult,
                            addedRowIdentifier: null,
                            addedRowType: null
                        });
                    } else {
                        // Different antibiotic: add as new row
                        const newRow = {
                            ...entry,
                            ExpertRuleLine: false, // Remove expert rule flag
                            ApplyRule: 'Yes', // Mark as applied
                            AppliedFromRule: ruleKey // Track where it came from
                        };
                        
                        // Determine which array to add to based on type
                        const targetArray = type === 'disk' ? newData.DiskResults : newData.MicResults;
                        targetArray.push(newRow);
                        
                        // Store identifier for undo
                        appliedRulesCopy.set(ruleKey, {
                            originalTestResult: null,
                            addedRowIdentifier: ruleKey,
                            addedRowType: type
                        });
                    }
                    
                    // Update expert rule row ApplyRule state
                    if (parentRow.EmbeddedASTRows && parentRow.EmbeddedASTRows[specialIndex]) {
                        parentRow.EmbeddedASTRows[specialIndex].ApplyRule = 'Yes';
                    }
                } else {
                    // Unapply the rule
                    const appliedInfo = appliedRulesCopy.get(ruleKey);
                    
                    if (appliedInfo) {
                        if (appliedInfo.addedRowIdentifier !== null) {
                            // Remove added row by finding it using AppliedFromRule
                            const targetArray = appliedInfo.addedRowType === 'disk' ? newData.DiskResults : newData.MicResults;
                            const rowIndex = targetArray.findIndex(row => row.AppliedFromRule === ruleKey);
                            if (rowIndex !== -1) {
                                targetArray.splice(rowIndex, 1);
                            }
                        } else if (appliedInfo.originalTestResult !== null && appliedInfo.originalTestResult !== undefined) {
                            // Restore original TestResult
                            parentRow.TestResult = appliedInfo.originalTestResult;
                        }
                        
                        // Remove from tracking
                        appliedRulesCopy.delete(ruleKey);
                    }
                    
                    // Update expert rule row ApplyRule state
                    if (parentRow.EmbeddedASTRows && parentRow.EmbeddedASTRows[specialIndex]) {
                        parentRow.EmbeddedASTRows[specialIndex].ApplyRule = 'No';
                    }
                }
            }
        });
        
        appliedRuleUndoRef.current = appliedRulesCopy;
        patternRowsRef.current = { disk: null, mic: null };
        setASTData(newData);
        updateSaveableData(newData);

        const parentsToRefresh = new Map();
        ruleGroup.entries.forEach(({ type, index, isTopLevel }) => {
            if (!isTopLevel) {
                parentsToRefresh.set(`${type}-${index}`, { type, index });
            }
        });
        parentsToRefresh.forEach(({ type, index }) => {
            const parentAfter = type === 'disk' ? newData.DiskResults[index] : newData.MicResults[index];
            if (parentAfter) {
                refreshParentRowExpertRules(type, index, parentAfter);
            }
        });
    };

    const groupedRules = groupExpertRules();

    const expertRules = (
        <ASTExpertRules
            groupedRules={groupedRules}
            astData={astData}
            changeHandler={ASTChangeHandler}
            deleteHandler={deleteRowHandler}
            onGroupApplyRuleChange={handleGroupApplyRuleChange}
            setHighlightedParentIds={setHighlightedParentIds}
            lists={props.lists}
            language={props.language}
            expertRuleColourMap={expertRuleColourMap}
        />
    );

    const comments = (
        <ASTComments
            commentOne={ASTCommentOne}
            commentTwo={ASTCommentTwo}
            additionalNotes={ASTAdditionalNotes}
            changeHandler={changeHandler}
        />
    );

    const deleteLinesControl = ( 
        <div className='astform-above-save'>
            {(deleteBlankRows !== null) ? (
            <SingleLineField
                key={deleteBlankRows.Id}
                config={deleteBlankRows}
                changeHandler={changeHandler}
            >
            </SingleLineField>
            ) : null }
        </div>
    );

    const rowRemovalConfirmation = (
        <div>
            <Dialog
                hidden={!showDialog}
                dialogContentProps={dialogContentProps}
                modalProps={modalPropsStyles}
            >
                <DialogFooter>
                    <PrimaryButton onClick={deleteRow} text={TranslateTag("@GenYesA@", props.language)} />
                    <DefaultButton onClick={dontDeleteRow} text={TranslateTag("@GenNo@", props.language)} />
                </DialogFooter>
            </Dialog>
        </div>
    );


    return (
        <div className='astform-content'>
            <div className='astform-title'>
                {props.config.PageTitle}
            </div>
            <div className='astform-headertext'>
                {props.config.Text}
            </div>
            {organismName ? (
                <div className="astform-add-row-top astform-organism-heading-row">
                    <div className="astform-antibiotic-level" />
                    <span className="astform-section-label" id="ast-organism-name">{organismName}</span>
                    {intrinsicGuidanceRules.length > 0 && (
                        <ExpertTriggerIcon
                            triggers={intrinsicGuidanceRules}
                            id="ast-organism-guidance-trigger"
                            language={props.language}
                            ariaLabelTag="@AstOrgGui@"
                        />
                    )}
                </div>
            ) : null}
            <br />
            <AlertList messages={topAlertMessages} position="top"></AlertList>
            {testPattern !== null && testPatternFullList !== null && CompletedDate !== null && CompletedTime !== null && (
                <ASTHeader
                    testPattern={testPattern}
                    testPatternFullList={testPatternFullList}
                    completedDate={CompletedDate}
                    completedTime={CompletedTime}
                    changeHandler={changeHandler}
                    language={props.language}
                />
            )}
            {diskTests}
            {stripTests}
            {((astData.ResistanceMechanisms ?? astData.resistanceMechanisms)?.length > 0) ? (
                <div
                    className="astform-manual-section astform-expert-finding-section"
                    role="region"
                    aria-label={TranslateTag('@AstExpFin@', props.language)}
                >
                    <div className="astform-add-row-top">
                        <div className="astform-antibiotic-level" />
                        <span className="astform-section-label">{TranslateTag('@AstExpFin@', props.language)}</span>
                        <span className="astform-expert-finding-header-spacer" aria-hidden="true" />
                    </div>
                    <div className="astform-resistance-mechanisms-inner">
                        <div className="astform-expert-finding-table-row">
                            <div className="astform-antibiotic-level" aria-hidden="true" />
                            <div className="astform-expert-finding-table-wrap">
                                <table className="astform-resistance-mechanisms-table">
                                    <thead>
                                        <tr>
                                            <th scope="col">{TranslateTag('@AstRmFam@', props.language)}</th>
                                            <th scope="col">{TranslateTag('@AstRmPhe@', props.language)}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {(astData.ResistanceMechanisms ?? astData.resistanceMechanisms).map((row, idx) => (
                                            <tr key={`rm-${idx}-${row.DrugFamily ?? row.drugFamily ?? ''}-${row.PhenoType ?? row.phenoType ?? ''}`}>
                                                <td>{row.DrugFamily ?? row.drugFamily ?? ''}</td>
                                                <td>{row.PhenoType ?? row.phenoType ?? ''}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            ) : null}
            {expertRules}
            <ASTIsolateTests
                cultureId={cultureId}
                cultureTypeId={cultureTypeId}
                specimenTypeId={specimenTypeId}
                laboratoryId={laboratoryId}
                astData={astData}
                onEditTest={onEditTest}
                laboratory={props.laboratory}
                language={props.language}
            />
            {comments}
            <br />
            {rowRemovalConfirmation}
            {deleteLinesControl}
            <AlertList messages={alertMessages} position="bottom"></AlertList>
            <ASTSusceptibilityOverridePanel
                visible={overridePanel != null}
                onDismiss={() => setOverridePanel(null)}
                onSave={handleOverridePanelSave}
                language={props.language}
                auditRequired={recordSusceptibilityChangeAudit === 'Yes'}
                pendingSusceptibilityLabel={
                    overridePanel ? resolveSusceptibilityLabel(overridePanel.pendingTestResult) : ''
                }
                overriddenFromLabel={
                    overridePanel ? resolveSusceptibilityLabel(overridePanel.previousTestResult) : ''
                }
                initialCannedCommentId={overridePanel?.initialCannedCommentId ?? 0}
                initialFreeText={overridePanel?.initialFreeText ?? ''}
                editMode={overridePanel?.mode === 'edit'}
                cannedOptions={overrideCannedOptions}
            />
            <FormHandler startConfig={formStartConfig} />
        </div>
    )
};


const mapStateToProps = state => {
    return {
        lists: state.config.lists,
        preferences: state.config.preferences,
        laboratory: state.config.laboratory
    };
}

export default connect(mapStateToProps)(AST);