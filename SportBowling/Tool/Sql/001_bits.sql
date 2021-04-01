CREATE SCHEMA bits;

CREATE TABLE bits.season (
    season_id INT GENERATED ALWAYS AS IDENTITY,
    start_year INT NOT NULL UNIQUE,
    end_year INT NOT NULL,
    PRIMARY KEY (season_id)
);

CREATE TABLE bits.division (
    division_id INT GENERATED ALWAYS AS IDENTITY,
    season_id INT NOT NULL,
    external_division_id INT NOT NULL UNIQUE,
    division_name VARCHAR(255) NOT NULL,
    created_utc TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    PRIMARY KEY (division_id),
    CONSTRAINT fk_season
        FOREIGN KEY (season_id)
            REFERENCES bits.season(season_id)
);
