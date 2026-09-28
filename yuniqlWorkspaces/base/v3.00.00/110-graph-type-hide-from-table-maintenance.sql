-- GraphType (list 14): hide from Table Maintenance (Tables view).
-- Table Maintenance uses CommonListQuery (common = true). Graph types are system-defined
-- and must not be edited by users.
UPDATE list SET common = false
WHERE id = 14 OR name = 'GraphType';
