-- index any foreign keys!
CREATE TABLE snittlistan.sent_email (
    sent_email_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    from_email varchar(100) NOT NULL,
    to_email varchar(100) NOT NULL,
    bcc_email varchar(100) NOT NULL,
    subject varchar(100) NOT NULL,
    data JSONB NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
