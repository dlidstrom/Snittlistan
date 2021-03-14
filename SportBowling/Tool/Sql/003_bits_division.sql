CREATE TABLE bits.division (
    division_id SERIAL PRIMARY KEY,
    external_division_id INT NOT NULL UNIQUE,
    division_name VARCHAR(255) NOT NULL,
    created_utc TIMESTAMP WITHOUT TIME ZONE NOT NULL
);
