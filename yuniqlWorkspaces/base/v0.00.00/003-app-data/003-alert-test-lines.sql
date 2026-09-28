-- 2026-08-03 These are unused for new deployments since expert rules changes
-- INSERT INTO alerttestlines
-- (id, alertid, testname, fieldname, comparison, compvalue, lastmodifieddate)
-- VALUES
-- (1, 20, 'betalactamasetestform', 'betalactamaseresultid', '=', '800', now());
ALTER SEQUENCE alerttestlines_id_seq
RESTART WITH 10000;
