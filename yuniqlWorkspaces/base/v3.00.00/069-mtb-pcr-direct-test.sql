-- MTB PCR direct test: lists, form configs, and specimen-type direct-test defaults (specimen list / add test).
--
-- Do not use draft ids 139-140 / 1256-1263 from standalone SQL snippets:
--   * List 139-140 are already assigned in 064-add-exportschedule-frequency-list.sql and
--     066-add-exportschedule-changes-to-include-list.sql (ExportScheduleFrequency / ExportScheduleChangesToInclude).
--   * ListItem 1256-1263 are already used in v0.00.0/003-app-data/021-iqc-test-profile-qc-antibiotics.sql (list 50).
-- This migration therefore uses list 141-142, listitem 1531-1538 (after 066’s 1529-1530), and the same config JSON.


INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 141, 'MTB PCR', 'Custom', null, true, 'MTB PCR', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 141 OR lower(name) = lower('MTB PCR'));

INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 142, 'MTB PCR Analyte Results', 'Custom', null, true, 'MTB PCR Analyte Results', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 142 OR name = 'MTB PCR Analyte Results');


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1531, 141, 'DETECTED', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1531);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1532, 141, 'NOT DETECTED', false, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1532);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1533, 141, 'DETECTED-LOW', false, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1533);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1534, 141, 'DETECTED MEDIUM', false, true, 4, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1534);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1535, 141, 'INVALID', false, true, 5, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1535);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1536, 142, 'Positive', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1536);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1537, 142, 'Negative', false, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1537);


INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1538, 142, 'Invalid', false, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1538);

