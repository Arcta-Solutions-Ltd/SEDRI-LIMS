-- Expert rule actions must target either a single antibiotic or an antibiotic group, never both.
-- Clear legacy dual-target rows (group wins, matching load/save normalization).
UPDATE expertruleaction
SET antibioticid = NULL
WHERE COALESCE(antibioticgroupid, 0) > 0
  AND COALESCE(antibioticid, 0) > 0;

ALTER TABLE expertruleaction
ADD CONSTRAINT expertruleaction_antibiotic_xor_group
CHECK (
    NOT (COALESCE(antibioticid, 0) > 0 AND COALESCE(antibioticgroupid, 0) > 0)
);
