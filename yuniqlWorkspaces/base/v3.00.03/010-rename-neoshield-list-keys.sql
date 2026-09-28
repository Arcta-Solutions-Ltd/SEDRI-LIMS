-- Rename Neoshield list-backed MoreData keys to the standard {Field}Id convention.
-- Only moves values when the old key exists and the new key does not.

UPDATE admission
SET moredata = (moredata - 'AdmissionDateTimeKnown')
    || jsonb_build_object('AdmissionDateTimeKnownId', moredata->'AdmissionDateTimeKnown')
WHERE moredata ? 'AdmissionDateTimeKnown'
  AND NOT (moredata ? 'AdmissionDateTimeKnownId');

UPDATE request
SET moredata = (moredata - 'CotAvailable')
    || jsonb_build_object('CotAvailableId', moredata->'CotAvailable')
WHERE moredata ? 'CotAvailable'
  AND NOT (moredata ? 'CotAvailableId');

UPDATE request
SET moredata = (moredata - 'WeightKnown')
    || jsonb_build_object('WeightKnownId', moredata->'WeightKnown')
WHERE moredata ? 'WeightKnown'
  AND NOT (moredata ? 'WeightKnownId');

UPDATE request
SET moredata = (moredata - 'AntibioticReceived')
    || jsonb_build_object('AntibioticReceivedId', moredata->'AntibioticReceived')
WHERE moredata ? 'AntibioticReceived'
  AND NOT (moredata ? 'AntibioticReceivedId');

UPDATE request
SET moredata = (moredata - 'AntibioticStartKnown')
    || jsonb_build_object('AntibioticStartKnownId', moredata->'AntibioticStartKnown')
WHERE moredata ? 'AntibioticStartKnown'
  AND NOT (moredata ? 'AntibioticStartKnownId');

UPDATE request
SET moredata = (moredata - 'CentralLineInPlace')
    || jsonb_build_object('CentralLineInPlaceId', moredata->'CentralLineInPlace')
WHERE moredata ? 'CentralLineInPlace'
  AND NOT (moredata ? 'CentralLineInPlaceId');
