CREATE TABLE listitemparentchild
(
    id SERIAL PRIMARY KEY,
    parentid INTEGER NOT NULL,
    childid INTEGER NOT NULL,
    displayorder INTEGER,
    lastmodifieddate TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

INSERT INTO listitemparentchild 
    (parentid, childid, displayorder, lastmodifieddate)
SELECT 
    parentid,
    id as childid,
    COALESCE(displayorder, 0) as displayorder,
    lastmodifieddate as lastmodifieddate
FROM listitem 
WHERE parentid IS NOT NULL
  AND deleted = false 
ORDER BY parentid, displayorder, id;

ALTER TABLE listitem DROP COLUMN parentid; 
