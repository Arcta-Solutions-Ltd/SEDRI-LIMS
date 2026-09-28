-- InstrumentStatus (list id 123): legacy inserts (e.g. v0.00.33 instrument support) omitted list.deleted.
-- GetListValuesQuery filters with l.deleted = false and li.deleted = false; NULL does not satisfy that, so the dropdown had no options.

UPDATE list
SET deleted = false,
    lastmodifieddate = now()
WHERE id = 123;

UPDATE listitem
SET deleted = false,
    lastmodifieddate = now()
WHERE listid = 123;
