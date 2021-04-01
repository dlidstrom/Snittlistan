CREATE SCHEMA log;

CREATE TABLE log.request (
    request_id INT GENERATED ALWAYS AS IDENTITY,
    url VARCHAR(255) NOT NULL,
    method VARCHAR(255) NOT NULL,
    body TEXT NOT NULL,
    created_utc TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    PRIMARY KEY (request_id)
);

CREATE TABLE log.response (
    response_id INT GENERATED ALWAYS AS IDENTITY,
    status_code INT NOT NULL,
    body TEXT NOT NULL,
    content_length INT NOT NULL,
    content_type INT NOT NULL,
    created_utc TIMESTAMP WITHOUT TIME ZONE,
    PRIMARY KEY (response_id)
);
