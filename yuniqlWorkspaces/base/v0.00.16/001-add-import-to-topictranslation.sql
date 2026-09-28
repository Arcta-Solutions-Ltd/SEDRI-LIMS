delete from TopicTranslation where id = 24;

insert into TopicTranslation(Id, listitemname, displayedtopic)
overriding system value
values 
( 24, 'Import', 'Import');

ALTER SEQUENCE TopicTranslation_id_seq RESTART WITH 25;
