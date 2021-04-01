-- todo: decide on a schema
CREATE TABLE bits.request (
    request_id INT GENERATED ALWAYS AS IDENTITY,
    url VARCHAR(255) NOT NULL,
    method VARCHAR(255) NOT NULL,
    body TEXT NOT NULL,
    created_utc timestamp without time zone NOT NULL,
    PRIMARY KEY (request_id)
);
