CREATE SCHEMA log;

CREATE TABLE log.request (
    request_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    hash BYTEA NOT NULL,
    url VARCHAR(255) NOT NULL,
    method VARCHAR(255) NOT NULL,
    request_body TEXT NOT NULL,
    request_utc TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    response_utc TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    response_body TEXT NOT NULL,
    status_code INT NOT NULL,
    content_length INT NOT NULL,
    content_type INT NOT NULL
);
CREATE INDEX "request_hash" ON "log"."request"("hash");
