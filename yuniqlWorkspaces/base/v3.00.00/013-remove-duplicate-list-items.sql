-- Set the target list id (e.g., 54)
DO $$
DECLARE
  v_target_listid int := 54;
BEGIN
  -- 1) Build mappings of duplicate listitem ids to a single canonical id per (listid, value)
  CREATE TEMP TABLE tmp_mappings AS
  WITH dupes AS (
    SELECT
      li.listid,
      btrim(li.value) AS value_norm,
      MIN(li.id) AS keep_id,
      ARRAY_AGG(li.id) AS all_ids
    FROM listitem li
    WHERE li.listid = v_target_listid
    GROUP BY li.listid, btrim(li.value)
    HAVING COUNT(*) > 1
  )
  SELECT
    d.listid,
    d.value_norm,
    d.keep_id,
    UNNEST(d.all_ids) AS old_id
  FROM dupes d;

  -- 2) Repoint specimen.specimenappearanceid to canonical id
  UPDATE specimen s
  SET specimenappearanceid = m.keep_id
  FROM tmp_mappings m
  WHERE s.specimenappearanceid = m.old_id;

  -- 3) Repoint child links in listitemparentchild
  UPDATE listitemparentchild pc
  SET childid = m.keep_id
  FROM tmp_mappings m
  WHERE pc.childid = m.old_id;

  -- 4) Remove duplicate (parentid, childid) pairs created by the update
  DELETE FROM listitemparentchild pc
  USING listitemparentchild pc2
  WHERE pc.id > pc2.id
    AND pc.parentid = pc2.parentid
    AND pc.childid = pc2.childid;

  -- 5) Delete redundant listitem rows (keep only canonical)
  DELETE FROM listitem li
  USING tmp_mappings m
  WHERE li.id = m.old_id
    AND m.old_id <> m.keep_id;

  DROP TABLE IF EXISTS tmp_mappings;
END $$;
