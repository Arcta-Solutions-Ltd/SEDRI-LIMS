-- Reorder Neoshield form pages so direct test selection is last.
-- C# form configs already define the new order; this updates stored overrides in configs
-- where a site customised page order through Configuration > Views.

UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{pages}',
    swapped.pages
)
FROM (
    SELECT
        c2.id,
        (
            SELECT jsonb_agg(
                CASE
                    WHEN elem #>> '{}' = 'testselectionpage' THEN to_jsonb('neoshieldbottlepage'::text)
                    WHEN elem #>> '{}' = 'neoshieldbottlepage' THEN to_jsonb('testselectionpage'::text)
                    ELSE elem
                END
                ORDER BY ord
            )
            FROM jsonb_array_elements(c2.contents -> 'pages') WITH ORDINALITY AS t(elem, ord)
        ) AS pages
    FROM configs c2
    WHERE c2.configname IN ('createneoshieldspecimenform', 'createneoshieldspecimenforpatientform')
      AND c2.contents IS NOT NULL
      AND c2.contents != '{}'::jsonb
      AND c2.contents ? 'pages'
      AND (
          SELECT MIN(ord)
          FROM jsonb_array_elements(c2.contents -> 'pages') WITH ORDINALITY AS t(p, ord)
          WHERE p #>> '{}' = 'testselectionpage'
      ) < (
          SELECT MIN(ord)
          FROM jsonb_array_elements(c2.contents -> 'pages') WITH ORDINALITY AS t(p, ord)
          WHERE p #>> '{}' = 'neoshieldbottlepage'
      )
) swapped
WHERE c.id = swapped.id;

UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Pages}',
    swapped.pages
)
FROM (
    SELECT
        c2.id,
        (
            SELECT jsonb_agg(
                CASE
                    WHEN elem #>> '{}' = 'testselectionpage' THEN to_jsonb('neoshieldbottlepage'::text)
                    WHEN elem #>> '{}' = 'neoshieldbottlepage' THEN to_jsonb('testselectionpage'::text)
                    ELSE elem
                END
                ORDER BY ord
            )
            FROM jsonb_array_elements(c2.contents -> 'Pages') WITH ORDINALITY AS t(elem, ord)
        ) AS pages
    FROM configs c2
    WHERE c2.configname IN ('createneoshieldspecimenform', 'createneoshieldspecimenforpatientform')
      AND c2.contents IS NOT NULL
      AND c2.contents != '{}'::jsonb
      AND c2.contents ? 'Pages'
      AND (
          SELECT MIN(ord)
          FROM jsonb_array_elements(c2.contents -> 'Pages') WITH ORDINALITY AS t(p, ord)
          WHERE p #>> '{}' = 'testselectionpage'
      ) < (
          SELECT MIN(ord)
          FROM jsonb_array_elements(c2.contents -> 'Pages') WITH ORDINALITY AS t(p, ord)
          WHERE p #>> '{}' = 'neoshieldbottlepage'
      )
) swapped
WHERE c.id = swapped.id;
