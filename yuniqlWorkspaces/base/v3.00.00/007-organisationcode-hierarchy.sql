WITH RECURSIVE recursive_hierarchy AS (
    SELECT
        id,
        parentorganisationid,
        organisationname,
		code,
        '{"organisationcodehierarchy" : "' || code::text AS path
    FROM organisation
    WHERE parentorganisationid IS NULL

    UNION ALL

    SELECT
        h.id,
        h.parentorganisationid,
        h.organisationname,
		h.code,
        rh.path || ' : ' || h.code AS path
    FROM organisation h
    JOIN recursive_hierarchy rh ON h.parentorganisationid = rh.id
)

Update organisation set moredata = moredata || recur.moredatapath FROM 
(SELECT id, organisationname, cast(path || '"}' As jsonb) as moredatapath FROM recursive_hierarchy) as recur
where organisation.id = recur.id;
