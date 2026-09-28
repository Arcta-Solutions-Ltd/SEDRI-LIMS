delete from ListItem where id >= 2053 and id <= 2075;

INSERT INTO ListItem
(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
VALUES 
( 2053, 77, 'Enterobacterales (- Citrobacter spp., Providencia spp. & Enterobacter spp.)', null, now(), false, true, 1, false),
( 2054, 77, 'Enterobacterales (- Providencia spp.)', null, now(), false, true, 1, false),
( 2055, 77, 'Enterobacterales (- Morganella spp.)', null, now(), false, true, 1, false),
( 2056, 77, 'Aerococcus urinae, Aerococcus viridans & Aerococcus sanguinicola', null, now(), false, true, 1, false),
( 2057, 77, 'Aeromonas spp. subset', null, now(), false, true, 1, false),
( 2058, 77, 'Coryneform genera', null, now(), false, true, 1, false),
( 2059, 77, 'Micrococcus spp., Kocuria spp., Nesterenkonia spp., Dermacoccus spp. & Kytococcus spp.', null, now(), false, true, 1, false),
( 2060, 77, 'S.aureus & S.argenteus', null, now(), false, true, 1, false),
( 2061, 77, 'Bacteroides spp., Parabacteroides spp., Phocaeicola dorei & Phocaeicola vulgatus', null, now(), false, true, 1, false),
( 2062, 77, 'V.alginolyticus, V.cholerae, V.fluvialis, V.parahaemolyticus & V.vulnificus', null, now(), false, true, 1, false),
( 2063, 77, 'Bacillus spp. (- anthracis)', null, now(), false, true, 1, false),
( 2064, 77, 'Bacillus spp. (- anthracis), Brevibacillus, Cohnella, Lysinibacillus, Paenibacillus & Sporolactobacillus', null, now(), false, true, 1, false),
( 2065, 77, 'Enterobacterales (- Morganella morganii, Proteus spp.& Serratia spp.)', null, now(), false, true, 1, false),
( 2066, 77, 'S.pseudintermedius, S.intermedius, S.schleiferi & S.coagulans', null, now(), false, true, 1, false),
( 2067, 77, 'Enterococcus spp. (- casseliflavus & gallinarum)', null, now(), false, true, 1, false),
( 2068, 77, 'Enterobacterales (- Salmonella spp. & Shigella spp.)', null, now(), false, true, 1, false),
( 2069, 77, 'Enterobacterales (- Morganellaceae, Salmonella spp. & Shigella spp.)', null, now(), false, true, 1, false),
( 2070, 77, 'Enterobacterales (- Citrobacter spp., Providencia spp., Enterobacter spp., Salmonella spp. & Shigella spp.)', null, now(), false, true, 1, false),
( 2071, 77, 'Enterobacterales (- Morganella spp., Salmonella spp. & Shigella spp.)', null, now(), false, true, 1, false),
( 2072, 77, 'Enterobacterales (- Providencia spp., Salmonella spp. & Shigella spp.)', null, now(), false, true, 1, false),
( 2073, 77, 'Salmonella spp. & Shigella spp.', null, now(), false, true, 1, false),
( 2074, 77, 'Typhoidal Salmonella', null, now(), false, true, 1, false),
( 2075, 77, 'Non-typhoidal Salmonella', null, now(), false, true, 1, false);
