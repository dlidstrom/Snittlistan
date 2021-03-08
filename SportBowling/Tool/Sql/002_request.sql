CREATE TABLE bits.request (
    request_id SERIAL PRIMARY KEY,
    url VARCHAR(255) NOT NULL,
    method VARCHAR(255) NOT NULL,
    body TEXT NOT NULL,
    created_utc timestamp without time zone NOT NULL
);
