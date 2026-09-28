alter table culture 
add column ParentCultureId int;

update culture set ParentCultureId = id;
