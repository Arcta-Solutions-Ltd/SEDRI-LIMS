-- Revert the Neoshield CROSS JOIN that linked every non-blood neonatal specimen type to every
-- anatomical site (1907-1930) added in v3.00.03/006-neoshield-admission-request.sql.
-- Legacy sparse type-site pairs (seeded in v0.00.00/003-list-items.sql) are preserved.

DELETE FROM listitemparentchild
WHERE childid BETWEEN 1907 AND 1930
  AND parentid IN (809, 810, 811, 813, 814, 815, 818, 820,
                   1900, 1901, 1902, 1903, 1904, 1905);

-- Sites with no parent are unrestricted and appear under every specimen type (IsOptionUnderParent).
-- Disable neonatal anatomical sites until clinically appropriate sparse mappings are defined.
UPDATE listitem
SET enabled = false, lastmodifieddate = now()
WHERE id BETWEEN 1907 AND 1930
  AND listid = 5
  AND deleted = false;
