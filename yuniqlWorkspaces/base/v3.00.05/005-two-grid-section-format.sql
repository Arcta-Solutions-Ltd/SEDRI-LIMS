-- A report section places one table per grid it binds, and each table takes its geometry from a grid
-- position on the section's format. Every shipped format defined a single grid position, so a test
-- with more than one fieldgrid had nowhere to put the second one and its rows were gathered into the
-- first grid behind an extra column holding the grid's name.
--
-- Add a format with two grid positions, then move any section that binds more grids than its format
-- can place onto a format that has enough. The pass is generic: it reads the grid counts out of the
-- stored documents rather than naming any section or test. Sections seeded with '{}' contents take
-- their definition from code and are left alone here.
--
-- Idempotent: safe to re-run.

-- The description is plain text rather than a language catalogue token. An untranslated token renders
-- as an empty string, and there is no catalogue entry for this format; formats created in the report
-- designer carry plain text descriptions in the same way.
INSERT INTO configs (Id, ConfigName, ConfigTypeId, Contents, LastModifiedDate)
OVERRIDING SYSTEM VALUE
SELECT
    128,
    'doublecolumnwithtwogrids',
    23,
    '{
    "Name": "DoubleColumnWithTwoGrids",
    "Type": "DoubleFieldColumn",
    "Description": "Two Column With Two Grids Format",
    "Columns": [
        {
            "left": 100,
            "width": 180,
            "labelwidth": 100
        },
        {
            "left": 300,
            "width": 180,
            "labelwidth": 100
        }
    ],
    "Grids": [
        {
            "left": 100,
            "width": "300|80"
        },
        {
            "left": 100,
            "width": "300|80"
        }
    ]
}',
    NOW ()
WHERE NOT EXISTS (
    SELECT 1 FROM configs WHERE lower(ConfigName) = 'doublecolumnwithtwogrids'
);

-- Keep the identity sequence ahead of the explicitly inserted id.
SELECT setval(
    pg_get_serial_sequence('configs', 'id'),
    GREATEST((SELECT MAX(Id) FROM configs), 1),
    true
);

-- Move sections onto a format that can place every grid they bind. ConfigTypeId 21 is still read for
-- formats alongside 23, because a database that has not run 004 keeps its formats on the old type.
WITH format_capacity AS (
    SELECT
        lower(ConfigName) AS format_name,
        COALESCE(NULLIF(Contents::jsonb ->> 'Name', ''), ConfigName) AS format_reference,
        COALESCE(jsonb_array_length(Contents::jsonb -> 'Grids'), 0) AS grid_positions,
        COALESCE(jsonb_array_length(Contents::jsonb -> 'Columns'), 0) AS column_positions
    FROM configs
    WHERE ConfigTypeId IN (21, 23)
      AND jsonb_typeof(Contents::jsonb) = 'object'
      AND jsonb_typeof(Contents::jsonb -> 'Grids') = 'array'
),
section_need AS (
    SELECT
        Id AS section_id,
        lower(Contents::jsonb ->> 'Format') AS current_format,
        jsonb_array_length(Contents::jsonb -> 'Grids') AS bound_grids
    FROM configs
    WHERE ConfigTypeId IN (14, 15)
      AND jsonb_typeof(Contents::jsonb) = 'object'
      AND jsonb_typeof(Contents::jsonb -> 'Grids') = 'array'
      AND NULLIF(Contents::jsonb ->> 'Format', '') IS NOT NULL
),
short_sections AS (
    SELECT s.section_id, s.bound_grids, f.column_positions
    FROM section_need s
    JOIN format_capacity f ON f.format_name = s.current_format
    WHERE s.bound_grids > f.grid_positions
),
replacement AS (
    SELECT
        s.section_id,
        (
            SELECT f.format_reference
            FROM format_capacity f
            WHERE f.grid_positions >= s.bound_grids
            -- Smallest format that still fits, preferring one laid out like the section's current
            -- format so the fields do not move at the same time as the grids.
            ORDER BY f.grid_positions, abs(f.column_positions - s.column_positions), f.format_name
            LIMIT 1
        ) AS new_format
    FROM short_sections s
)
UPDATE configs c
SET Contents = jsonb_set(c.Contents::jsonb, '{Format}', to_jsonb(r.new_format))::json,
    LastModifiedDate = now()
FROM replacement r
WHERE c.Id = r.section_id
  AND r.new_format IS NOT NULL;
