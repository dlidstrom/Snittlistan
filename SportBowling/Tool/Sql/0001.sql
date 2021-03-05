CREATE TABLE division (
    division_id SERIAL PRIMARY KEY,
    external_division_id VARCHAR(10) NOT NULL,
    division_name TEXT NOT NULL
);
