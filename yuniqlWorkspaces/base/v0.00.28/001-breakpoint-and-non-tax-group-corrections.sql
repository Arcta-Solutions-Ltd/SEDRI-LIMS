/* Correct EUCAST & CLSI handling of Staphylococcus argenteus */

update breakpoint set orderid = 9, familyid = 9, organismid = 58880, orggroupcodingid = 0 where id = 250;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1253;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1254;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1255;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1257;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1258;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1259;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1314;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1315;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1318;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1319;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1320;
update breakpoint set orderid = 0, familyid = 0, organismid = 0, orggroupcodingid = 2060 where id = 1321;

delete from organismcoding where id = 100832;
delete from organismcoding where id = 100883;

insert into organismcoding values (103764, '', 58878, 2010, now());
insert into organismcoding values (103765, '', 58878, 2025, now());
