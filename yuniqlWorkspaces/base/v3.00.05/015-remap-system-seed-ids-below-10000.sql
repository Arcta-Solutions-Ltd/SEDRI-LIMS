-- Remap yuniql system seed rows that were assigned ids >= 10000 (user-reserved range).
--
-- Sources:
--   v0.00.33/004-comment-changes-final.sql — CannedComments (list 127) inserted without explicit ids
--   v3.00.00/071-add-specimen-state-graph-analytics-menu.sql — configs.specimenstategraphuievent
--   v3.00.00/079-homedashboard-tat-compliance-query.sql — configs.homedashboardtatcompliance
--   v3.00.03/006-existing-field-reuse-and-patient-view.sql — configs.patients
--
-- Idempotent: safe to re-run. Matches by id, never by translated listitem value.
-- Cypress test SQL under arcportal/cypress/data/ is out of scope (simulates user data).

DO $$
DECLARE
  v_listitem_base int;
  v_config_base int;
BEGIN
  -- -------------------------------------------------------------------------
  -- 1. CannedComments (list 127): listitem ids >= 10000 -> below 10000
  -- -------------------------------------------------------------------------
  IF EXISTS (
    SELECT 1 FROM listitem WHERE listid = 127 AND id >= 10000
  ) THEN
    SELECT GREATEST(COALESCE(MAX(id), 0), 1233)
    INTO v_listitem_base
    FROM listitem
    WHERE id < 10000;

    CREATE TEMP TABLE tmp_canned_comment_id_map ON COMMIT DROP AS
    SELECT
      li.id AS old_id,
      v_listitem_base + ROW_NUMBER() OVER (ORDER BY li.id)::int AS new_id
    FROM listitem li
    WHERE li.listid = 127
      AND li.id >= 10000;

    -- Drop mappings whose target id is already taken (re-run / collision safety).
    DELETE FROM tmp_canned_comment_id_map m
    USING listitem existing
    WHERE existing.id = m.new_id
      AND existing.id <> m.old_id;

    UPDATE specimencomment sc
    SET cannedcommentid = m.new_id
    FROM tmp_canned_comment_id_map m
    WHERE sc.cannedcommentid = m.old_id;

    UPDATE listitemparentchild lpc
    SET childid = m.new_id
    FROM tmp_canned_comment_id_map m
    WHERE lpc.childid = m.old_id;

    UPDATE listitemparentchild lpc
    SET parentid = m.new_id
    FROM tmp_canned_comment_id_map m
    WHERE lpc.parentid = m.old_id;

    INSERT INTO listitem (
      id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate
    )
    OVERRIDING SYSTEM VALUE
    SELECT
      m.new_id,
      li.listid,
      li.value,
      li.fixed,
      li.enabled,
      li.displayorder,
      li.deleted,
      li.lastmodifieddate
    FROM listitem li
    INNER JOIN tmp_canned_comment_id_map m ON m.old_id = li.id
    WHERE NOT EXISTS (SELECT 1 FROM listitem target WHERE target.id = m.new_id);

    DELETE FROM listitem li
    USING tmp_canned_comment_id_map m
    WHERE li.id = m.old_id;
  END IF;

  -- -------------------------------------------------------------------------
  -- 2. Configs: known seed rows with id >= 10000 -> below 10000
  -- -------------------------------------------------------------------------
  IF EXISTS (
    SELECT 1
    FROM configs
    WHERE id >= 10000
      AND lower(configname) IN (
        'specimenstategraphuievent',
        'homedashboardtatcompliance',
        'patients'
      )
  ) THEN
    -- Remove high-id duplicates when the same configname already exists below 10000.
    DELETE FROM configs high_row
    WHERE high_row.id >= 10000
      AND lower(high_row.configname) IN (
        'specimenstategraphuievent',
        'homedashboardtatcompliance',
        'patients'
      )
      AND EXISTS (
        SELECT 1
        FROM configs low_row
        WHERE low_row.id < 10000
          AND lower(low_row.configname) = lower(high_row.configname)
      );

    SELECT COALESCE(MAX(id), 0)
    INTO v_config_base
    FROM configs
    WHERE id < 10000;

    CREATE TEMP TABLE tmp_config_id_map ON COMMIT DROP AS
    SELECT
      c.id AS old_id,
      v_config_base + ROW_NUMBER() OVER (ORDER BY c.id)::int AS new_id
    FROM configs c
    WHERE c.id >= 10000
      AND lower(c.configname) IN (
        'specimenstategraphuievent',
        'homedashboardtatcompliance',
        'patients'
      );

    DELETE FROM tmp_config_id_map m
    USING configs existing
    WHERE existing.id = m.new_id
      AND lower(existing.configname) <> (
        SELECT lower(c2.configname) FROM configs c2 WHERE c2.id = m.old_id
      );

    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'configshistory') THEN
      UPDATE configshistory ch
      SET configsid = m.new_id
      FROM tmp_config_id_map m
      WHERE ch.configsid = m.old_id;
    END IF;

    INSERT INTO configs (id, configname, configtypeid, contents, lastmodifieddate)
    OVERRIDING SYSTEM VALUE
    SELECT
      m.new_id,
      c.configname,
      c.configtypeid,
      c.contents,
      c.lastmodifieddate
    FROM configs c
    INNER JOIN tmp_config_id_map m ON m.old_id = c.id
    WHERE NOT EXISTS (SELECT 1 FROM configs target WHERE target.id = m.new_id);

    DELETE FROM configs c
    USING tmp_config_id_map m
    WHERE c.id = m.old_id;
  END IF;
END $$;

-- Set identity sequences to the user-reserved boundary (10000) or one above the
-- highest id already in use, whichever is greater (existing deployments may
-- already have user-created rows >= 10000).
SELECT setval(
  pg_get_serial_sequence('listitem', 'id'),
  GREATEST((SELECT COALESCE(MAX(id), 0) FROM listitem) + 1, 10000),
  false
);

SELECT setval(
  pg_get_serial_sequence('configs', 'id'),
  GREATEST((SELECT COALESCE(MAX(id), 0) FROM configs) + 1, 10000),
  false
);
