-- Add antibiotic group multiselect column to laboratory table.
-- Stores comma-separated listitem IDs from the AntibioticGroup list (list id 82).
ALTER TABLE laboratory ADD antibioticgroupids VARCHAR(200);

UPDATE laboratory SET antibioticgroupids = '' WHERE antibioticgroupids IS NULL;
