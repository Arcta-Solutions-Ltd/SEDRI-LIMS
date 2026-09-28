-- Logical table for the event (from EventConfig), not the Event DB table row.
ALTER TABLE queue ADD COLUMN IF NOT EXISTS tablename VARCHAR(128);

-- Backfill from Message JSON (EventModel.Table / table) where column was never set.
UPDATE queue
SET tablename = lower(trim(COALESCE(Message::jsonb->>'Table', Message::jsonb->>'table')))
WHERE tablename IS NULL
  AND trim(COALESCE(Message::jsonb->>'Table', Message::jsonb->>'table', '')) <> '';
