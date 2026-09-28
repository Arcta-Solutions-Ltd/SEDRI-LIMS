CREATE TABLE images (
   id SERIAL PRIMARY KEY,
   imagedata BYTEA,
   name VARCHAR(30),
   format VARCHAR(5),
   description VARCHAR(255),
   lastmodifieddate TIMESTAMPTZ NOT NULL
);

ALTER SEQUENCE images_id_seq restart with 1000000;
